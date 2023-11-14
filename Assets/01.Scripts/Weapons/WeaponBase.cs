using System;
using UnityEngine;
using Ramdom = UnityEngine.Random;
using CharacterController = TopdownShooter.Characters.CharacterController;

namespace TopdownShooter.Weapons
{
	public enum WeaponState
	{
		None,
		Attackable,
		AmmoEmpty,
		Reload,
	}


	public abstract class WeaponBase : MonoBehaviour, IWeapon, IAmmo
	{
		[HideInInspector] public WeaponState weaponState;
		[SerializeField] WeaponType _wepaonType;
		[SerializeField] AmmoType _ammoType;
		[SerializeField] private float _attackCoolTime;
		[SerializeField] private float _reloadCoolTime;
		[SerializeField] private float _minDamage;
		[SerializeField] private float _maxDamage;

		public SpriteRenderer renderer { private set; get; }

		public bool isUseAmmo;

		protected CharacterController owner;

		protected LayerMask targetMask;

		public float damage => UnityEngine.Random.Range(_minDamage, _maxDamage);

		public float attackCoolTime => _attackCoolTime;
		public float reloadCoolTime => _reloadCoolTime;

		public bool CanAttack => ammoValue > minAmmo && weaponState == WeaponState.Attackable;

		#region events
		//IWeapon
		public event Action onAttack;
		public event Action onReloadEnter;
		public event Action onReloadExit;
		//IAmmo
		public event Action<int> onChangeAmmo;
		public event Action<int> onUseAmmo;
		public event Action<int> onAddAmmo;
		public event Action onMinAmmo;
		public event Action onMaxAmmo;
		#endregion

		#region types
		public WeaponType weaponType => _wepaonType;

		public AmmoType ammoType => _ammoType;
		#endregion

		#region ammo
		[SerializeField] private int _maxAmmo;
		public int maxAmmo => _maxAmmo;

		[SerializeField] private int _ammoValue;
		public int ammoValue
		{
			get
			{
				return _ammoValue;
			}
			set
			{
				if (value != _ammoValue)
					onChangeAmmo?.Invoke(value);

				_ammoValue = Mathf.Clamp(value, minAmmo, maxAmmo);

				if (minAmmo == _ammoValue)
					onMinAmmo?.Invoke();
				if (maxAmmo == _ammoValue)
					onMaxAmmo?.Invoke();

			}
		}

		public int minAmmo => 0;
		#endregion

		private void Awake()
		{
			renderer = GetComponentInChildren<SpriteRenderer>();
		}

		public virtual void Init(CharacterController owner, LayerMask targetMask)
		{
			this.owner = owner;
			this.targetMask = targetMask;
			weaponState = WeaponState.Attackable;
			_ammoValue = maxAmmo;
		}

		public virtual void Attack(Vector2 attackDiraction) 
		{
			onAttack?.Invoke();
		}

		public void UseAmmo(int amount)
		{
			if (isUseAmmo)
			{
				ammoValue -= amount;
				if (ammoValue == minAmmo)
					weaponState = WeaponState.AmmoEmpty;
			}
			onUseAmmo?.Invoke(amount);
		}

		public void AddAmmo(int amount)
		{
			ammoValue += amount;
			onAddAmmo?.Invoke(amount);
		}

		public void ReloadEnter()
		{
			weaponState = WeaponState.Reload;
			onReloadEnter?.Invoke();
		}

		public virtual void ReloadExit()
		{
			weaponState = WeaponState.Attackable;
			ammoValue = maxAmmo;
			onReloadExit?.Invoke();
		}
	}
}

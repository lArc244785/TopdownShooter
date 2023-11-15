using System;
using UnityEngine;
using CharacterController = TopdownShooter.Characters.CharacterController;
using Random = UnityEngine.Random;

namespace TopdownShooter.Weapons
{
	public enum WeaponState
	{
		None,
		Attackable,
		Attack,
		MagazineAmmoEmpty,
		Reloading,
		AmmoEmpty,
	}


	public abstract class WeaponBase : MonoBehaviour, IWeapon, IMagazine
	{

		[SerializeField] private WeaponType _weaponType;

		[SerializeField] private float _minDamage;
		[SerializeField] private float _maxDamage;

		[SerializeField] private float _attackTime;
		[SerializeField] private float _reloadTime;

		[SerializeField] public bool isInfiniteAmmo;

		public bool canReload => (weaponState == WeaponState.Attackable && magazineAmmoValue < maxMagazineAmmo) ||
								  weaponState == WeaponState.MagazineAmmoEmpty;
		public float damage => Random.RandomRange(_minDamage, _maxDamage);

		public float attackTime => _attackTime;

		public float reloadTime => _reloadTime;

		public WeaponType weaponType => _weaponType;
		[field: SerializeField] public WeaponState weaponState { set; get; }

		private int _magazineAmmoValue;
		public int magazineAmmoValue
		{
			get
			{
				return _magazineAmmoValue;
			}
			set
			{
				if (value == magazineAmmoValue)
					return;
				_magazineAmmoValue = Mathf.Clamp(value, minMagazineAmmo, maxMagazineAmmo);

				onChangeMagazineAmmo?.Invoke(value);
				if (_magazineAmmoValue == minMagazineAmmo)
					onMinMagazineAmmo?.Invoke();
			}
		}

		[SerializeField] private int _maxMagazineAmmo;
		public int maxMagazineAmmo => _maxMagazineAmmo;
		public int minMagazineAmmo => 0;

		private int _ammoValue;
		public int ammoValue
		{
			get
			{
				return _ammoValue;
			}
			set
			{
				if (_ammoValue == value)
					return;

				_ammoValue = Mathf.Clamp(value, minAmmo, maxAmmo);
				onChangeAmmo?.Invoke(value);

				if (_ammoValue == minAmmo)
					onMinAmmo?.Invoke();
				else if (_ammoValue == maxAmmo)
					onMaxAmmo?.Invoke();
			}
		}

		[SerializeField] private int _maxAmmo;
		public int maxAmmo => _maxAmmo;

		public int minAmmo => 0;

		public event Action onAttack;
		public event Action onReload;
		public event Action<int> onChangeMagazineAmmo;
		public event Action onMinMagazineAmmo;
		public event Action onMaxAmmo;
		public event Action onMinAmmo;
		public event Action<int> onChangeAmmo;
		public event Action onAddAmmo;

		public CharacterController owner => _owner;
		private CharacterController _owner;

		protected LayerMask targetLayerMask;
		public SpriteRenderer renderer { get; private set; }

		protected virtual void Awake()
		{
			renderer = GetComponentInChildren<SpriteRenderer>();

			_ammoValue = _maxAmmo;
			_magazineAmmoValue = _maxMagazineAmmo;
			weaponState = WeaponState.Attackable;
			//탄창이 비어있는데 탄도 없는 경우
			onMinMagazineAmmo += () =>
			{
				weaponState = WeaponState.MagazineAmmoEmpty;
				if (ammoValue == minAmmo)
					weaponState = WeaponState.AmmoEmpty;
			};
			//탄이 없다가 탄이 채워진 경우
			onAddAmmo += () =>
			{
				if (weaponState == WeaponState.AmmoEmpty)
					weaponState = WeaponState.MagazineAmmoEmpty;
			};
		}

		public virtual void Init(CharacterController owner, LayerMask targetLayerMask)
		{
			_owner = owner;
			this.targetLayerMask = targetLayerMask;
		}


		public void AddAmmo(int amount)
		{
			ammoValue += amount;
			onAddAmmo?.Invoke();
		}

		public void AddMagazineAmmo(int amount)
		{
			magazineAmmoValue += amount;
		}

		public virtual bool Attack(Vector2 attackDiraction)
		{
			return weaponState == WeaponState.Attackable;
		}


		public void Reload()
		{
			weaponState = WeaponState.Attackable;

			//가지고 있는 탄약에서 최재 장탄수를 빼본다.
			bool inputMaxAmmo = ammoValue - maxMagazineAmmo > 0 ? true : false;
			int ammo = maxMagazineAmmo;

			if (!inputMaxAmmo)
			{
				ammo = ammoValue;
			}

			//탄이 남아있는 경우
			if(ammo > 0)
			{
				AddMagazineAmmo(ammo);
				UseAmmo(ammo);
			}
		}

		public void UseAmmo(int amount)
		{
			if (!isInfiniteAmmo)
				ammoValue -= amount;
		}

		public void UseMagazineAmmo(int amount)
		{
			magazineAmmoValue -= amount;
		}
	}
}

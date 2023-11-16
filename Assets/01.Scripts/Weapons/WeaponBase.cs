﻿using System;
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
		[SerializeField] private string _weaponName;	
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
		public event Action onAddMagazine;

		public CharacterController owner => _owner;
		private CharacterController _owner;

		protected LayerMask targetLayerMask;
		public SpriteRenderer renderer { get; private set; }

		public bool isAmmoEmpty { get; private set; }
		public bool isMagazineEmtpy { get; private set; }

		public string weaponName => _weaponName;

		protected virtual void Awake()
		{
			renderer = GetComponentInChildren<SpriteRenderer>();

			_ammoValue = _maxAmmo;
			_magazineAmmoValue = _maxMagazineAmmo;
			weaponState = WeaponState.Attackable;

			//탄 추가
			onAddAmmo += () => isAmmoEmpty = false;
			onAddMagazine += () => isMagazineEmtpy = false;

			//탄 모두 소모 시
			onMinAmmo += () => isAmmoEmpty = true;
			onMinMagazineAmmo += () => isMagazineEmtpy = true;
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
			onAddMagazine?.Invoke();
		}

		public virtual bool Attack(Vector2 attackDiraction)
		{
			if (weaponState == WeaponState.Attackable)
			{
				weaponState = WeaponState.Attack;
				return true;
			}

			return false;
		}


		public void Reload()
		{
			Debug.Log($"{weaponName} Reload");
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

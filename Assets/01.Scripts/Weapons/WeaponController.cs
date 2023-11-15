using System;
using System.Collections.Generic;
using UnityEngine;
using CharacterController = TopdownShooter.Characters.CharacterController;
namespace TopdownShooter.Weapons
{

	public class WeaponController : MonoBehaviour
	{
		[field: SerializeField] public Transform handPivot { private set; get; }
		[SerializeField] private WeaponBase _currentWeapon;
		[SerializeField] private List<WeaponBase> _weaponList;

		public bool CanWeaponSwitch() =>
			_currentWeapon == null ||
			_currentWeapon.weaponState == WeaponState.Attackable ||
			_currentWeapon.weaponState == WeaponState.MagazineAmmoEmpty;

		[SerializeField]
		private LayerMask _hitMask;

		private CharacterController _owner;

		public Vector2 aimDiraction { get; private set; }
		private Timer _attackTimer = new();
		private Timer _reloadTimer = new();

		public event Action<WeaponBase> onChangeWeapon;

		private void Start()
		{
			_owner = GetComponent<CharacterController>();
			foreach (var weapon in _weaponList)
			{
				weapon.Init(_owner, _hitMask);
				weapon.gameObject.SetActive(false);
			}

			ChangeWeapon(0);

		}

		public bool ChangeWeapon(int index)
		{
			if (!CanWeaponSwitch() || (index < 0 || index >= _weaponList.Count))
				return false;

			_currentWeapon?.gameObject.SetActive(false);
			_currentWeapon = _weaponList[index];
			_currentWeapon.gameObject.SetActive(true);

			_attackTimer.currentTime = 0.0f;
			_attackTimer.endTime = _currentWeapon.attackTime;
			_reloadTimer.currentTime = 0.0f;
			_reloadTimer.endTime = _currentWeapon.reloadTime;

			onChangeWeapon?.Invoke(_currentWeapon);
			return true;
		}

		public bool TryGetWeapon(WeaponType weaponType, out WeaponBase weapon)
		{
			weapon = null;
			foreach (var item in _weaponList)
			{
				if (item.weaponType == weaponType)
				{
					weapon = item;
					return true;
				}
			}

			return false;
		}

		private void Update()
		{
			if (_currentWeapon == null)
				return;

			WeaponUpdate();
		}

		private void WeaponUpdate()
		{
			switch (_currentWeapon.weaponState)
			{
				case WeaponState.Attack:
					_attackTimer.currentTime += Time.deltaTime;
					if (_attackTimer.currentTime >= _attackTimer.endTime)
					{
						_attackTimer.currentTime = 0.0f;
						_currentWeapon.weaponState = WeaponState.Attackable;
					}
					break;

				case WeaponState.Reloading:
					_reloadTimer.currentTime += Time.deltaTime;
					if (_reloadTimer.currentTime >= _reloadTimer.endTime)
					{
						_reloadTimer.currentTime = 0.0f;
						_currentWeapon.Reload();
					}
					break;
			}
		}

		public void Attack()
		{
			if (!_currentWeapon.Attack(aimDiraction))
				return;

			if(_currentWeapon.weaponState == WeaponState.Attackable)
				_currentWeapon.weaponState = WeaponState.Attack;
		}

		public void ReLoad()
		{
			if (!_currentWeapon.canReload)
				return;

			_currentWeapon.weaponState = WeaponState.Reloading;
		}

		public void AimUpdate(Vector2 aim)
		{
			aimDiraction = aim;
			var theta = Mathf.Atan2(aimDiraction.y, aimDiraction.x) * Mathf.Rad2Deg;
			handPivot.rotation = Quaternion.Euler(0, 0, theta);
			_currentWeapon.renderer.flipY = (theta >= 90.0f && theta <= 180.0f) || (theta <= -90.0f && theta >= -180.0f) ? true : false;
		}

	}
}

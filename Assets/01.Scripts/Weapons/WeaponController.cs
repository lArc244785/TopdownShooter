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

		public event Action<WeaponBase, WeaponBase> onChangeWeapon;

		public Action _attackEndCallback;
		public Action _reloadEndCallBack;

		public WeaponState currentWeaponState => _currentWeapon.weaponState;

		public Action<float> OnFireing;

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

			WeaponBase oldWeapon = _currentWeapon;

			_currentWeapon?.gameObject.SetActive(false);
			_currentWeapon = _weaponList[index];
			_currentWeapon.gameObject.SetActive(true);

			_attackTimer.currentTime = 0.0f;
			_attackTimer.endTime = _currentWeapon.attackTime;
			_reloadTimer.currentTime = 0.0f;
			_reloadTimer.endTime = _currentWeapon.reloadTime;

			onChangeWeapon?.Invoke(oldWeapon, _currentWeapon);
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
						if(!_currentWeapon.isMagazineEmtpy)
							_currentWeapon.weaponState = WeaponState.Attackable;
						else if(!_currentWeapon.isAmmoEmpty)
							_currentWeapon.weaponState = WeaponState.MagazineAmmoEmpty;
						else
							_currentWeapon.weaponState = WeaponState.AmmoEmpty;

						_attackEndCallback?.Invoke();
						OnFireing?.Invoke(aimDiraction.x);
					}
					break;

				case WeaponState.Reloading:
					_reloadTimer.currentTime += Time.deltaTime;
					if (_reloadTimer.currentTime >= _reloadTimer.endTime)
					{
						_reloadTimer.currentTime = 0.0f;
						_currentWeapon.Reload();
						_reloadEndCallBack?.Invoke();
					}
					break;
			}
		}

		public bool Attack(Action attackEndCallBack = null)
		{
			if (_currentWeapon.Attack(aimDiraction))
			{
				_attackTimer.currentTime = 0.0f;
				_attackEndCallback = attackEndCallBack;
				return true;
			}

			return false;
		}

		public bool ReLoad(Action reloadEndCallback = null)
		{
			if (!_currentWeapon.canReload)
				return false;

			_currentWeapon.ReloadStart();
			_currentWeapon.weaponState = WeaponState.Reloading;
			_reloadEndCallBack = reloadEndCallback;
			return true;
		}

		public void AimUpdate(Vector2 aim)
		{
			aimDiraction = aim;
			var theta = Mathf.Atan2(aimDiraction.y, aimDiraction.x) * Mathf.Rad2Deg;
			handPivot.rotation = Quaternion.Euler(0, 0, theta);
			bool flipY = (theta >= 90.0f && theta <= 180.0f) || (theta <= -90.0f && theta >= -180.0f) ? true : false;
			_currentWeapon.SetFlipY(flipY);
		}

		public bool CanAttack()
		{
			return _currentWeapon != null && _currentWeapon.weaponState == WeaponState.Attackable;
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.cyan;

			var diraction = aimDiraction.sqrMagnitude > 0.0f ? aimDiraction : Vector2.right;

			var startPos = handPivot.position;
			var endPos = startPos + (Vector3)(Vector2.one * diraction * 1.0f);

			Gizmos.DrawLine(startPos, endPos);
		}
	}
}

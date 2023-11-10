using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using CharacterController = TopdownShooter.Characters.CharacterController;
namespace TopdownShooter.Weapons
{

	public class WeaponController : MonoBehaviour
	{
		[field:SerializeField] public Transform handPivot { private set; get; }
		[SerializeField] private WeaponBase _currentWeapon;
		[SerializeField] private List<WeaponBase> _weaponList;

		private bool _isSwapable => _currentWeapon.weaponState != WeaponState.Reload || 
			(_currentWeapon.weaponState == WeaponState.Attackable && _attackWaitTime < 0.0f);

		[SerializeField]
		private LayerMask _targetMask;

		private CharacterController _owner;

		private float _attackWaitTime;
		private float _reloadWaitTime;

		private void Start()
		{
			_owner = GetComponent<CharacterController>();

			foreach (var weapon in _weaponList)
			{
				weapon.Init(_owner, _targetMask);
				weapon.gameObject.SetActive(false);
			}
			_currentWeapon = _weaponList[0];
			_currentWeapon.gameObject.SetActive(true);
		}

		public bool ChangeWeapon(int index)
		{
			if (!_isSwapable || (index < 0 || index >= _weaponList.Count))
				return false;

			_currentWeapon.gameObject.SetActive(false);
			_currentWeapon = _weaponList[index];
			_currentWeapon.gameObject.SetActive(true);
			return true;
		}


		public void Attack()
		{
			if (_attackWaitTime > 0.0f)
				return;

			_currentWeapon.Attack();
			_attackWaitTime = _currentWeapon.attackCoolTime;
		}

		public void ReLoad()
		{
			_currentWeapon.ReloadEnter();
			_reloadWaitTime = _currentWeapon.reloadCoolTime;
		}

		private void Update()
		{
			HandRotaion();

			//Attack
			if (_currentWeapon.weaponState == WeaponState.Attackable)
			{
				_attackWaitTime -= Time.deltaTime;
			}

			//Reload
			if (_currentWeapon.weaponState == WeaponState.Reload)
			{
				_reloadWaitTime -= Time.deltaTime;
				if (_reloadWaitTime < 0.0f)
					_currentWeapon.ReloadExit();
			}
		}

		private void HandRotaion()
		{
			float radTheta = Mathf.Atan2(_owner.lookDirection.y, _owner.lookDirection.x);
			handPivot.rotation = Quaternion.Euler(0, 0, radTheta * Mathf.Rad2Deg);
		}

	}
}

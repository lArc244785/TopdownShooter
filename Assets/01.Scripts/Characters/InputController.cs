using UnityEngine;
using TopdownShooter.Weapons;

namespace TopdownShooter.Characters
{
	public class InputController : MonoBehaviour
	{
		[SerializeField] private CharacterController _characterController;
		[SerializeField] private WeaponController _weaponController;


		private void Update()
		{
			Move();
			MouseLook();
			Attack();
			Reload();
			WeaponChange();
		}

		private void Move()
		{
			_characterController.horizontal = Input.GetAxis("Horizontal");
			_characterController.vertical = Input.GetAxis("Vertical");
		}

		private void MouseLook()
		{
			Vector2 characterToMousePosDiraction = Input.mousePosition - Camera.main.WorldToScreenPoint(_weaponController.handPivot.position);
			characterToMousePosDiraction.Normalize();

			_weaponController.AimUpdate(characterToMousePosDiraction);
		}

		private void Attack()
		{
			if (Input.GetMouseButton(0))
				_weaponController?.Attack();
		}

		private void Reload()
		{
			if (Input.GetKeyDown(KeyCode.R))
				_weaponController?.ReLoad();
		}

		private void WeaponChange()
		{
			if (Input.GetKeyDown(KeyCode.Alpha1))
				_weaponController?.ChangeWeapon(0);
			if (Input.GetKeyDown(KeyCode.Alpha2))
				_weaponController?.ChangeWeapon(1);
		}
	}

}


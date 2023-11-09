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
		}

		private void Move()
		{
			_characterController.horizontal = Input.GetAxis("Horizontal");
			_characterController.vertical = Input.GetAxis("Vertical");
		}

		private void MouseLook()
		{
			Vector2 characterToMousePos = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
			characterToMousePos.Normalize();

			_characterController.lookDirection = characterToMousePos;
		}

		private void Attack()
		{
			if (Input.GetMouseButtonDown(0))
				_weaponController?.Attack();
		}

		private void Reload()
		{
			//if (Input.GetKeyDown(KeyCode.R))
			//	//_weaponController?.Reload();
		}
	}

}


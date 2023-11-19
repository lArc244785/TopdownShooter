using UnityEngine;
using TopdownShooter.Weapons;
using TopdownShooter.Interactions;
using TopdownShooter.UIs;

namespace TopdownShooter.Characters
{
	public class InputController : MonoBehaviour
	{
		[SerializeField] private CharacterController _characterController;
		[SerializeField] private WeaponController _weaponController;
		[HideInInspector] public bool isInputable;
		[SerializeField] private GameOptionPopUp _gameOptionPopUp;

		private IIteraction _interaction; 

		private void Awake()
		{
			isInputable = true;
		}

		private void Update()
		{
			if (!isInputable)
				return;

			if (Input.GetKeyDown(KeyCode.Escape))
				_gameOptionPopUp.OptionPopUp();

			if (_gameOptionPopUp.IsVisable)
				return;

			Interaction();
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

		public void SetInteraction(IIteraction interaction)
		{
			_interaction = interaction;
		}

		private void Interaction()
		{
			if (_interaction == null)
				return;

			if(Input.GetKeyDown(KeyCode.F))
			{
				_interaction.Interaction();
				_interaction = null;
			}
				
		}
	}

}


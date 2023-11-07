using UnityEngine;

namespace TopdownShooter.Controllers
{
	public class InputController : MonoBehaviour
	{
		[SerializeField] private CharacterController _characterController;


		private void Update()
		{
			Move();
			MouseLook();
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
	}

}


using UnityEngine;
using CharacterController = TopdownShooter.Characters.CharacterController;
namespace TopdownShooter.Weapons
{
	
	public class WeaponController : MonoBehaviour
	{
		[SerializeField]
		private WeaponBase _weapon;

		[SerializeField]
		private LayerMask _targetMask;

		private CharacterController _owner;

		private void Start()
		{
			_owner = GetComponent<CharacterController>();
			_weapon.Init(_owner, _targetMask);
		}


		public void Attack()
		{
			_weapon?.Attack();
		}

		public void ReLoadEnter()
		{
			_weapon?.ReloadEnter();
		}

		public void ReLoadExit()
		{
			_weapon?.ReloadExit();
		}

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TopdownShooter.Weapons
{
	public class WeaponAnimation : MonoBehaviour
	{
		[SerializeField] private Animator _animator;
		private WeaponBase _weapon;

		private void Start()
		{
			_weapon = GetComponent<WeaponBase>();
			_weapon.onAttack += () =>
			{
				_animator?.SetTrigger("Attack");
			};
			_weapon.onReloadStart += () => _animator?.SetTrigger("Reload");
		}
	}
}

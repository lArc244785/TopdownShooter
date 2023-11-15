using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopdownShooter.Characters;
using TopdownShooter.Weapons;
using UnityEngine;

namespace TopdownShooter.Items
{
	public class ItemAddAmmo : ItemBase
	{
		public override ItemType Type => ItemType.RecoverHp;
		[SerializeField] private int _addAmmo;
		[SerializeField] private WeaponType _weaponType;

		public override bool Effect(Collider2D collider)
		{
			if(collider.TryGetComponent<WeaponController>(out var controller))
			{
				if(controller.TryGetWeapon(_weaponType, out var weapon))
				{
					weapon.AddAmmo(_addAmmo);
					return base.Effect(collider);
				}
			}

			return false;
		}
	}
}

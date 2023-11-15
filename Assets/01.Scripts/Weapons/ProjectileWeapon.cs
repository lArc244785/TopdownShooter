using System;
using TopdownShooter.Characters;
using UnityEngine;

namespace TopdownShooter.Weapons
{
	public class ProjectileWeapon : WeaponBase 
	{

		[SerializeField] Projectile _projectile;
		[SerializeField] private Transform _firePoint;

		public override bool Attack(Vector2 attackDiraction)
		{
			if (!base.Attack(attackDiraction))
				return false;

			var bullet = Instantiate(_projectile);
			bullet.transform.position = _firePoint.position;
			bullet.Init(owner, attackDiraction, damage, targetLayerMask);

			UseMagazineAmmo(1);

			return true;
		}
	}
}

using System;
using TopdownShooter.Characters;
using UnityEngine;

namespace TopdownShooter.Weapons
{
	public class ProjectileWeapon : WeaponBase
	{

		[SerializeField] Projectile _projectile;
		[SerializeField] private Transform _firePoint;

		public override void Attack()
		{
			base.Attack();

			var bullet = Instantiate(_projectile);
			bullet.transform.position = _firePoint.position;
			bullet.Init(owner, damage, targetMask);

			UseAmmo(1);
		}

	}
}

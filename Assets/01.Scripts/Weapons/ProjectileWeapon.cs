using System;
using TopdownShooter.Characters;
using UnityEngine;

namespace TopdownShooter.Weapons
{
	public class ProjectileWeapon : WeaponBase
	{

		private float _projectionInstanceDistance = 0.5f;

		[SerializeField] Projectile _projectile;

		public override void Attack()
		{
			base.Attack();

			var bullet = Instantiate(_projectile);
			bullet.transform.position = owner.transform.position + (Vector3)(owner.lookDirection * _projectionInstanceDistance);
			bullet.Init(owner, damage, targetMask);

			UseAmmo(1);
		}

	}
}

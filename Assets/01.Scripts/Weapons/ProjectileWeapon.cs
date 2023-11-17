using UnityEngine;

namespace TopdownShooter.Weapons
{
	public class ProjectileWeapon : WeaponBase 
	{

		[SerializeField] Projectile _projectile;
		[SerializeField] private Transform _firePoint;
		[SerializeField] private Vector2 _flipTrueFireLocalPosition;
		[SerializeField] private Vector2 _flipFalsFireLocalPosition;
		private SpriteRenderer _muzzleFlashSpriteRenderer;


		protected override void Awake()
		{
			base.Awake();
			_muzzleFlashSpriteRenderer = _firePoint.GetComponent<SpriteRenderer>();
		}

		public override bool Attack(Vector2 attackDiraction)
		{
			if (!base.Attack(attackDiraction))
				return false;

			var bullet = Instantiate(_projectile);
			bullet.transform.position = _firePoint.position;
			bullet.Init(owner, attackDiraction, damage, targetLayerMask);

			UseMagazineAmmo(1);

			CameraEffect.instance.CameraShake(3.0f, 1.0f, 0.1f);

			return true;
		}

		public override void SetFlipY(bool y)
		{
			renderer.flipY = y;
			_muzzleFlashSpriteRenderer.flipY = y;
			//_firePoint.localPosition = y ? _flipTrueFireLocalPosition : _flipFalsFireLocalPosition;
		}
	}
}

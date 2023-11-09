using TopdownShooter.Characters;
using UnityEngine;

namespace TopdownShooter.Weapons
{
	public class MeleeWeapon : WeaponBase
	{
		[SerializeField] private Vector2 _attackSize;
		public Vector2 AttackSize => _attackSize;

		[SerializeField] private float _attackDistance;
		public float attackDistance => _attackDistance;


		public override void Attack()
		{
			base.Attack();
			var hits = Physics2D.BoxCastAll(owner.transform.position, _attackSize, 0.0f, owner.lookDirection, _attackDistance, targetMask);

			DebugDrawHitBox(Color.red, 0.5f);

			if (hits == null)
				return;

			foreach(var hit in hits)
			{
				if(hit.collider.TryGetComponent<IHP>(out var hp))
				{
					hp.DeleteHp(owner.transform,damage);
				}
			}
		}

		private void DebugDrawHitBox(Color color, float duration)
		{
			Vector2 topLeft = (Vector2)owner.transform.position + _attackSize * Vector2.up * 0.5f;
			Vector2 topRight = topLeft + Vector2.right * _attackDistance + _attackSize.x * Vector2.right;
			Vector2 bottomLeft = (Vector2)owner.transform.position + _attackSize  * Vector2.down * 0.5f;
			Vector2 bottomRight = bottomLeft + Vector2.right * _attackDistance + _attackSize.x * Vector2.right;

			Debug.DrawLine(topLeft, topRight, color, duration);
			Debug.DrawLine(topRight, bottomRight, color, duration);
			Debug.DrawLine(bottomRight, bottomLeft, color, duration);
			Debug.DrawLine(bottomLeft, topLeft, color, duration);
		}

	}
}

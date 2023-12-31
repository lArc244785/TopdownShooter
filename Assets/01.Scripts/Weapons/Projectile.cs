﻿using TopdownShooter.Characters;
using UnityEngine;
using CharacterController = TopdownShooter.Characters.CharacterController;
namespace TopdownShooter.Weapons
{
	public class Projectile : MonoBehaviour
	{
		[SerializeField] private float _speed;
		[SerializeField] private float _lifeTime;

		private float _timer;

		private Vector2 _diraction;
		private LayerMask _targetMask;
		private float _damage;
		private CharacterController _owner;

		public void Init(CharacterController owner, Vector2 attackDiraction,float dmage, LayerMask targetMask)
		{
			_owner = owner;
			_damage = dmage;
			_targetMask = targetMask;
			_diraction = attackDiraction;

			_timer = 0.0f;
		}

		private void Update()
		{
			_timer += Time.deltaTime;
			if (_timer >= _lifeTime)
				DestroyProjectile();
		}

		private void FixedUpdate()
		{
			Vector2 nextMovePoint = (Vector2)transform.position + (_diraction * _speed * Time.fixedDeltaTime);
			var hit = Physics2D.OverlapPoint(nextMovePoint, _targetMask);

			if (hit != null)
			{
				if(hit.TryGetComponent<IHP>(out var hp))
				{
					Debug.Log($"Hit Owner{_owner.gameObject.name}");
					hp.DeleteHp(_owner, _damage);
				}
				DestroyProjectile();
			}
			else
			{
				transform.position = nextMovePoint;
			}
		}

		private void DestroyProjectile()
		{
			Destroy(gameObject);
		}
	}
}

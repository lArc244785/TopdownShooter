using TopdownShooter.FSM;
using TopdownShooter.Pathfinders;
using TopdownShooter.Weapons;
using UnityEngine;

namespace TopdownShooter.Characters
{
	public enum AIState
	{
		None,
		RandomMove,
		Attack,
		Follow,
		Pursue,
	}


	public class EnemyController : CharacterController
	{
		[SerializeField] private AIState _aiState;
		//RandomMove
		[SerializeField] private Timer _randomMoveTimer;

		//Purse
		[SerializeField] private Timer _pursueTimer;
		[SerializeField] private float _targetPursueDistance;

		//Follow
		[SerializeField] private float _followRange;

		//Attack
		[SerializeField] private WeaponController _weaponController;
		[SerializeField] private Timer _attackWaitTimer;
		[SerializeField] private float _attackRange;
		private Transform _attackTarget;

		[SerializeField] private Transform _target;
		[SerializeField] private LayerMask _targetLayer;
		[SerializeField] private LayerMask _cantMoveLayer;

		private PathFinder _pathFinder;


		private Vector2[] _paths;
		private int _index;
		private Vector2 _currentTargetPos;
		private Vector2 _diraction;
		private Vector2 _nextMovePosition;

		private LayerMask _rayMask;

		protected override void Awake()
		{
			base.Awake();
			_rayMask = _targetLayer | _cantMoveLayer;
		}

		protected override void Start()
		{
			base.Start();
			_pathFinder = new PathfFinderAStar();
			machine = new EnemyMachine(this);
			_aiState = AIState.RandomMove;
		}

		protected override void Update()
		{
			base.Update();

			AIUpdate();

		}

		private void AIUpdate()
		{
			if (machine.currentID == CharacterStateID.Hurt ||
				machine.currentID == CharacterStateID.Die)
				return;

			Attack();
			Pursue();
			Follow();
			RandomMove();
		}

		private void RandomMove()
		{
			if (_aiState != AIState.RandomMove)
			{
				_randomMoveTimer.currentTime = 0.0f;
				return;
			}

			_randomMoveTimer.currentTime += Time.deltaTime;
			if (_randomMoveTimer.currentTime >= _randomMoveTimer.endTime)
			{
				horizontal = Random.RandomRange(-1.0f, 1.0f);
				vertical = Random.RandomRange(-1.0f, 1.0f);

				var moveDir = new Vector2(horizontal, vertical).normalized;
				horizontal = moveDir.x;
				vertical = moveDir.y;

				_weaponController.AimUpdate(moveDir);
				_randomMoveTimer.currentTime = 0.0f;
			}
		}

		void Pursue()
		{
			if (_aiState == AIState.Attack ||
				_aiState == AIState.Follow)
			{
				_paths = null;
				return;
			}

			var target = Physics2D.OverlapCircle(transform.position, _targetPursueDistance, _targetLayer);

			if (_aiState != AIState.Pursue && target != null)
			{
				_pursueTimer.currentTime = 0.0f;
				_aiState = AIState.Pursue;
			}
			else if (target != null)
			{
				_pursueTimer.currentTime += Time.deltaTime;
				if (_pursueTimer.currentTime >= _pursueTimer.endTime)
				{
					_pursueTimer.currentTime = 0.0f;

					if (_pathFinder.TryGetPath(transform.position, target.transform.position, out _paths))
					{
						for (int i = 1; i < _paths.Length; i++)
						{
							Debug.DrawLine(_paths[i - 1], _paths[i], Color.white, 1.0f);
						}

						_index = 1;
						PathDiractionUpdate();
					}
				}
			}
		}


		private void Follow()
		{
			if (_aiState == AIState.Attack)
				return;
			// Follow 범위에 Target 있는가?
			var target = Physics2D.OverlapCircle(transform.position, _followRange, _targetLayer);
			if (target == null)
				return;

			// 직선 방향으로 Target 에게 이동이 가능한가?
			var rayDir = (target.transform.position - transform.position).normalized;
			var hitTarget = Physics2D.Raycast(transform.position, rayDir, _followRange, _rayMask);

			//직선 상에 없는 경우
			if (hitTarget.collider == null || (_cantMoveLayer & 1 << hitTarget.transform.gameObject.layer) > 0)
			{
				if (_aiState == AIState.Follow)
				{
					_aiState = AIState.RandomMove;
					horizontal = 0.0f;
					vertical = 0.0f;
				}
				return;
			}

			//직선 상에 있는 경우
			if (_aiState != AIState.Follow)
			{
				_aiState = AIState.Follow;
				_target = hitTarget.transform;
			}


			Vector2 followDiraction = ((Vector2)_target.transform.position - (Vector2)transform.position).normalized;
			horizontal = followDiraction.x;
			vertical = followDiraction.y;

		}

		private void Attack()
		{
			if (_aiState == AIState.Attack)
			{
				_attackWaitTimer.currentTime += Time.deltaTime;
				var aimDir = (_attackTarget.position - transform.position).normalized;
				_weaponController.AimUpdate(aimDir);

				if (_attackWaitTimer.currentTime >= _attackWaitTimer.endTime)
				{
					_weaponController.Attack();
					_attackWaitTimer.currentTime = 0.0f;
					_aiState = AIState.RandomMove;
					horizontal = 0.0f;
					vertical = 0.0f;
					isMoveable = true;
				}
			}
			else
			{
				var col = Physics2D.OverlapCircle(transform.position, _attackRange, _targetLayer);
				if (col == null)
					return;

				Vector2 rayDiraction = (col.transform.position - transform.position).normalized;
				var hit = Physics2D.Raycast(transform.position, rayDiraction, _attackRange, _rayMask);

				if (hit.collider == null || (_cantMoveLayer & 1 << hit.transform.gameObject.layer) > 0)
					return;

				isMoveable = false;
				_attackTarget = hit.transform;
				_aiState = AIState.Attack;
				_attackWaitTimer.currentTime = 0.0f;
			}
		}


		private void PathDiractionUpdate()
		{
			if (_paths == null || _index >= _paths.Length)
				throw new System.Exception($"경로가 없거나 최종 목적에 도착하고 다음 방향을 찾고자 시도하였습니다.");

			_currentTargetPos = _paths[_index];
			_diraction = (_currentTargetPos - (Vector2)transform.position).normalized;
			horizontal = _diraction.x;
			vertical = _diraction.y;
		}

		protected override void Move()
		{
			if (!isMoveable)
				return;


			if (_aiState == AIState.Pursue && _paths != null)
				PurseMove();
			else
			{
				var dir = new Vector2(horizontal, vertical);

				if (Physics2D.Raycast(transform.position, dir, 0.5f, _cantMoveLayer))
				{
					vertical = 0.0f;
					horizontal = 0.0f;
				}
				base.Move();
			}
			var aimDir = new Vector2(horizontal, vertical).normalized;
			if (aimDir.sqrMagnitude > 0)
				_weaponController.AimUpdate(aimDir);
		}

		private void PurseMove()
		{
			_nextMovePosition = (Vector2)transform.position + new Vector2(horizontal, vertical) * speed * Time.fixedDeltaTime;
			if (Vector2.Distance(transform.position, _currentTargetPos) > Vector2.Distance(_nextMovePosition, _currentTargetPos))
				rig2D.position = _nextMovePosition;
			else
			{
				rig2D.position = _currentTargetPos;
				_index++;
				if (_index == _paths.Length)
				{
					_paths = null;
					horizontal = 0.0f;
					vertical = 0.0f;
					_aiState = AIState.RandomMove;
				}
				else
					PathDiractionUpdate();
			}
		}
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(transform.position, _targetPursueDistance);

			Gizmos.color = Color.cyan;
			Gizmos.DrawWireSphere(transform.position, _followRange);

			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, _attackRange);
		}

	}
}

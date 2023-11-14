using TopdownShooter.FSM;
using TopdownShooter.Pathfinders;
using UnityEngine;

namespace TopdownShooter.Characters
{
	public class EnemyController : CharacterController
	{
		private float _purseTime;
		[SerializeField]private float _purseTimer = 5.0f;
		[SerializeField] private Transform _target;
		[SerializeField] private LayerMask _targetLayer;
		[SerializeField] private LayerMask _cantMoveLayer;
		[SerializeField] protected float _aiUpdateTime;
		[SerializeField] private float _targetDetetedDistacne;
		[SerializeField] private float _targetPursueDistance;

		private PathFinder _pathFinder;
		
		private float _aiTime;

		private Vector2[] _paths;
		private int _index;
		private Vector2 _currentTargetPos;
		private Vector2 _diraction;

		private Vector2 _nextMovePosition;

		private Transform _oldTarget;

		private bool isFollow;

		protected override void Awake()
		{
			base.Awake();
		}

		private void Start()
		{
			_pathFinder = new PathfFinderAStar();
			machine = new EnemyMachine(this);
		}


		protected override void Update()
		{
			base.Update();

			if (_target != null)
				return;

			_aiTime += Time.deltaTime;
			if(_aiTime >= _aiUpdateTime)
			{
				horizontal = Random.RandomRange(-1.0f, 1.0f);
				vertical = Random.RandomRange(-1.0f, 1.0f);

				var moveDir = new Vector2(horizontal, vertical).normalized;
				horizontal = moveDir.x;
				vertical = moveDir.y;

				_aiTime = 0.0f;
			}

		}

		protected override void FixedUpdate()
		{
			Pursue();
			Follow();

			base.FixedUpdate();
		}


		void Pursue()
		{
			if (isFollow)
			{
				_purseTime = 0.0f;
				return;
			}

			var target = Physics2D.OverlapCircle(transform.position, _targetPursueDistance, _targetLayer);
			if(target != null)
			{
				_purseTime += Time.deltaTime;
				if (_purseTime < _purseTimer)
					return;

				_purseTime = 0.0f;
				if (_pathFinder.TryGetPath(transform.position, _target.position, out _paths))
				{
					_index = 1;
					PathDiractionUpdate();
				}
				_target = target.transform;
			}
			else
			{
				_purseTime = 0.0f;
			}
		}

		private void Follow()
		{
			var target = Physics2D.OverlapCircle(transform.position, _targetDetetedDistacne, _targetLayer);
			if (target != null)
			{
				isFollow = true; 
				_paths = null;
				_target = target.transform;
				Vector2 followDiraction = ((Vector2)target.transform.position - (Vector2)transform.position).normalized;
				horizontal = followDiraction.x;
				vertical = followDiraction.y;
				_oldTarget = target.transform;
			}
			else if (isFollow && target == null)
			{
				isFollow = false;
				horizontal = 0.0f;
				vertical = 0.0f;
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

			if (_paths == null)
			{
				var dir = new Vector2(horizontal, vertical);
				if (Physics2D.Raycast(transform.position, dir, 0.5f, _cantMoveLayer))
				{
					vertical = 0.0f;
					horizontal = 0.0f;
				}

				base.Move();
				return;
			}

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
				}
				else
					PathDiractionUpdate();
			}
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(transform.position, _targetPursueDistance);

			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, _targetDetetedDistacne);
		}

	}
}

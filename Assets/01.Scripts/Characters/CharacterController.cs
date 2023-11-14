using System;
using TopdownShooter.FSM;
using Unity.VisualScripting;
using UnityEngine;

namespace TopdownShooter.Characters
{
	public class CharacterController : MonoBehaviour , IHP
	{
		[field: SerializeField]
		public float horizontal
		{
			get
			{
				return _horizontal;
			}

			set
			{
				if (Mathf.Abs(value) > 0.0f)
					LookUpdate(value);

				_horizontal = value;
			}
		}


		private float _horizontal;

		[field: SerializeField]
		public float vertical { set; get; }

		[SerializeField] protected float speed;

		#region Flags
		public bool isMoveable;
		public bool isLookable;
		#endregion

		[SerializeField] private float _lookDirection;
		public float lookDirection
		{
			set { _lookDirection = value; }
			get { return _lookDirection; }
		}

		public Vector2 move { get; protected set; }
		public Rigidbody2D rig2D { private set; get; }

		public event Action<float> onHpChanged;
		public event Action<float> onHpRecover;
		public event Action<float> onHpDelete;
		public event Action onHpMax;
		public event Action onHpMin;
		public event Action onDead;

		public bool invincible { get; set; }

		public float hpValue
		{
			get
			{
				return _hpValue;
			}
			set
			{
				if (value != hpValue)
					onHpChanged?.Invoke(value);

				_hpValue = Mathf.Clamp(value,minHp, maxHp);
				
				if (_hpValue == maxHp)
					onHpMax?.Invoke();

				if(_hpValue == minHp)
					onHpMin?.Invoke();
			}
		}

		public float maxHp => _maxHp;
		[SerializeField] private float _maxHp;

		public float minHp => 0.0f;

		private float _hpValue;

		public Animator animator { get; private set; }

		protected CharacterMachine machine;
		private SpriteRenderer _renderer;

		protected virtual void Awake()
		{
			rig2D = GetComponent<Rigidbody2D>();
			animator = GetComponentInChildren<Animator>();
			_renderer = animator.GetComponent<SpriteRenderer>();
			_hpValue = _maxHp;
		}

		protected virtual void Start()
		{
			onHpMin += () => machine.ChangeState(CharacterStateID.Die);
			onHpDelete += (value) => machine.ChangeState(CharacterStateID.Hurt);
			onDead += () => this.enabled = false;
		}

		protected virtual void Update()
		{
			machine.OnStateUpdate();
		}


		protected virtual void FixedUpdate()
		{
			Move();
		}

		protected virtual void Move()
		{
			if (!isMoveable)
				return;

			move = new Vector2(horizontal, vertical) * speed * Time.fixedDeltaTime;
			rig2D.transform.position += (Vector3)move;
		}

		private void LookUpdate(float look)
		{
			if(!isLookable)
				return;

			_lookDirection = look;

			if (_lookDirection > 0.0f)
				_renderer.flipX = false;
			else if (_lookDirection < 0.0f)
				_renderer.flipX = true;
		}

		public virtual void RecoverHp(object subject, float amount)
		{
			hpValue += amount;
			onHpRecover?.Invoke(amount);
		}

		public virtual void DeleteHp(object subject, float amount)
		{
			hpValue -= amount;
			onHpDelete?.Invoke(amount);
		}

		public void OnDead()
		{
			onDead?.Invoke();
		}
	}

}

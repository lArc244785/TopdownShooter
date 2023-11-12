using System;
using TopdownShooter.FSM;
using Unity.VisualScripting;
using UnityEngine;

namespace TopdownShooter.Characters
{
	public class CharacterController : MonoBehaviour , IHP
	{
		[field:SerializeField]
		public float horizontal { set; get; }
		[field: SerializeField]
		public float vertical { set; get; }

		[SerializeField] protected float speed;

		#region Flags
		public bool isMoveable;
		public bool isLookable;
		#endregion

		[SerializeField] private Vector2 _lookDirection;
		public Vector2 lookDirection
		{
			set { _lookDirection = value; }
			get { return _lookDirection; }
		}

		public Vector2 move { get; protected set; }
		public Rigidbody2D rigidbody2D { private set; get; }

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
			rigidbody2D = GetComponent<Rigidbody2D>();
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
			Look();
		}

		protected virtual void Move()
		{
			if (!isMoveable)
				return;

			move = new Vector2(horizontal, vertical) * speed * Time.fixedDeltaTime;
			rigidbody2D.transform.position += (Vector3)move;
		}

		private void Look()
		{
			if(!isLookable)
				return;

			if (horizontal > 0.0f)
				_renderer.flipX = false;
			else if (horizontal < 0.0f)
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

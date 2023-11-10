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
		public float vertical { set; get; }

		[SerializeField] private float speed;

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

		public Vector2 move { get; private set; }
		public Rigidbody2D rigidbody2D { private set; get; }

		public event Action<float> onHpChanged;
		public event Action<float> onHpRecover;
		public event Action<float> onHpDelete;
		public event Action onHpMax;
		public event Action onHpMin;

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

		protected virtual void Awake()
		{
			rigidbody2D = GetComponent<Rigidbody2D>();
			animator = GetComponentInChildren<Animator>();
			_hpValue = _maxHp;
		}

		protected virtual void Update()
		{
			machine.OnStateUpdate();
		}


		protected virtual void FixedUpdate()
		{
			Move();
			HandRotaion();
		}

		private void Move()
		{
			if (!isMoveable)
				return;

			move = new Vector2(horizontal, vertical) * speed * Time.fixedDeltaTime;
			rigidbody2D.transform.position += (Vector3)move;
		}

		private void HandRotaion()
		{
			if(!isLookable)
				return;

			
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
	}

}

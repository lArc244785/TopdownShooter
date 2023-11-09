using System;
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

		private Vector2 _move;
		public Rigidbody2D rigidbody2D { private set; get; }

		public event Action<float> onHpChanged;
		public event Action<float> onHpRecover;
		public event Action<float> onHpDelete;
		public event Action onHpMax;
		public event Action onHpMin;

		public bool invincible { get => _invincible; set => _invincible = value; }
		private bool _invincible;

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


		protected virtual void Awake()
		{
			rigidbody2D = GetComponent<Rigidbody2D>();
			_hpValue = _maxHp;
		}

		protected virtual void FixedUpdate()
		{
			Move();
			Roation();
		}

		private void Move()
		{
			if (!isMoveable)
				return;

			_move = new Vector2(horizontal, vertical) * speed * Time.fixedDeltaTime;
			rigidbody2D.transform.position += (Vector3)_move;
		}

		private void Roation()
		{
			if(!isLookable)
				return;
			float radTheta = Mathf.Atan2(_lookDirection.y, _lookDirection.x);
			transform.rotation = Quaternion.Euler(0, 0, radTheta * Mathf.Rad2Deg);
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

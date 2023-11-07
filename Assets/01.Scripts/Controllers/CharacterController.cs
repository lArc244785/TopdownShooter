using UnityEngine;

namespace TopdownShooter.Controllers
{
	public class CharacterController : MonoBehaviour
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

		protected virtual void Awake()
		{
			rigidbody2D = GetComponent<Rigidbody2D>();
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

	}

}

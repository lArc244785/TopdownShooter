using UnityEngine;

namespace TopdownShooter.Pathfinders
{
	public class Map : MonoBehaviour
	{
		#region Property
		/// <summary>
		/// Map �̱��� �ν��Ͻ�
		/// </summary>
		public static Map Instance
		{
			get
			{
				if (_instance == null)
					throw new System.Exception("Map is not Instance");
				return _instance;
			}
			set
			{
				if (_instance != null)
					throw new System.Exception($"Map is exist {_instance.gameObject.name}");
				_instance = value;
			}
		}

		/// <summary>
		/// Map�� World���������� CellSize
		/// </summary>
		public Vector2 CellSize => _grid.cellSize;

		/// <summary>
		/// World���� �� ��ġ Offset
		/// </summary>
		[field: SerializeField] public Vector2 Offset { get; private set; }

		/// <summary>
		/// Map�� ũ��
		/// </summary>
		[field: SerializeField] public Vector2 Size { get; private set; }

		/// <summary>
		/// Map�� Y���� ����
		/// </summary>
		public int TotalY { get; private set; }

		/// <summary>
		/// Map�� X���� ����
		/// </summary>
		public int TotalX { get; private set; }

		/// <summary>
		/// Map�� Node�� Node.Index�� ���ؼ� �����մϴ�.
		/// </summary>
		/// <param name="index"></param>
		public Node this[Index index] => _map[index.y, index.x];

		/// <summary>
		/// Map�� Node�� [y,x]�� ���ؼ� �����մϴ�.
		/// </summary>
		public Node this[int y, int x] => _map[y, x];
		#endregion


		#region Field
		/// <summary>
		/// �� �ν��Ͻ�
		/// </summary>
		private static Map _instance;

		/// <summary>
		/// Map ������
		/// </summary>
		private Node[,] _map;

		/// <summary>
		/// Ÿ�ϸ� Cell ������ ������ �ֽ��ϴ�.
		/// </summary>
		private Grid _grid;

		/// <summary>
		/// �� �����Ϳ� ������ �� ���̾���� �����մϴ�.
		/// Ground  : �̵� ���� ���
		/// Wall	: �̵� �Ұ��� ���
		/// </summary>
		[SerializeField] private LayerMask _mapLayer;

		/// <summary>
		/// �ʿ� ���� �Ʒ������� �� ��ġ
		/// </summary>
		private Vector2 _bottomLeft;

		/// <summary>
		/// �ʿ� ������ �������� �� ��ġ
		/// </summary>
		private Vector2 _topRight;

		/// <summary>
		/// �� ���� �Ʒ��� ����� ��ġ
		/// </summary>
		private Vector2 _bottomLeftNodePoint;

		/// <summary>
		/// �� ������ ���� ����� ��ġ
		/// </summary>
		private Vector2 _topRightNodePoint;
		#endregion


		#region Function
		private void Awake()
		{
			Init();
		}

		/// <summary>
		/// �̱��濡 �ν��Ͻ��� ����ϰ� ���� ������ ȣ���մϴ�.
		/// </summary>
		public void Init()
		{
			_grid = GetComponent<Grid>();
			SetUp();
			Instance = this;
		}

		/// <summary>
		/// ������ ������ ���߾ �� �����͸� �����մϴ�.
		/// </summary>
		private void SetUp()
		{
			Vector2 center = (Vector2)transform.position + Offset;
			_bottomLeft = center - (Size * 0.5f);
			_bottomLeftNodePoint = _bottomLeft + ((Vector2)_grid.cellSize * 0.5f);

			_topRight = center + (Size * 0.5f);
			_topRightNodePoint = _topRight - ((Vector2)_grid.cellSize * 0.5f);

			float cellSizeX = _grid.cellSize.x;
			float cellSizeY = _grid.cellSize.y;

			TotalY = (int)((_topRightNodePoint.y - _bottomLeftNodePoint.y) / _grid.cellSize.y) + 1;
			TotalX = (int)((_topRightNodePoint.x - _bottomLeftNodePoint.x) / _grid.cellSize.x) + 1;

			_map = new Node[TotalY, TotalX];

			Vector2 point = _bottomLeftNodePoint;
			bool isVisitable;

			int groundLayer = LayerMask.NameToLayer("Ground");

			for (int i = 0; i < TotalY; i++)
			{
				point = _bottomLeftNodePoint + Vector2.up * cellSizeY * i;
				for (int j = 0; j < TotalX; j++)
				{
					isVisitable = false;
					Vector2 rayPoint = point + Vector2.right * cellSizeX * j;
					var col = Physics2D.OverlapPointAll(rayPoint, _mapLayer);
					if (col != null && col.Length == 1)
					{
						if (col[0].gameObject.layer == groundLayer && col[0].gameObject.tag != "NotMoveable")
							isVisitable = true;
					}

					_map[i, j] = new Node(rayPoint, new Index(j, i), isVisitable);
				}
			}
		}

		/// <summary>
		/// ���� ��ġ�� ��尡 �����ϴ� ��ġ�� ��� ��带 ��ȯ�մϴ�.
		/// </summary>
		public bool TryGetNode(Vector2 position, out Node node)
		{
			node = null;

			var col = Physics2D.OverlapPoint(position, _mapLayer);
			if (col == null)
				return false;

			var localPos = position - _bottomLeft;
			int y = (int)(localPos.y / _grid.cellSize.y);
			int x = (int)(localPos.x / _grid.cellSize.x);

			node = _map[y, x];

			return true;
		}

#if UNITY_EDITOR
		/// <summary>
		/// ������ �󿡼� �� �����͸� �����ݴϴ�.
		/// </summary>
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawWireCube(transform.position + (Vector3)Offset, Size);

			if (_map == null)
				return;

			for (int i = 0; i < TotalY; i++)
			{
				for (int j = 0; j < TotalX; j++)
				{
					Gizmos.color = _map[i, j].IsVisitable ? Color.green : Color.red;
					Gizmos.DrawCube(_map[i, j].Position, Vector2.one * 0.2f);
				}
			}
		}
	}
#endif
	#endregion
}


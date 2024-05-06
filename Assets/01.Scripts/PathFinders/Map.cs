using UnityEngine;

namespace TopdownShooter.Pathfinders
{
	public class Map : MonoBehaviour
	{
		#region Property
		/// <summary>
		/// Map 싱글톤 인스턴스
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
		/// Map의 World공간에서의 CellSize
		/// </summary>
		public Vector2 CellSize => _grid.cellSize;

		/// <summary>
		/// World에서 맵 위치 Offset
		/// </summary>
		[field: SerializeField] public Vector2 Offset { get; private set; }

		/// <summary>
		/// Map의 크기
		/// </summary>
		[field: SerializeField] public Vector2 Size { get; private set; }

		/// <summary>
		/// Map의 Y축의 갯수
		/// </summary>
		public int TotalY { get; private set; }

		/// <summary>
		/// Map의 X축의 갯수
		/// </summary>
		public int TotalX { get; private set; }

		/// <summary>
		/// Map에 Node를 Node.Index를 통해서 접근합니다.
		/// </summary>
		/// <param name="index"></param>
		public Node this[Index index] => _map[index.y, index.x];

		/// <summary>
		/// Map의 Node를 [y,x]를 통해서 접근합니다.
		/// </summary>
		public Node this[int y, int x] => _map[y, x];
		#endregion


		#region Field
		/// <summary>
		/// 맵 인스턴스
		/// </summary>
		private static Map _instance;

		/// <summary>
		/// Map 데이터
		/// </summary>
		private Node[,] _map;

		/// <summary>
		/// 타일맵 Cell 정보를 가지고 있습니다.
		/// </summary>
		private Grid _grid;

		/// <summary>
		/// 맵 데이터에 영향을 줄 레이어들을 설정합니다.
		/// Ground  : 이동 가능 노드
		/// Wall	: 이동 불가능 노드
		/// </summary>
		[SerializeField] private LayerMask _mapLayer;

		/// <summary>
		/// 맵에 왼쪽 아래에서의 끝 위치
		/// </summary>
		private Vector2 _bottomLeft;

		/// <summary>
		/// 맵에 오른쪽 위에서의 끝 위치
		/// </summary>
		private Vector2 _topRight;

		/// <summary>
		/// 맨 왼쪽 아래의 노드의 위치
		/// </summary>
		private Vector2 _bottomLeftNodePoint;

		/// <summary>
		/// 맨 오른쪽 위의 노드의 위치
		/// </summary>
		private Vector2 _topRightNodePoint;
		#endregion


		#region Function
		private void Awake()
		{
			Init();
		}

		/// <summary>
		/// 싱글톤에 인스턴스를 등록하고 맵을 생성을 호출합니다.
		/// </summary>
		public void Init()
		{
			_grid = GetComponent<Grid>();
			SetUp();
			Instance = this;
		}

		/// <summary>
		/// 설정한 영역에 맞추어서 맵 데이터를 생성합니다.
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
		/// 현재 위치가 노드가 존재하는 위치인 경우 노드를 반환합니다.
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
		/// 에디터 상에서 맵 데이터를 보여줍니다.
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


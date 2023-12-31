using Unity.VisualScripting;
using UnityEngine;

namespace TopdownShooter.Pathfinders
{
	public class Map : MonoBehaviour
	{

		[SerializeField] private Vector2 _size;
		[SerializeField] private Vector2 _offset;
		[SerializeField] private LayerMask _mapLayer;
		private Grid _grid;

		private Vector2 _origin;
		private Vector2 _end;
		public int totalY { get; private set; }
		public int totalX { get; private set; }

		private Node[,] _map;

		public Node this[Node.Index index] => _map[index.y, index.x];
		public Node this[int y, int x] => _map[y, x];

		private Vector2 _bottomLeft;

		private static Map _instance;
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

		public Vector2 cellSize => _grid.cellSize;
		public Vector2 offset => _offset;
		public Vector2 size => _size;

		private void Awake()
		{
			Init();
		}

		public void Init()
		{
			_grid = GetComponent<Grid>();
			SetUp();
			Instance = this;
		}

		private void SetUp()
		{
			Vector2 center = (Vector2)transform.position + _offset;
			_bottomLeft = center - (_size * 0.5f);
			_origin = _bottomLeft + ((Vector2)_grid.cellSize * 0.5f);

			Vector2 topRight = center + (_size * 0.5f);
			_end = topRight - ((Vector2)_grid.cellSize * 0.5f);

			float cellSizeX = _grid.cellSize.x;
			float cellSizeY = _grid.cellSize.y;

			totalY = (int)((_end.y - _origin.y) / _grid.cellSize.y) + 1;
			totalX = (int)((_end.x - _origin.x) / _grid.cellSize.x) + 1;

			_map = new Node[totalY, totalX];

			Vector2 point = _origin;
			bool isVisitable;

			int groundLayer = LayerMask.NameToLayer("Ground");

			for (int i = 0; i < totalY; i++)
			{
				point = _origin + Vector2.up * cellSizeY * i;
				for (int j = 0; j < totalX; j++)
				{
					isVisitable = false;
					Vector2 rayPoint = point + Vector2.right * cellSizeX * j;
					var col = Physics2D.OverlapPointAll(rayPoint, _mapLayer);
					if (col != null && col.Length == 1)
					{
						if (col[0].gameObject.layer == groundLayer && col[0].gameObject.tag != "NotMoveable")
							isVisitable = true;
					}

					_map[i, j] = new Node(rayPoint, new Node.Index(j, i), isVisitable);
				}
			}
		}

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

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawWireCube(transform.position + (Vector3)_offset, _size);

			if (_map == null)
				return;

			for (int i = 0; i < totalY; i++)
			{
				for (int j = 0; j < totalX; j++)
				{
					Gizmos.color = _map[i, j].isVisitable ? Color.green : Color.red;
					Gizmos.DrawCube(_map[i, j].position, Vector2.one * 0.2f);
				}
			}
		}
	}
}


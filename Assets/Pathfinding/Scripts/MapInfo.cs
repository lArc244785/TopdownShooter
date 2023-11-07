using System;
using UnityEngine;

public class MapInfo : MonoBehaviour
{
	[Serializable]
	public struct Node
	{
		public Coord coord;
		public int layer;
		public int itemID;

		public Node(Coord coord, int layer, int itemID)
		{
			this.coord = coord;
			this.layer = layer;
			this.itemID = itemID;
		}
	}
	[Serializable]
	public struct Coord
	{
		public int y, x;

		public Coord(int y, int x)
		{
			this.y = y;
			this.x = x;
		}
	}

	private BoxCollider2D _bound;
	private Grid _grid;
	[SerializeField] private LayerMask _mapMask;

	private Vector2 _origin;
	private Vector2 _end;
	private int _totalY;
	private int _totalX;

	private Node[,] _map;

	private void Awake()
	{
		_bound = GetComponent<BoxCollider2D>();
		_grid = GetComponent<Grid>();
		SetUp();
		Debug.Log(_map[_totalY - 1, _totalX - 1]);
	}

	private void SetUp()
	{
		Vector2 center = (Vector2)transform.position + _bound.offset;
		Vector2 bottomLeft = center - (_bound.size * 0.5f);
		_origin = bottomLeft + ((Vector2)_grid.cellSize * 0.5f);

		Vector2 topRight = center + (_bound.size * 0.5f);
		_end = topRight - ((Vector2)_grid.cellSize * 0.5f);

		float cellSizeX = _grid.cellSize.x;
		float cellSizeY = _grid.cellSize.y;

		_totalY = (int)((_end.y - _origin.y) / _grid.cellSize.y) + 1;
		_totalX = (int)((_end.x - _origin.x) / _grid.cellSize.x) + 1;

		_map = new Node[_totalY, _totalX];

		int towerNodeLayer = LayerMask.NameToLayer("TowerNode");
		int enemyLayer = LayerMask.NameToLayer("EnmeyPath");


		Vector2 point = _origin;
		for (int i = 0; i < _totalY; i++)
		{
			point = _origin + Vector2.up * cellSizeY * i;
			for (int j = 0; j < _totalX; j++)
			{
				var col = Physics2D.OverlapPoint(point + Vector2.right * cellSizeY * j, _mapMask);
				int layer = 0;
				if (col != null)
				{
					if (col.gameObject.layer == towerNodeLayer)
						layer = towerNodeLayer;
					else if (col.gameObject.layer == enemyLayer)
						layer = enemyLayer;
				}

				_map[i, j] = new Node(new Coord(j, i), layer, 0);
			}
		}
	}
}

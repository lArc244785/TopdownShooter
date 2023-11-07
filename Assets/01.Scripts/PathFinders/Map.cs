using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopdownShooter.Pathfinders
{
	public class Map : MonoBehaviour
	{
		[SerializeField] private Vector2 _size;
		[SerializeField] private Vector2 _offset;
		[SerializeField] private LayerMask _moveableLayer;
		[SerializeField] private Vector2Int _cellAmount;
		private Node[,] _map;

		private void Awake()
		{
			
		}

		private void SetUp()
		{
			_map = new Node[_cellAmount.y, _cellAmount.x];

			Vector2 cellSize = _size / _cellAmount;
			Vector2 mapBottomLeft = (Vector2)transform.position + _offset +
									_size.x * 0.5f * Vector2.left +
									_size.y * 0.5f * Vector2.down;
			Vector2 cellStartPos = mapBottomLeft + cellSize * 0.5f;

		}
	}
}


using System.Collections.Generic;
using TopdownShooter.Characters;
using TopdownShooter.Pathfinders;
using Unity.VisualScripting;
using UnityEngine;

namespace TopdownShooter
{
	public enum MiniMapIconType
	{
		None = -1,
		Player,
		Enemy,
		Target,
	}

	public class MiniMap : MonoBehaviour
	{
		[SerializeField] private Color _groundColor;
		[SerializeField] private Color _wallColor;
		[SerializeField] private SpriteRenderer _prefabTile;
		[SerializeField] private Transform _camera;

		private List<Transform> _targetList = new List<Transform>();
		private List<SpriteRenderer> _objectTileList = new List<SpriteRenderer>();

		private Transform _trackingTarget;

		[SerializeField] private Color _playerIconColor;
		[SerializeField] private Color _enemyIconColor;
		[SerializeField] private Color _targetIconColor;

		public static MiniMap instance { get; private set; }

		private void Awake()
		{
			instance = this;
		}

		private void Start()
		{
			DrawMap();
		}


		private void LateUpdate()
		{
			for(int i =0; i < _objectTileList.Count; i++)
			{
				_objectTileList[i].transform.localPosition = _targetList[i].transform.position;
			}

			_camera.position = new Vector3(_trackingTarget.position.x, _trackingTarget.position.y, _camera.position.z);
		}


		private void DrawMap()
		{
			if (Map.Instance == null)
				throw new System.Exception("Map Instance is Null");

			var center = (Vector2)transform.position + Map.Instance.offset;
			Vector2 bottomLeft = center - (Map.Instance.size * 0.5f);
			Vector2 origin = bottomLeft + (Map.Instance.cellSize * 0.5f);

			Vector2 topRight = center + (Map.Instance.size * 0.5f);
			Vector2 end = topRight - (Map.Instance.size * 0.5f);


			for (int i = 0; i < Map.Instance.totalY; i++)
			{
				Vector2 point = origin + Vector2.up * Map.Instance.cellSize.y * i;
				for (int j = 0; j < Map.Instance.totalX; j++)
				{
					Vector2 spawnPoint = point + Vector2.right * Map.Instance.cellSize.x * j;
					if (Map.Instance[i,j].isVisitable || CanWallTileInstance(new(j, i)))
					{
						var tile = Instantiate(_prefabTile, spawnPoint, Quaternion.identity);
						tile.transform.parent = transform;
						tile.color = Map.Instance[i, j].isVisitable ? _groundColor : _wallColor;
					}

				}
			}
		}

		private bool CanWallTileInstance(Vector2Int point)
		{
			Vector2Int[] dir = { new(0, 1), new(1, 1), new(1, 0), new(1, -1), new(0, -1), new(-1, -1), new(-1, 0), new(-1, 1) };
			bool isInstanceWallTile = false;

			for(int i = 0; i < dir.Length; i++)
			{
				Vector2Int checkIndex = point + dir[i];
				if (checkIndex.x < 0 || checkIndex.x >= Map.Instance.totalX)
					continue;
				if (checkIndex.y < 0 || checkIndex.y >= Map.Instance.totalY)
					continue;

				if (Map.Instance[checkIndex.y, checkIndex.x].isVisitable)
				{
					isInstanceWallTile = true;
					break;
				}
			}
			return isInstanceWallTile;
		}

		public void ReginsterObject(Transform target, MiniMapIconType iconType)
		{
			var objectTile =  Instantiate(_prefabTile);
			objectTile.transform.parent = transform;
			objectTile.transform.localPosition = target.position;
			objectTile.transform.localScale = Vector3.one * 0.5f;
			objectTile.sortingOrder = 1;

			switch (iconType)
			{
				case MiniMapIconType.Player:
					objectTile.color = _playerIconColor;
					objectTile.transform.name = "MiniMapIcon Player";
					_trackingTarget = objectTile.transform;
					break;
				case MiniMapIconType.Enemy:
					objectTile.color = _enemyIconColor;
					objectTile.transform.name = "MiniMapIcon Enemy";
					break;
				case MiniMapIconType.Target:
					objectTile.color = _targetIconColor;
					objectTile.transform.name = "MiniMapIcon Target";
					objectTile.transform.localScale = Vector3.one;
					break;
			}

			_targetList.Add(target);
			_objectTileList.Add(objectTile);
			if(target.TryGetComponent<IHP>(out var hp))
			{
				hp.onHpMin += () =>
				{
					_targetList.Remove(target);
					_objectTileList.Remove(objectTile);
					Destroy(objectTile);
				};
			}

		}

	}

}


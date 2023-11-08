using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace TopdownShooter.Pathfinders
{
	public abstract class PathFinder : MonoBehaviour
	{
		protected enum StraightMove
		{
			None = -1,
			Up,
			Right,
			Down,
			Left,
		}

		protected enum DiagonalMove
		{
			None = -1,
			UpRight,
			DownRight,
			DownLeft,
			UpLeft,
		}

		protected bool[,] closePathTable;

		protected List<Node> visitNodeList;
		[SerializeField] protected Map map;

		protected Node.Index[] straightMoveDir =
		{
			new Node.Index(0,1),
			new Node.Index(1,0),
			new Node.Index(0,-1),
			new Node.Index(-1,0),
		};

		protected Node.Index[] diagonalMoveDir =
		{
			new Node.Index(1,1),
			new Node.Index(1,-1),
			new Node.Index(-1,-1),
			new Node.Index(-1,1),
		};


		protected virtual void Awake()
		{
			if (!map.isInit)
				map.Init();

			visitNodeList = new();
			closePathTable = new bool[map.totalY, map.totalX];
			ResetPath();
		}

		public void ResetPath()
		{
			visitNodeList.Clear();

			for (int i = 0; i < map.totalY; i++)
			{
				for (int j = 0; j < map.totalX; j++)
				{
					closePathTable[i, j] = !map[i, j].isVisitable;
				}
			}
		}

		public abstract bool TryGetPath(Vector2 startPos,Vector2 targetPos, out Vector2[] paths);

		protected virtual void OnDrawGizmosSelected()
		{
			if (visitNodeList == null)
				return;

			Gizmos.color = Color.yellow;

			foreach(var node in visitNodeList)
			{
				Gizmos.DrawCube(node.position, Vector3.one * 0.3f);
			}

		}

		private bool IsNodeExistence(Node.Index index)
		{
			if (index.x < 0 || index.x >= map.totalX) return false;
			if (index.y < 0 || index.y >= map.totalY) return false;

			return true;
		}

		protected bool CanMoveStraight(Node.Index index,StraightMove dir)
		{
			if(dir == StraightMove.None) return false;
			Node.Index moveIndex = index + straightMoveDir[(int)dir];

			return IsNodeExistence(moveIndex) && !closePathTable[moveIndex.y, moveIndex.x];
		}


		protected bool CanMoveDiagonal(Node.Index index, DiagonalMove dir)
		{
			if (dir == DiagonalMove.None) return false;
			Node.Index moveIndex = index + diagonalMoveDir[(int)dir];

			if(!IsNodeExistence(moveIndex) || closePathTable[moveIndex.y, moveIndex.x]) return false;

			switch (dir)
			{
				case DiagonalMove.UpRight:
					{
						Node.Index left = moveIndex + new Node.Index(-1, 0);
						Node.Index down = moveIndex + new Node.Index(0, -1);

						if(!IsNodeExistence(left) || closePathTable[left.y, left.x])
							return false;
						if(!IsNodeExistence(down) || closePathTable[down.y, down.x])
							return false;
					break;
					}
				case DiagonalMove.DownRight:
					{
						Node.Index left = moveIndex + new Node.Index(-1, 0);
						Node.Index up = moveIndex + new Node.Index(0, 1);

						if (!IsNodeExistence(left) || closePathTable[left.y, left.x])
							return false;
						if (!IsNodeExistence(up) || closePathTable[up.y, up.x])
							return false;
					break;
					}
				case DiagonalMove.DownLeft:
					{
						Node.Index right = moveIndex + new Node.Index(1, 0);
						Node.Index up = moveIndex + new Node.Index(0, 1);

						if (!IsNodeExistence(right) || closePathTable[right.y, right.x])
							return false;
						if (!IsNodeExistence(up) || closePathTable[up.y, up.x])
							return false;
						break;
					}
				case DiagonalMove.UpLeft:
					{
						Node.Index right = moveIndex + new Node.Index(1, 0);
						Node.Index down = moveIndex + new Node.Index(0, -1);

						if (!IsNodeExistence(right) || closePathTable[right.y, right.x])
							return false;
						if (!IsNodeExistence(down) || closePathTable[down.y, down.x])
							return false;
						break;
					}
			}

			return true;
		}
	}
}

using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace TopdownShooter.Pathfinders
{
	public abstract class PathFinder
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

		protected bool[,] openPathTable;

		protected List<Node> visitNodeList;

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

		protected const int MOVE_DIR_LENGTH = 4;


		public PathFinder()
		{
			visitNodeList = new(1000);
			openPathTable = new bool[Map.Instance.TotalY, Map.Instance.TotalX];
		}

		public void ResetPath()
		{
			visitNodeList.Clear();

			for (int i = 0; i < Map.Instance.TotalY; i++)
			{
				for (int j = 0; j < Map.Instance.TotalX; j++)
				{
					openPathTable[i, j] = Map.Instance[i, j].IsVisitable;
				}
			}
		}

		public abstract bool TryGetPath(Vector2 startPos,Vector2 targetPos, out Vector2[] paths);

		private bool IsNodeExistence(Node.Index index)
		{
			if (index.x < 0 || index.x >= Map.Instance.TotalX) return false;
			if (index.y < 0 || index.y >= Map.Instance.TotalY) return false;

			return true;
		}

		/// <summary>
		///  1. 노드가 존재하는가?, 노드가 방문이 가능한가?, 노드로 가는 경로가 열려있는가?
		/// </summary>
		private bool CanNodeMoveable(Node.Index index)
		{
			return IsNodeExistence(index) && Map.Instance[index].IsVisitable && openPathTable[index.y, index.x];
		}


		protected bool CanMoveStraight(Node.Index currentIndex, StraightMove moveDir)
		{
			if(moveDir == StraightMove.None) return false;
			Node.Index moveIndex = currentIndex + straightMoveDir[(int)moveDir];

			return CanNodeMoveable(moveIndex);
		}


		protected bool CanMoveDiagonal(Node.Index currentIndex, DiagonalMove moveDir)
		{
			if (moveDir == DiagonalMove.None) return false;
			Node.Index moveIndex = currentIndex + diagonalMoveDir[(int)moveDir];

			if(!CanNodeMoveable(moveIndex)) return false;

			//대각 이동시 주변 타일이 존재하고 주변 타일중에 벽이 없는 경우 그리고 해당 노드가 방문을 했던 노드가 아닌경우

			switch (moveDir)
			{
				case DiagonalMove.UpRight:
					{
						Node.Index left = moveIndex + new Node.Index(-1, 0);
						Node.Index down = moveIndex + new Node.Index(0, -1);

						if(!CanNodeMoveable(left))
							return false;
						if(!CanNodeMoveable(down))
							return false;
					break;
					}
				case DiagonalMove.DownRight:
					{
						Node.Index left = moveIndex + new Node.Index(-1, 0);
						Node.Index up = moveIndex + new Node.Index(0, 1);

						if (!CanNodeMoveable(left))
							return false;
						if (!CanNodeMoveable(up))
							return false;
					break;
					}
				case DiagonalMove.DownLeft:
					{
						Node.Index right = moveIndex + new Node.Index(1, 0);
						Node.Index up = moveIndex + new Node.Index(0, 1);

						if (!CanNodeMoveable(right))
							return false;
						if (!CanNodeMoveable(up))
							return false;
						break;
					}
				case DiagonalMove.UpLeft:
					{
						Node.Index right = moveIndex + new Node.Index(1, 0);
						Node.Index down = moveIndex + new Node.Index(0, -1);

						if (!CanNodeMoveable(right))
							return false;
						if (!CanNodeMoveable(down))
							return false;
						break;
					}
			}

			return true;
		}
	}
}

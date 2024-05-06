using System.Collections.Generic;
using UnityEngine;

namespace TopdownShooter.Pathfinders
{
	public abstract class PathFinder
	{
		#region Constructor
		public PathFinder()
		{
			visitNodeList = new(1000);
			openPathTable = new bool[Map.Instance.TotalY, Map.Instance.TotalX];
		}
		#endregion


		#region Enum
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
		#endregion


		#region Field
		/// <summary>
		/// Map 데이터에서 가져온 방문 가능에 대한 정보를 복제해서 사용합니다.
		/// </summary>
		protected bool[,] openPathTable;

		/// <summary>
		/// 방문한 노드들의 정보를 담는 리스트입니다.
		/// </summary>
		protected List<Node> visitNodeList;

		/// <summary>
		/// 방향 정보들의 크기
		/// </summary>
		protected const int MOVE_DIR_LENGTH = 4;

		/// <summary>
		/// 직선 움직임 방향 정보들
		/// </summary>
		protected Index[] straightMoveDir =
		{
			new Index(0,1),
			new Index(1,0),
			new Index(0,-1),
			new Index(-1,0),
		};

		/// <summary>
		/// 대각 움직임 방향 정보들
		/// </summary>
		protected Index[] diagonalMoveDir =
		{
			new Index(1,1),
			new Index(1,-1),
			new Index(-1,-1),
			new Index(-1,1),
		};
		#endregion


		#region Method
		/// <summary>
		/// 방문한 노드 정보를 초기화하고 원본 맵 데이터에 맞추어 데이터를 설정합니다.
		/// </summary>
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

		/// <summary>
		/// 길찾기를 시도합니다.
		/// </summary>
		/// <param name="startPos">시작 위치</param>
		/// <param name="targetPos">목표 위치</param>
		/// <param name="paths">발견한 경로 정보</param>
		/// <returns>성공 여부</returns>
		public abstract bool TryGetPath(Vector2 startPos, Vector2 targetPos, out Vector2[] paths);

		/// <summary>
		/// 해당 위치가 맵의 범위안에 있는지 확인합니다.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		private bool IsNodeExistence(Index index)
		{
			if (index.x < 0 || index.x >= Map.Instance.TotalX) return false;
			if (index.y < 0 || index.y >= Map.Instance.TotalY) return false;

			return true;
		}

		/// <summary>
		///  해당 위치의 노드로 이동할 수 있는지 확인합니다.
		/// </summary>
		private bool CanNodeMoveable(Index index)
		{
			return IsNodeExistence(index) && Map.Instance[index].IsVisitable && openPathTable[index.y, index.x];
		}

		/// <summary>
		/// 직선으로 이동이 가능한지 확인합니다.
		/// </summary>
		protected bool CanMoveStraight(Index currentIndex, StraightMove moveDir)
		{
			if (moveDir == StraightMove.None) return false;
			Index moveIndex = currentIndex + straightMoveDir[(int)moveDir];

			return CanNodeMoveable(moveIndex);
		}

		/// <summary>
		/// 대각으로 이동이 가능한지 확인합니다.
		/// </summary>
		protected bool CanMoveDiagonal(Index currentIndex, DiagonalMove moveDir)
		{
			if (moveDir == DiagonalMove.None) return false;
			Index moveIndex = currentIndex + diagonalMoveDir[(int)moveDir];

			if (!CanNodeMoveable(moveIndex)) return false;

			//대각 이동시 주변 타일이 존재하고 주변 타일중에 벽이 없는 경우 그리고 해당 노드가 방문을 했던 노드가 아닌경우

			switch (moveDir)
			{
				case DiagonalMove.UpRight:
					{
						Index left = moveIndex + new Index(-1, 0);
						Index down = moveIndex + new Index(0, -1);

						if (!CanNodeMoveable(left))
							return false;
						if (!CanNodeMoveable(down))
							return false;
						break;
					}
				case DiagonalMove.DownRight:
					{
						Index left = moveIndex + new Index(-1, 0);
						Index up = moveIndex + new Index(0, 1);

						if (!CanNodeMoveable(left))
							return false;
						if (!CanNodeMoveable(up))
							return false;
						break;
					}
				case DiagonalMove.DownLeft:
					{
						Index right = moveIndex + new Index(1, 0);
						Index up = moveIndex + new Index(0, 1);

						if (!CanNodeMoveable(right))
							return false;
						if (!CanNodeMoveable(up))
							return false;
						break;
					}
				case DiagonalMove.UpLeft:
					{
						Index right = moveIndex + new Index(1, 0);
						Index down = moveIndex + new Index(0, -1);

						if (!CanNodeMoveable(right))
							return false;
						if (!CanNodeMoveable(down))
							return false;
						break;
					}
			}

			return true;
		}
		#endregion
	}
}

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
		/// Map �����Ϳ��� ������ �湮 ���ɿ� ���� ������ �����ؼ� ����մϴ�.
		/// </summary>
		protected bool[,] openPathTable;

		/// <summary>
		/// �湮�� ������ ������ ��� ����Ʈ�Դϴ�.
		/// </summary>
		protected List<Node> visitNodeList;

		/// <summary>
		/// ���� �������� ũ��
		/// </summary>
		protected const int MOVE_DIR_LENGTH = 4;

		/// <summary>
		/// ���� ������ ���� ������
		/// </summary>
		protected Index[] straightMoveDir =
		{
			new Index(0,1),
			new Index(1,0),
			new Index(0,-1),
			new Index(-1,0),
		};

		/// <summary>
		/// �밢 ������ ���� ������
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
		/// �湮�� ��� ������ �ʱ�ȭ�ϰ� ���� �� �����Ϳ� ���߾� �����͸� �����մϴ�.
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
		/// ��ã�⸦ �õ��մϴ�.
		/// </summary>
		/// <param name="startPos">���� ��ġ</param>
		/// <param name="targetPos">��ǥ ��ġ</param>
		/// <param name="paths">�߰��� ��� ����</param>
		/// <returns>���� ����</returns>
		public abstract bool TryGetPath(Vector2 startPos, Vector2 targetPos, out Vector2[] paths);

		/// <summary>
		/// �ش� ��ġ�� ���� �����ȿ� �ִ��� Ȯ���մϴ�.
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
		///  �ش� ��ġ�� ���� �̵��� �� �ִ��� Ȯ���մϴ�.
		/// </summary>
		private bool CanNodeMoveable(Index index)
		{
			return IsNodeExistence(index) && Map.Instance[index].IsVisitable && openPathTable[index.y, index.x];
		}

		/// <summary>
		/// �������� �̵��� �������� Ȯ���մϴ�.
		/// </summary>
		protected bool CanMoveStraight(Index currentIndex, StraightMove moveDir)
		{
			if (moveDir == StraightMove.None) return false;
			Index moveIndex = currentIndex + straightMoveDir[(int)moveDir];

			return CanNodeMoveable(moveIndex);
		}

		/// <summary>
		/// �밢���� �̵��� �������� Ȯ���մϴ�.
		/// </summary>
		protected bool CanMoveDiagonal(Index currentIndex, DiagonalMove moveDir)
		{
			if (moveDir == DiagonalMove.None) return false;
			Index moveIndex = currentIndex + diagonalMoveDir[(int)moveDir];

			if (!CanNodeMoveable(moveIndex)) return false;

			//�밢 �̵��� �ֺ� Ÿ���� �����ϰ� �ֺ� Ÿ���߿� ���� ���� ��� �׸��� �ش� ��尡 �湮�� �ߴ� ��尡 �ƴѰ��

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

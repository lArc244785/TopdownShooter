using System.Collections.Generic;
using UnityEngine;

namespace TopdownShooter.Pathfinders
{
	public class PathFinderBFS : PathFinder
	{
		#region Method
		/// <summary>
		/// ���۳�忡�� ����� ������ Ž���ϸ鼭 ���� ã���ϴ�.
		/// </summary>
		public override bool TryGetPath(Vector2 startPos, Vector2 targetPos, out Vector2[] paths)
		{
			ResetPath();

			paths = null;
			Node startNode = null;
			Node targetNode = null;

			if (!Map.Instance.TryGetNode(startPos, out startNode) || !Map.Instance.TryGetNode(targetPos, out targetNode))
				return false;

			Queue<Node> openPath = new Queue<Node>();
			openPath.Enqueue(startNode);

			visitNodeList.Add(startNode);
			openPathTable[startNode.Point.y, startNode.Point.x] = false;

			bool isFound = false;
			Node endNode = null;

			while (openPath.Count > 0 && !isFound)
			{
				Node node = openPath.Dequeue();
				isFound = node.Point == targetNode.Point;
				if (!isFound)
					UpdateMoveablePaths(in openPath, node);
				else
					endNode = node;
			}

			if (!isFound)
				return false;

			Node pathNode = endNode;
			List<Vector2> pathList = new List<Vector2>();

			while (pathNode != null)
			{
				pathList.Add(pathNode.Position);
				pathNode = pathNode.Parent;
			}

			pathList.Reverse();
			paths = pathList.ToArray();

			return true;
		}

		/// <summary>
		/// ���� ��忡�� �������� �湮�� �� �ִ� ��带 ������Ʈ�մϴ�.
		/// </summary>
		/// <param name="openPath"></param>
		/// <param name="currentNode"></param>
		private void UpdateMoveablePaths(in Queue<Node> openPath, Node currentNode)
		{
			Index point = currentNode.Point;

			for (int i = 0; i < MOVE_DIR_LENGTH; i++)
			{
				//���� �̵� ���� ��� �߰�
				if (CanMoveStraight(point, (StraightMove)i))
				{
					Index nextIndex = point + straightMoveDir[i];
					Node visitNode = Map.Instance[nextIndex].GetClone();
					visitNode.Parent = currentNode;
					visitNodeList.Add(visitNode);
					openPathTable[nextIndex.y, nextIndex.x] = false;
					openPath.Enqueue(visitNode);
				}

				//�밢 �̵� ���� ��� �߰�
				if (CanMoveDiagonal(point, (DiagonalMove)i))
				{
					Index nextIndex = point + diagonalMoveDir[i];
					Node visitNode = Map.Instance[nextIndex].GetClone();
					visitNode.Parent = currentNode;
					visitNodeList.Add(visitNode);
					openPathTable[nextIndex.y, nextIndex.x] = false;
					openPath.Enqueue(visitNode);
				}
			}
		}
		#endregion
	}
}

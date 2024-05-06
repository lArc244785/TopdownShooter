using System.Collections.Generic;
using UnityEngine;

namespace TopdownShooter.Pathfinders
{
	public class PathFinderBFS : PathFinder
	{
		#region Method
		/// <summary>
		/// 시작노드에서 가까운 노드부터 탐색하면서 길을 찾습니다.
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
		/// 현재 노드에서 다음으로 방문할 수 있는 노드를 업데이트합니다.
		/// </summary>
		/// <param name="openPath"></param>
		/// <param name="currentNode"></param>
		private void UpdateMoveablePaths(in Queue<Node> openPath, Node currentNode)
		{
			Index point = currentNode.Point;

			for (int i = 0; i < MOVE_DIR_LENGTH; i++)
			{
				//직선 이동 가능 경로 추가
				if (CanMoveStraight(point, (StraightMove)i))
				{
					Index nextIndex = point + straightMoveDir[i];
					Node visitNode = Map.Instance[nextIndex].GetClone();
					visitNode.Parent = currentNode;
					visitNodeList.Add(visitNode);
					openPathTable[nextIndex.y, nextIndex.x] = false;
					openPath.Enqueue(visitNode);
				}

				//대각 이동 가능 경로 추가
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

using System.Collections.Generic;
using UnityEngine;

namespace TopdownShooter.Pathfinders
{
	public class PathFinderDFS : PathFinder
	{
		#region Method
		/// <summary>
		/// 발견한 노드를 우선적으로 탐색하면서 길을 찾습니다.
		/// </summary>
		public override bool TryGetPath(Vector2 startPos, Vector2 targetPos, out Vector2[] paths)
		{
			ResetPath();

			paths = null;
			Node startNode = null;
			Node targetNode = null;

			if (!Map.Instance.TryGetNode(startPos, out startNode) || !Map.Instance.TryGetNode(targetPos, out targetNode))
				return false;

			Stack<Node> openPath = new Stack<Node>();
			openPath.Push(startNode);
			bool isFound = false;
			Node endNode = null;


			visitNodeList.Add(startNode);
			openPathTable[startNode.Point.y, startNode.Point.x] = false;

			while (openPath.Count > 0 && !isFound)
			{
				Node node = openPath.Pop();
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
		/// 현재 노드에서 다음으로 이동할 수 있는 노드를 업데이트합니다.
		/// </summary>
		private void UpdateMoveablePaths(in Stack<Node> openPath, Node currentNode)
		{
			Index point = currentNode.Point;

			for (int i = MOVE_DIR_LENGTH - 1; i >= 0; i--)
			{
				if (CanMoveDiagonal(point, (DiagonalMove)i))
				{
					Index nextIndex = point + diagonalMoveDir[i];
					Node visitNode = Map.Instance[nextIndex].GetClone();
					visitNode.Parent = currentNode;
					visitNodeList.Add(visitNode);
					openPathTable[nextIndex.y, nextIndex.x] = false;
					openPath.Push(visitNode);
				}

				if (CanMoveStraight(point, (StraightMove)i))
				{
					Index nextIndex = point + straightMoveDir[i];
					Node visitNode = Map.Instance[nextIndex].GetClone();
					visitNode.Parent = currentNode;
					visitNodeList.Add(visitNode);
					openPathTable[nextIndex.y, nextIndex.x] = false;
					openPath.Push(visitNode);
				}
			}
		}
		#endregion

	}
}

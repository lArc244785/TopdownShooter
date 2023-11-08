using System.Collections.Generic;
using UnityEngine;

namespace TopdownShooter.Pathfinders
{
	public class PathFinderBFS : PathFinder
	{


		public override bool TryGetPath(Vector2 startPos, Vector2 targetPos, out Vector2[] paths)
		{
			ResetPath();

			paths = null;
			Node startNode = null;
			Node targetNode = null;

			if (!map.TryGetNode(startPos, out startNode) || !map.TryGetNode(targetPos, out targetNode))
				return false;

			Queue<Node> openPath = new Queue<Node>();
			openPath.Enqueue(startNode);
			closePathTable[startNode.index.y, startNode.index.x] = true;
			bool isFound = false;

			Node endNode = null;
			while (openPath.Count > 0 && !isFound)
			{
				Node node = openPath.Dequeue();
				isFound = node.index == targetNode.index;
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
				pathList.Add(pathNode.position);
				pathNode = pathNode.parent;
			}

			pathList.Reverse();
			paths = pathList.ToArray();

			return true;
		}

		private void UpdateMoveablePaths(in Queue<Node> openPath, Node currentNode)
		{
			Node.Index point = currentNode.index;



			//직선 이동 가능 경로 확인
			for (int i = 0; i < straightMoveDir.Length; i++)
			{
				if (CanMoveStraight(point, (StraightMove)i))
				{
					Node.Index nextIndex = point + straightMoveDir[i];
					Node visitNode = map[nextIndex].GetClone();
					visitNode.parent = currentNode;
					visitNodeList.Add(visitNode);
					closePathTable[nextIndex.y, nextIndex.x] = true;
					openPath.Enqueue(visitNode);
				}
			}

			//대각 이동 가능 경로 확인
			for (int i = 0; i < diagonalMoveDir.Length; i++)
			{
				if (CanMoveDiagonal(point, (DiagonalMove)i))
				{
					Node.Index nextIndex = point + diagonalMoveDir[i];
					Node visitNode = map[nextIndex].GetClone();
					visitNode.parent = currentNode;
					visitNodeList.Add(visitNode);
					closePathTable[nextIndex.y, nextIndex.x] = true;
					openPath.Enqueue(visitNode);
				}
			}


		}
	}
}

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

			if (!Map.Instance.TryGetNode(startPos, out startNode) || !Map.Instance.TryGetNode(targetPos, out targetNode))
				return false;

			Queue<Node> openPath = new Queue<Node>();
			openPath.Enqueue(startNode);

			visitNodeList.Add(startNode);
			openPathTable[startNode.index.y, startNode.index.x] = false;
			
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

			for (int i = 0; i < MOVE_DIR_LENGTH; i++)
			{
				//직선 이동 가능 경로 추가
				if (CanMoveStraight(point, (StraightMove)i))
				{
					Node.Index nextIndex = point + straightMoveDir[i];
					Node visitNode = Map.Instance[nextIndex].GetClone();
					visitNode.parent = currentNode;
					visitNodeList.Add(visitNode);
					openPathTable[nextIndex.y, nextIndex.x] = false;
					openPath.Enqueue(visitNode);
				}

				//대각 이동 가능 경로 추가
				if (CanMoveDiagonal(point, (DiagonalMove)i))
				{
					Node.Index nextIndex = point + diagonalMoveDir[i];
					Node visitNode = Map.Instance[nextIndex].GetClone();
					visitNode.parent = currentNode;
					visitNodeList.Add(visitNode);
					openPathTable[nextIndex.y, nextIndex.x] = false;
					openPath.Enqueue(visitNode);
				}
			}



		}
	}
}

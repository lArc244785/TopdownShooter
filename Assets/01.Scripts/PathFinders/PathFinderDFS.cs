using System.Collections.Generic;
using UnityEngine;

namespace TopdownShooter.Pathfinders
{
	public class PathFinderDFS : PathFinder
	{
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

		private void UpdateMoveablePaths(in Stack<Node> openPath, Node currentNode)
		{
			Node.Index point = currentNode.Point;

			for (int i = MOVE_DIR_LENGTH - 1; i >= 0; i--)
			{
				if (CanMoveDiagonal(point, (DiagonalMove)i))
				{
					Node.Index nextIndex = point + diagonalMoveDir[i];
					Node visitNode = Map.Instance[nextIndex].GetClone();
					visitNode.Parent = currentNode;
					visitNodeList.Add(visitNode);
					openPathTable[nextIndex.y, nextIndex.x] = false;
					openPath.Push(visitNode);
				}

				if (CanMoveStraight(point, (StraightMove)i))
				{
					Node.Index nextIndex = point + straightMoveDir[i];
					Node visitNode = Map.Instance[nextIndex].GetClone();
					visitNode.Parent = currentNode;
					visitNodeList.Add(visitNode);
					openPathTable[nextIndex.y, nextIndex.x] = false;
					openPath.Push(visitNode);
				}
			}
		}
	}
}

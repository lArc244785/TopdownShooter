using System.Collections.Generic;
using UnityEngine;

namespace TopdownShooter.Pathfinders
{
	public class PathfFinderAStar : PathFinder
	{
		#region Field
		/// <summary>
		/// 직선 비용
		/// </summary>
		private int _straightCost = 10;

		/// <summary>
		/// 대각 비용
		/// </summary>
		private int _diagonalCost = 14;
		#endregion


		#region Method
		/// <summary>
		/// 가중치가 적은 노드를 우선적으로 탐색하면서 길을 찾습니다.
		/// </summary>
		public override bool TryGetPath(Vector2 startPos, Vector2 targetPos, out Vector2[] paths)
		{
			ResetPath();

			paths = null;
			Node startNode = null;
			Node targetNode = null;

			//현재 위치 또는 목표 위치에 노드가 존재하지 않는 경우 진행하지 않음
			if (!Map.Instance.TryGetNode(startPos, out startNode) || !Map.Instance.TryGetNode(targetPos, out targetNode))
				return false;

			PriorityQueue<Node> openPath = new PriorityQueue<Node>();
			//시작위치에서 목표 위치까지의 대각선 거리
			int current2Target = CalculationManhattanDistance(startNode.Point, targetNode.Point);
			Node startAStarNode = new Node(startNode.Position, startNode.Point, startNode.IsVisitable, 0, current2Target);

			visitNodeList.Add(startAStarNode);
			openPathTable[startAStarNode.Point.y, startAStarNode.Point.x] = false;

			openPath.Enqueue(startAStarNode);
			bool isPathFound = false;

			Node endNode = null;
			//우선 순위 큐에 있는 노드들을 탐색하면서 길찾기를 진행
			while (openPath.Count > 0 && !isPathFound)
			{
				Node node = openPath.Dequeue();
				isPathFound = node.Point == targetNode.Point;
				if (!isPathFound)
					UpdateMoveablePaths(in openPath, node, targetNode);
				else
					endNode = node;
			}

			if (!isPathFound)
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
		/// 현재 노드에서 목표까지 갈 수 있는 가장 최적의 경로로 노드들을 업데이트 합니다.
		/// </summary>
		private void UpdateMoveablePaths(in PriorityQueue<Node> openPath, Node currentNode, Node targetNode)
		{
			Index point = currentNode.Point;

			//대각 이동 가능 경로 확인
			for (int i = 0; i < diagonalMoveDir.Length; i++)
			{
				if (CanMoveDiagonal(point, (DiagonalMove)i))
				{
					Index nextIndex = point + diagonalMoveDir[i];
					Node visitNode = Map.Instance[nextIndex].GetClone();
					int g = visitNode.Parent != null ? visitNode.Parent.Start2CurrentValue + _diagonalCost : _diagonalCost;
					visitNode.Start2CurrentValue = g;
					visitNode.Current2EndValue = CalculationManhattanDistance(visitNode.Point, targetNode.Point);
					visitNode.Parent = currentNode;
					visitNodeList.Add(visitNode);
					openPathTable[visitNode.Point.y, visitNode.Point.x] = false;
					openPath.Enqueue(visitNode);
				}
			}

			//직선 이동 가능 경로 확인
			for (int i = 0; i < straightMoveDir.Length; i++)
			{
				if (CanMoveStraight(point, (StraightMove)i))
				{
					Index nextIndex = point + straightMoveDir[i];
					Node visitNode = Map.Instance[nextIndex].GetClone();
					int g = visitNode.Parent != null ? visitNode.Parent.Start2CurrentValue + _straightCost : _straightCost;
					visitNode.Start2CurrentValue = g;
					visitNode.Current2EndValue = CalculationManhattanDistance(visitNode.Point, targetNode.Point);
					visitNode.Parent = currentNode;
					visitNodeList.Add(visitNode);
					openPathTable[visitNode.Point.y, visitNode.Point.x] = false;
					openPath.Enqueue(visitNode);
				}
			}

		}

		/// <summary>
		/// 두 노드의 직선 거리를 반환합니다.
		/// </summary>
		private int CalculationManhattanDistance(Index a, Index b)
		{
			int x = Mathf.Abs((a.x - b.x)) * 10;
			int y = Mathf.Abs((a.y - b.y)) * 10;
			int powX = (int)Mathf.Pow(x, 2);
			int powY = (int)Mathf.Pow(y, 2);

			return (int)Mathf.Sqrt(powX + powY);
		}
		#endregion
	}
}
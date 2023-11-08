using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

namespace TopdownShooter.Pathfinders
{
	public class PathfFinderAStar : PathFinder
	{
		class PriorityQueue<T>
		{
			private SortedSet<PriorityQueueNode<T>> set = new SortedSet<PriorityQueueNode<T>>();

			public void Enqueue(T item, int priority)
			{
				set.Add(new PriorityQueueNode<T>(item, priority));
			}

			public T Dequeue()
			{
				if (set.Count == 0)
					throw new InvalidOperationException("Priority queue is empty.");

				var first = set.Min;
				set.Remove(first);
				return first.Value;
			}

			public bool IsEmpty()
			{
				return set.Count == 0;
			}

			public int Count
			{
				get { return set.Count; }
			}
		}

		class PriorityQueueNode<T> : IComparable<PriorityQueueNode<T>>
		{
			public T Value { get; private set; }
			public int Priority { get; private set; }

			public PriorityQueueNode(T value, int priority)
			{
				Value = value;
				Priority = priority;
			}

			public int CompareTo(PriorityQueueNode<T> other)
			{
				if (other == null)
					return 1;

				return Priority.CompareTo(other.Priority);
			}
		}

		private int _straightCost = 10;
		private int _diagonalCost = 14;



		public override bool TryGetPath(Vector2 startPos, Vector2 targetPos, out Vector2[] paths)
		{
			ResetPath();

			paths = null;
			Node startNode = null;
			Node targetNode = null;

			if (!map.TryGetNode(startPos, out startNode) || !map.TryGetNode(targetPos, out targetNode))
				return false;

			PriorityQueue<Node> openPath = new PriorityQueue<Node>();
			int h = CalculationManhattanDistance(startNode.index, targetNode.index);
			Node startAStarNode = new Node(startNode.position, startNode.index, startNode.isVisitable, 0, h);
			
			visitNodeList.Add(startAStarNode);
			openPathTable[startAStarNode.index.y, startAStarNode.index.x] = false;

			openPath.Enqueue(startAStarNode, startAStarNode.f);
			bool isPathFound = false;

			Node endNode = null;
			while (openPath.Count > 0 && !isPathFound)
			{
				Node node = openPath.Dequeue();
				isPathFound = node.index == targetNode.index;
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
				pathList.Add(pathNode.position);
				pathNode = pathNode.parent;
			}

			pathList.Reverse();
			paths = pathList.ToArray();

			return true;
		}

		private void UpdateMoveablePaths(in PriorityQueue<Node> openPath, Node currentNode, Node targetNode)
		{
			Node.Index point = currentNode.index;

			//대각 이동 가능 경로 확인
			for (int i = 0; i < diagonalMoveDir.Length; i++)
			{
				if (CanMoveDiagonal(point, (DiagonalMove)i))
				{
					Node.Index nextIndex = point + diagonalMoveDir[i];
					Node visitNode = map[nextIndex].GetClone();
					int g = visitNode.parent != null ? visitNode.parent.g + _diagonalCost : _diagonalCost;
					visitNode.g = g;
					visitNode.h = CalculationManhattanDistance(visitNode.index, targetNode.index);
					visitNode.parent = currentNode;
					visitNodeList.Add(visitNode);
					openPathTable[visitNode.index.y, visitNode.index.x] = false;
					openPath.Enqueue(visitNode, visitNode.f);
				}
			}

			//직선 이동 가능 경로 확인
			for (int i = 0; i < straightMoveDir.Length; i++)
			{
				if (CanMoveStraight(point, (StraightMove)i))
				{
					Node.Index nextIndex = point + straightMoveDir[i];
					Node visitNode = map[nextIndex].GetClone();
					int g = visitNode.parent != null ? visitNode.parent.g + _straightCost : _straightCost;
					visitNode.g = g;
					visitNode.h = CalculationManhattanDistance(visitNode.index, targetNode.index);
					visitNode.parent = currentNode;
					visitNodeList.Add(visitNode);
					openPathTable[visitNode.index.y, visitNode.index.x] = false;
					openPath.Enqueue(visitNode, visitNode.f);
				}
			}

		}

		private int CalculationManhattanDistance(Node.Index a, Node.Index b)
		{
			int x = Mathf.Abs((a.x - b.x)) * 10;
			int y = Mathf.Abs((a.y - b.y)) * 10;
			int powX = (int)Mathf.Pow(x, 2);
			int powY = (int)Mathf.Pow(y, 2);

			return (int)Mathf.Sqrt(powX + powY);
		}
	}
}
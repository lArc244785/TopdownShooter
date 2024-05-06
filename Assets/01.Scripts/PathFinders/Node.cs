using System;
using UnityEngine;

namespace TopdownShooter.Pathfinders
{
	[Serializable]
	public class Node : IComparable<Node>
	{
		#region Constructor
		public Node(Vector2 position, Index index, bool isVisitable, int start2CurrentValue = 0, int current2EndValue = 0, Node parnet = null)
		{
			Position = position;
			Point = index;
			IsVisitable = isVisitable;
			Parent = parnet;
			Start2CurrentValue = start2CurrentValue;
			Current2EndValue = current2EndValue;
		}
		#endregion


		#region Property
		/// <summary>
		/// 노드의 월드 공간에서의 위치 값
		/// </summary>
		public Vector2 Position { private set; get; }
		
		/// <summary>
		/// Map에서의 위치 값
		/// </summary>
		public Index Point { private set; get; }
		
		/// <summary>
		/// 방문 여부
		/// </summary>
		public bool IsVisitable { private set; get; }
		
		/// <summary>
		/// 시작 노드에서 현재 노드까지의 비용
		/// </summary>
		public int Start2CurrentValue { get; set; }

		/// <summary>
		/// 현재 노드에서 목표 노드까지의 비용
		/// </summary>
		public int Current2EndValue { get; set; }

		/// <summary>
		/// 가중치
		/// </summary>
		public int Weight => Start2CurrentValue + Current2EndValue;
		
		/// <summary>
		/// 노드의 부모 노드
		/// </summary>
		public Node Parent { get; set; }
		#endregion


		#region Method
		/// <summary>
		/// 해당 노드의 데이터를 복제한 클론을 반환합니다.
		/// </summary>
		public Node GetClone()
		{
			return new Node(Position, Point, IsVisitable, Start2CurrentValue, Current2EndValue, Parent);
		}

		/// <summary>
		/// 노드들의 우선 순위를 정렬할 때 사용됩니다.
		/// 정렬은 오름차순으로 정렬됩니다.
		/// </summary>
		public int CompareTo(Node other)
		{
			if (other == null)
				throw new Exception("other node is Null");

			if (Weight == other.Weight)
				return 0;

			if (Weight > other.Weight)
				return 1;

			return -1;
		}
		#endregion
	}
}
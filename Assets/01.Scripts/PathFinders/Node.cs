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
		/// ����� ���� ���������� ��ġ ��
		/// </summary>
		public Vector2 Position { private set; get; }
		
		/// <summary>
		/// Map������ ��ġ ��
		/// </summary>
		public Index Point { private set; get; }
		
		/// <summary>
		/// �湮 ����
		/// </summary>
		public bool IsVisitable { private set; get; }
		
		/// <summary>
		/// ���� ��忡�� ���� �������� ���
		/// </summary>
		public int Start2CurrentValue { get; set; }

		/// <summary>
		/// ���� ��忡�� ��ǥ �������� ���
		/// </summary>
		public int Current2EndValue { get; set; }

		/// <summary>
		/// ����ġ
		/// </summary>
		public int Weight => Start2CurrentValue + Current2EndValue;
		
		/// <summary>
		/// ����� �θ� ���
		/// </summary>
		public Node Parent { get; set; }
		#endregion


		#region Method
		/// <summary>
		/// �ش� ����� �����͸� ������ Ŭ���� ��ȯ�մϴ�.
		/// </summary>
		public Node GetClone()
		{
			return new Node(Position, Point, IsVisitable, Start2CurrentValue, Current2EndValue, Parent);
		}

		/// <summary>
		/// ������ �켱 ������ ������ �� ���˴ϴ�.
		/// ������ ������������ ���ĵ˴ϴ�.
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
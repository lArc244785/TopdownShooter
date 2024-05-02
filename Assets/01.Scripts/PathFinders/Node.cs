using System;
using UnityEngine;

namespace TopdownShooter.Pathfinders
{
	[Serializable]
	public class Node : IComparable<Node>
	{
		[Serializable]
		public struct Index
		{
			public int y, x;

			public Index(int x, int y)
			{
				this.x = x;
				this.y = y;
			}

			public static Index operator +(Index left, Index right) => new Index(left.x + right.x, left.y + right.y);
			public static bool operator ==(Index left, Index right) => left.x == right.x && left.y == right.y;
			public static bool operator !=(Index left, Index right) => !(left == right);
		}

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
		public Node Parent;

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

		public Node(Vector2 position, Index index, bool isVisitable, int g = 0, int h = 0, Node parnet = null)
		{
			this.Position = position;
			this.Point = index;
			this.IsVisitable = isVisitable;
			this.Parent = parnet;
			this.Start2CurrentValue = g;
			this.Current2EndValue = h;
		}

		/// <summary>
		/// �ش� ����� �����͸� ������ Ŭ���� ��ȯ�մϴ�.
		/// </summary>
		public Node GetClone()
		{
			return new Node(Position, Point, IsVisitable, Start2CurrentValue, Current2EndValue, Parent);
		}

		public int CompareTo(Node other)
		{
			if (other == null)
				return 1;

			if (Weight == other.Weight)
				return 0;

			if (Weight > other.Weight)
				return 1;

			return -1;
		}
	}
}
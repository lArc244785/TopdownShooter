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

		public Vector2 position { private set; get; }
		public Index index { private set; get; }
		public bool isVisitable { private set; get;}
		public Node parent;


		public int g;
		public int h;
		public int f => g + h;

		public Node(Vector2 position, Index index, bool isVisitable,int g = 0, int h = 0, Node parnet = null)
		{
			this.position = position;
			this.index = index;
			this.isVisitable = isVisitable;
			this.parent = parnet;
			this.g = g;
			this.h = h;
		}

		public Node GetClone()
		{
			return new Node(position, index, isVisitable, g,h,parent);
		}

		public int CompareTo(Node other)
		{
			if(other == null)
				return 1;

			if (f == other.f)
				return 0;

			if (f > other.f)
				return 1;

			return -1;
		}
	}
}
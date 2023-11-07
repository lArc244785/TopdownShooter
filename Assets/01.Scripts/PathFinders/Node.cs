using UnityEngine;

namespace TopdownShooter.Pathfinders
{
	class Node
	{
		public Vector2 position { private set; get; }
		public Vector2Int index { private set; get; }
		public bool isVisitable { private set; get;}
		public Node parent;

		public Node(Vector2 position, Vector2Int index, bool isVisitable, Node parnet = null)
		{
			this.position = position;
			this.index = index;
			this.isVisitable = isVisitable;
			this.parent = parnet;
		}

		public Node GetClone()
		{
			return new Node(position, index, isVisitable, parent);
		} 
	}
}
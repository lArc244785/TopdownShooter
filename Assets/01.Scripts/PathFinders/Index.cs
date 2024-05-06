using System;
using UnityEngine;

namespace TopdownShooter.Pathfinders
{
	[Serializable]
	public struct Index
	{
		#region Constructor
		public Index(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
		#endregion

		#region Field
		public int y, x;
		#endregion

		#region Method
		public static Index operator +(Index left, Index right) => new Index(left.x + right.x, left.y + right.y);
		public static bool operator ==(Index left, Index right) => left.x == right.x && left.y == right.y;
		public static bool operator !=(Index left, Index right) => !(left == right);
		#endregion
	}
}

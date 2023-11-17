using System;

namespace TopdownShooter
{
	[Serializable]
	public struct Timer
	{
		public float endTime;
		public float currentTime;

		public float progress
		{
			get
			{
				if (currentTime == 0.0f)
					return 0.0f;

				return currentTime / endTime;
			}
		}
	}
}

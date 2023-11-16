using System;
using UnityEngine;

namespace TopdownShooter.GameTargets
{
	public abstract class GameTarget : MonoBehaviour
	{
		public abstract bool isComplete { get; }
		public abstract string ToStringProgress { get; }
		public event Action onUpdateProgress;
		public event Action onCompelete;
		public virtual void UpdateProgress()
		{
			onUpdateProgress?.Invoke();
		}

		public virtual void Compelete()
		{
			onCompelete?.Invoke();
		}
	}

}

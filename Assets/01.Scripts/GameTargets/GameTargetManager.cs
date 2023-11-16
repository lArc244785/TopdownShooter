using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TopdownShooter.GameTargets
{
	public class GameTargetManager : MonoBehaviour
	{
		public GameTarget[] targets { private set; get; }
		private int _completeCount;
		
		public event Action onAllTargetComplete;

		private void Awake()
		{
			targets = GetComponentsInChildren<GameTarget>();

			foreach (GameTarget target in targets)
			{
				target.onCompelete += CompeleteTarget;
			}

			onAllTargetComplete += () => Debug.Log("AllTargetComplete");
		}

		private void CompeleteTarget()
		{
			_completeCount++;
			if (_completeCount == targets.Length)
				onAllTargetComplete?.Invoke();
		}
	}
}

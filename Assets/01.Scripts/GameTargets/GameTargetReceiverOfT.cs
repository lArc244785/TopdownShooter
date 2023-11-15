using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TopdownShooter.GameTargets
{
	public abstract class GameTargetReceiverOfT<T> :MonoBehaviour
		where T : GameTarget
	{
		private T _gameTarget;

		public void Init(T gameTarget)
		{
			_gameTarget = gameTarget;
		}

		public void Receve()
		{
			_gameTarget.UpdateProgress();
		}
	}
}

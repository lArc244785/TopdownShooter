using System.Collections.Generic;
using TopdownShooter.Characters;
using Unity.VisualScripting;
using UnityEngine;
namespace TopdownShooter.GameTargets
{
	public class GameTargetEnemyKill : GameTarget
	{
		[SerializeField] private List<EnemyController> m_enemyList;

		[SerializeField] private int _targetKill;
		private int _currentKill = 0;

		public override string progressToString => $"{_currentKill} / {_targetKill}";

		private void Start()
		{
			foreach(var enemy in m_enemyList) 
			{
				var gameTarget = enemy.AddComponent<GameTargetReceiverEnmyKill>();
				gameTarget.Init(this);
			}
		}

		public override void UpdateProgress()
		{
			_currentKill++;

			base.UpdateProgress();
			if (_currentKill == _targetKill)
				Compelete();
		}

		public override void Compelete()
		{
			base.Compelete();
			Debug.Log("All Kill Enemy!!!!");
		}
	}
}

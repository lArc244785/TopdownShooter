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

		public override string ToStringProgress => $"Enemy Kill {_currentKill} / {_targetKill}";

		public override bool isComplete => _isCompelte;

		public override bool isDrawUiTarget => true;

		private bool _isCompelte;

		private void Awake()
		{
			EnemyController[] enemys = GetComponentsInChildren<EnemyController>();

			if (enemys != null)
			{
				m_enemyList.Clear();
				foreach (var enemy in enemys)
				{
					m_enemyList.Add(enemy);
				}
				_targetKill = m_enemyList.Count;
			}
		}

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
			_isCompelte = true;
		}
	}
}

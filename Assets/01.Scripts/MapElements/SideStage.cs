using System;
using System.Collections.Generic;
using TopdownShooter.Characters;
using TopdownShooter.Interactions;
using UnityEngine;

namespace TopdownShooter.GameElements
{
	public class SideStage : MonoBehaviour
	{
		[SerializeField] private bool isStartBarrideOn;

		private ISwitch[] _barricades;
		private BoxCollider2D _spawnBoundery;

		private EnemyController[] _enemys;

		private Vector2 _spawnLeftDownPoint;
		private Vector2 _spawnRightTopPoint;

		private int _enemyKillCount;

		private bool _isStageStart = false;

		public event Action onStageClear;


		private void Awake()
		{
			_barricades = GetComponentsInChildren<Barricade>();
			var boundery = GetComponent<BoxCollider2D>();

			Vector2 origin = (Vector2)transform.position + boundery.offset;

			_spawnLeftDownPoint = origin + Vector2.left * boundery.size.x * 0.5f + Vector2.down * boundery.size.y * 0.5f;
			_spawnRightTopPoint = origin + Vector2.right * boundery.size.x * 0.5f + Vector2.up * boundery.size.y * 0.5f;

			_enemys = GetComponentsInChildren<EnemyController>();
			foreach (var enemy in _enemys)
			{
				enemy.onDead += () =>
				{
					_enemyKillCount++;
					ChackStageClear();
				};
				enemy.gameObject.SetActive(false);
			}

			foreach (var door in _barricades)
			{
				if (isStartBarrideOn)
					door.SwitchOn();
				else
					door.SwitchOff();
			}

		}


		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (_isStageStart)
				return;

			if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerTrigger"))
			{
				StageStart();
			}
		}


		private void StageStart()
		{
			foreach (var door in _barricades)
				door.SwitchOn();

			foreach(var enemy in _enemys)
			{
				enemy.gameObject.SetActive(true);
			}
			_isStageStart = true;
		}

		public void ChackStageClear()
		{
			if(_enemyKillCount >= _enemys.Length)
			{
				foreach (var door in _barricades)
					door.SwitchOff();
				onStageClear?.Invoke();
			}
		}
	}
}

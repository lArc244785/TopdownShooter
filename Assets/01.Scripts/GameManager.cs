using System;
using System.Collections;
using System.Collections.Generic;
using TopdownShooter.Characters;
using TopdownShooter.GameTargets;
using TopdownShooter.UIs;
using UnityEngine;
using CharacterController = TopdownShooter.Characters.CharacterController;

namespace TopdownShooter
{
	public class GameManager : MonoBehaviour
	{
		[SerializeField] private InputController _playerInput;
		[SerializeField] private CharacterController _player;
		[SerializeField] private GameResultPopUp _resultPopUp;
		[SerializeField] private GameTargetManager _gameTargetManger;
		[SerializeField] private string _nextStageName;
		[SerializeField] private float _bestTime;

		private int _hitCount;

		private const int MAXSCORE = 100;

		public event Action OnClear;
		public event Action OnOver;

		// Start is called before the first frame update
		void Start()
		{
			_player.onDead += () => GameResult(false, 0);
			_gameTargetManger.onAllTargetComplete += () => GameResult(true, CalculationScore());
		}

		private void GameResult(bool isGameClear, int score)
		{
			_playerInput.isInputable = false;
			_player.Stop();
			_resultPopUp.PopUpResult(isGameClear, score, _nextStageName);

			if(isGameClear == true)
			{
				OnClear?.Invoke();
			}
			else
			{
				OnOver?.Invoke();
			}
		}
		
		private int CalculationScore()
		{
			var score = MAXSCORE;

			score -= _hitCount;

			return score;
		}
	}
}


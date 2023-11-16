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

		// Start is called before the first frame update
		void Start()
		{
			_player.onDead += () => GameResult(false, -9999);
			_gameTargetManger.onAllTargetComplete += () => GameResult(true, 9999);
		}

		private void GameResult(bool isGameClear, int score)
		{
			_playerInput.isInputable = false;
			_player.Stop();
			_resultPopUp.PopUpResult(isGameClear, score);
		}
		
	}
}


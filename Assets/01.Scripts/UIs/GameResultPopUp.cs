using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TopdownShooter.UIs
{
	public class GameResultPopUp : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _textGameResult;
		[SerializeField] private TextMeshProUGUI _textScore;
		[SerializeField] private Button _buttonContinue;
		[SerializeField] private Button _buttonExit;

		private void Awake()
		{
			_buttonExit.onClick.AddListener(() => EditorApplication.isPlaying = false) ;
			gameObject.SetActive(false);
		}

		public void PopUpResult(bool isGameClear, int scroe, string nextStage)
		{
			_buttonContinue.onClick.AddListener(() => SceneManager.LoadScene(nextStage));
			gameObject.SetActive(true);
			string gameResult = isGameClear ? "game clear" : "game over";
			_textGameResult.text = gameResult;
			_textScore.text = scroe.ToString();
		}
	}

}

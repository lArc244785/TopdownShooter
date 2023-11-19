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
		[SerializeField] private TextMeshProUGUI _textGameNextOrContinue;
		[SerializeField] private TextMeshProUGUI _textScore;
		[SerializeField] private Button _buttonNextOrContinue;
		[SerializeField] private Button _buttonExit;

		private string _stageName;

		private void Awake()
		{
			_stageName = SceneManager.GetActiveScene().name;

#if UNITY_EDITOR
			_buttonExit.onClick.AddListener(() => EditorApplication.isPlaying = false) ;
#else
			_buttonExit.onClick.AddListener(() => Application.Quit());
#endif
			gameObject.SetActive(false);
		}

		public void PopUpResult(bool isGameClear, int scroe, string nextStage)
		{
			if(isGameClear)
			{
				_buttonNextOrContinue.onClick.AddListener(() => SceneManager.LoadScene(nextStage));
				_textGameNextOrContinue.text = "Next Stage";
				_textGameResult.text = "game clear";
			}
			else
			{
				_buttonNextOrContinue.onClick.AddListener(() => SceneManager.LoadScene(_stageName));
				_textGameNextOrContinue.text = "Continue";
				_textGameResult.text = "game over";
			}

			gameObject.SetActive(true);
			_textScore.text = scroe.ToString();
		}
	}

}

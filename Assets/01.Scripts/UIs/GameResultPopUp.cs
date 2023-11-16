using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TopdownShooter.UIs
{
	public class GameResultPopUp : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _textScore;
		[SerializeField] private Button _buttonContinue;
		[SerializeField] private Button _buttonExit;

		private void Awake()
		{
			_buttonContinue.onClick.AddListener(() => SceneManager.LoadScene(0));
			_buttonExit.onClick.AddListener(() => EditorApplication.isPlaying = false) ;
			gameObject.SetActive(false);
		}

		public void PopUpResult(bool isGameClear, int scroe)
		{
			gameObject.SetActive(true);
			string gameResult = isGameClear ? "game clear" : "game over";
			_textScore.text = scroe.ToString();
		}
	}

}

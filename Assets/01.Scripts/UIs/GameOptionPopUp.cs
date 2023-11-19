using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace TopdownShooter.UIs
{
	public class GameOptionPopUp : MonoBehaviour
	{
		[SerializeField] private Button _buttonYes;
		[SerializeField] private Button _buttonNo;

		private bool _isVisable = false;
		public bool IsVisable => _isVisable;

		private void Awake()
		{
#if UNITY_EDITOR
			_buttonYes.onClick.AddListener(() => EditorApplication.isPlaying = false);
#else
			_buttonYes.onClick.AddListener(() => Application.Quit());
#endif

			_buttonNo.onClick.AddListener(() =>
			{
				OptionPopUp();
			});

			gameObject.SetActive(false);
		}

		public void OptionPopUp()
		{
			if (!_isVisable)
			{
				Time.timeScale = 0.0f;
				gameObject.SetActive(true);
			}
			else
			{
				Time.timeScale = 1.0f;
				gameObject.SetActive(false);
			}
			_isVisable = !_isVisable;
		}
	}
}


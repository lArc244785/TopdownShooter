using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopdownShooter.GameTargets;
using UnityEngine;
using TMPro;

namespace TopdownShooter.UIs
{
	public class GameTargetList : MonoBehaviour
	{
		[SerializeField] private GameTargetManager _manager;
		[SerializeField] private TextMeshProUGUI _textGameTarget;
		[SerializeField] private float _elemntSizeY;

		private void Start()
		{
			var rectTransfrom = GetComponent<RectTransform>();
			rectTransfrom.sizeDelta = new Vector2(rectTransfrom.rect.width, _elemntSizeY * _manager.targets.Length);

			foreach(var target in _manager.targets)
			{
				var textGameTarget = Instantiate(_textGameTarget, rectTransfrom);
				textGameTarget.text = target.ToStringProgress;
				target.onUpdateProgress += () => textGameTarget.text = target.ToStringProgress;
				target.onCompelete += () => textGameTarget.fontStyle = FontStyles.Strikethrough | FontStyles.Bold;
			}
		}
	}
}

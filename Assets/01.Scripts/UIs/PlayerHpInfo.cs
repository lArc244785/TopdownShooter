using TMPro;
using TopdownShooter.Characters;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using CharacterController = TopdownShooter.Characters.CharacterController;

namespace TopdownShooter.UIs
{
	public class PlayerHpInfo : MonoBehaviour
	{
		[SerializeField] private CharacterController _controller;
		[SerializeField] private UnityEngine.UI.Slider _slider;
		[SerializeField] private TextMeshProUGUI _textHp;

		private void Start()
		{
			var hp = (IHP)_controller;

			_slider.maxValue = hp.maxHp;
			_slider.minValue = hp.minHp;
			_slider.value = hp.hpValue;
			_textHp.text = $"{_slider.value} / {_slider.maxValue}";

			hp.onHpChanged += (value) =>
			{
				_slider.value = value;
				_textHp.text = $"{_slider.value} / {_slider.maxValue}";
			};
		}
	}
}

using TMPro;
using TopdownShooter.Characters;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using CharacterController = TopdownShooter.Characters.CharacterController;
using Image = UnityEngine.UI.Image;

namespace TopdownShooter.UIs
{
	public class PlayerHpInfo : MonoBehaviour
	{
		[SerializeField] private CharacterController _controller;
		[SerializeField] private Image _imgHpBarValue;

		private float _minValue;
		private float _maxValue;
		private float _value;

		private float GetRatio()
		{
			if (_value <= _minValue)
				return 0.0f;

			var ratio = _value / _maxValue;

			return ratio;
		}

		private void Start()
		{
			var hp = (IHP)_controller;

			_minValue = hp.minHp;
			_maxValue = hp.maxHp;
			_value = hp.hpValue;
			_imgHpBarValue.fillAmount = GetRatio();

			hp.onHpChanged += (value) =>
			{
				_value = value;
				_imgHpBarValue.fillAmount = GetRatio();
			};
		}
	}
}

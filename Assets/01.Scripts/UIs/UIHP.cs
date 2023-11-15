using TMPro;
using TopdownShooter.Characters;
using UnityEngine;

namespace TopdownShooter.UIs
{
	public class UIHP : MonoBehaviour
	{
		[SerializeField] private IHP _plyaer;
		[SerializeField] private TextMeshProUGUI _text;

		private void Start()
		{
			_plyaer.onHpChanged += (value) => _text.text = $"{value} / {_plyaer.maxHp}";
		}
	}
}

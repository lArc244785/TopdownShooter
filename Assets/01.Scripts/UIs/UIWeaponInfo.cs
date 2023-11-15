using System.Collections.Generic;
using TMPro;
using TopdownShooter.Weapons;
using UnityEngine;

namespace TopdownShooter.UIs
{
	public class UIWeaponInfo : MonoBehaviour
	{
		[SerializeField] private WeaponController _weaponController;
		[SerializeField] private List<Sprite> _sprites = new List<Sprite>();
		[SerializeField] private TextMeshProUGUI _textWeaponName;
		[SerializeField] private TextMeshProUGUI _textWeaponAmmo;

		private void Start()
		{
			
		}

	}
}

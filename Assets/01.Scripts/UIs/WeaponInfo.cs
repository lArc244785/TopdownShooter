using System.Collections.Generic;
using TMPro;
using TopdownShooter.Weapons;
using UnityEngine;
using UnityEngine.UI;

namespace TopdownShooter.UIs
{
	public class WeaponInfo : MonoBehaviour
	{
		[SerializeField] private WeaponController _weaponController;
		[SerializeField] private Image _imageWeapon;
		[SerializeField] private TextMeshProUGUI _textWeaponName;
		[SerializeField] private TextMeshProUGUI _textWeaponAmmo;

		private WeaponBase _currentWeapon;
		private int _magazineAmmoValue;
		private int _ammoValue;

		private void Awake()
		{
			_weaponController.onChangeWeapon += (oldWeapon, newWeapon) =>
			{
				_imageWeapon.sprite = newWeapon.renderer.sprite;
				_textWeaponName.text = newWeapon.weaponName;
				_magazineAmmoValue = newWeapon.magazineAmmoValue;
				_ammoValue = newWeapon.ammoValue;
				UpdateWeaponAmmo();

				if(oldWeapon != null)
				{
					oldWeapon.onChangeAmmo -= SetAmmo;
					oldWeapon.onChangeMagazineAmmo -= SetMagazineAmmo;
				}

				newWeapon.onChangeAmmo += SetAmmo;
				newWeapon.onChangeMagazineAmmo += SetMagazineAmmo;
			};
		}

		private void UpdateWeaponAmmo()
		{
			_textWeaponAmmo.text = $"{_magazineAmmoValue} / {_ammoValue}";
		}

		private void SetAmmo(int amount)
		{
			_ammoValue = amount;
			UpdateWeaponAmmo();
		}

		private void SetMagazineAmmo(int amount) 
		{
			_magazineAmmoValue = amount;
			UpdateWeaponAmmo();
		}

	}
}

using System;
using UnityEngine;

namespace TopdownShooter.Weapons
{
	public enum WeaponType
	{
		None,
		Pistorl,
		Knife,
	}


	public interface IWeapon
	{
		public string name { get; }
		public float damage { get; }
		public float attackCoolTime { get; }
		public float reloadCoolTime { get; }
		public WeaponType weaponType { get; }

		public bool CanAttack { get; }

		public void Attack(Vector2 attackDiraction);
		public void ReloadEnter();
		public void ReloadExit();

		public event Action onAttack;
		public event Action onReloadEnter;
		public event Action onReloadExit;
	}
}

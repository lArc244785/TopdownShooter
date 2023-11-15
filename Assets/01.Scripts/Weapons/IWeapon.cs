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

		public float attackTime { get; }
		public float reloadTime { get; }
		public bool canReload { get;}

		public WeaponType weaponType { get; }

		public bool Attack(Vector2 attackDiraction);
		public void Reload();

		public event Action onAttack;
		public event Action onReload;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopdownShooter.Weapons
{
	public enum AmmoType 
	{
		None,
		Ammo_9mm,
	}


	public interface IAmmo
	{
		public AmmoType ammoType { get; }
		public int ammoValue { set; get; }
		public int maxAmmo { get; }
		public int minAmmo { get; }

		public void UseAmmo(int amount);
		public void AddAmmo(int amount);

		public event Action<int> onChangeAmmo;
		public event Action<int> onUseAmmo;
		public event Action<int> onAddAmmo;
		public event Action onMinAmmo;
		public event Action onMaxAmmo;
	}
}

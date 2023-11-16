using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopdownShooter.Weapons
{

	public interface IMagazine : IAmmo
	{
		public int magazineAmmoValue { set; get; }
		public int maxMagazineAmmo {get;}
		public int minMagazineAmmo {get;}

		public void UseMagazineAmmo(int amount);
		public void AddMagazineAmmo(int amount);

		public event Action<int> onChangeMagazineAmmo;
		public event Action onMinMagazineAmmo;
		public event Action onAddMagazine;
	}
}

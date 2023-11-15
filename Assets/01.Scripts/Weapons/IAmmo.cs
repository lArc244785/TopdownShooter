using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopdownShooter.Weapons
{

	public interface IAmmo
	{
		//소지 탄약
		public int ammoValue { set; get; }
		public int maxAmmo { get; }
		public int minAmmo { get; }
		public void UseAmmo(int amount);
		public void AddAmmo(int amount);

		public event Action onMaxAmmo;
		public event Action onMinAmmo;
		public event Action<int> onChangeAmmo;
		public event Action onAddAmmo;
	}
}

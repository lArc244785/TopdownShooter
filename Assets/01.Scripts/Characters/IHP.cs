using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopdownShooter.Characters
{
	public interface IHP
	{
		public bool invincible { get; set; }
		public float hpValue { get; set; }
		public float maxHp { get; }
		public float minHp { get; }

		public void RecoverHp(object subject, float amount);
		public void DeleteHp(object subject, float amount);

		public event Action<float> onHpChanged;
		public event Action<float> onHpRecover;
		public event Action<float> onHpDelete;
		public event Action onHpMax;
		public event Action onHpMin;
	}
}

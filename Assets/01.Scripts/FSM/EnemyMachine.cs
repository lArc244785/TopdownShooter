using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopdownShooter.Characters;

namespace TopdownShooter.FSM
{
	public class EnemyMachine : CharacterMachine
	{
		public EnemyMachine(CharacterController owner) : base(owner)
		{
			Init(CharacterDataShet.GetEnemyData(this));
		}
	}
}

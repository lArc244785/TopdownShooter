using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopdownShooter.Characters;

namespace TopdownShooter.FSM
{
	public class PlayerMachine : CharacterMachine
	{
		public PlayerMachine(CharacterController owner) : base(owner)
		{
			Init(CharacterDataShet.GetPlayerData(this));
		}
	}
}

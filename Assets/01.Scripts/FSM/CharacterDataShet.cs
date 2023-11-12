using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopdownShooter.Characters;

namespace TopdownShooter.FSM
{
	public static class CharacterDataShet
	{
		public static IDictionary<CharacterStateID, IState<CharacterStateID>> GetPlayerData(CharacterMachine machine)
		{
			return new Dictionary<CharacterStateID, IState<CharacterStateID>>() {
				{ CharacterStateID.Idle, new CharacterFsmIdle(machine)},
				{ CharacterStateID.Move, new CharacterFsmMove(machine)},
				{ CharacterStateID.Hurt, new CharacterFsmHurt(machine)},
				{ CharacterStateID.Die, new CharacterFsmDie(machine)}
			};
		}		
		public static IDictionary<CharacterStateID, IState<CharacterStateID>> GetEnemyData(CharacterMachine machine)
		{
			return new Dictionary<CharacterStateID, IState<CharacterStateID>>() {
				{ CharacterStateID.Idle, new CharacterFsmIdle(machine)},
				{ CharacterStateID.Move, new CharacterFsmMove(machine)},
				{ CharacterStateID.Hurt, new CharacterFsmHurt(machine)},
				{ CharacterStateID.Die, new CharacterFsmDie(machine)}
			};
		}

	}
}

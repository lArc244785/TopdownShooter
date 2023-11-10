using System.Collections.Generic;
using TopdownShooter.Characters;

namespace TopdownShooter.FSM
{
	public abstract class CharacterMachine : StateMachine<CharacterStateID>
	{
		public CharacterController owner { get; protected set; }
		public CharacterMachine(CharacterController owner)
		{
			this.owner = owner;
		}
	}
}

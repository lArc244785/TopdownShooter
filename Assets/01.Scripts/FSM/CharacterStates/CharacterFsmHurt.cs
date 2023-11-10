using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopdownShooter.FSM
{
	public class CharacterFsmHurt : CharacterStateBase
	{
		public CharacterFsmHurt(CharacterMachine machine) : base(machine){}

		public override CharacterStateID id => CharacterStateID.Hurt;

		public override bool canExecute => true;

		public override void OnStateEnter()
		{
			base.OnStateEnter();
			animator.Play("Hurt");
		}

		public override CharacterStateID OnStateUpdate()
		{
			var nextID = base.OnStateUpdate();
			if (nextID == CharacterStateID.None)
				nextID =  id;

			return nextID;
		}

	}
}

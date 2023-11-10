using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopdownShooter.FSM
{
	public class CharacterFsmMove : CharacterStateBase
	{
		public CharacterFsmMove(CharacterMachine machine) : base(machine){}

		public override CharacterStateID id => CharacterStateID.Move;

		public override bool canExecute => true;

		public override void OnStateEnter()
		{
			base.OnStateEnter();
			animator.Play("Move");
		}

		public override CharacterStateID OnStateUpdate()
		{
			var nextID = base.OnStateUpdate();
			if (nextID == CharacterStateID.None)
				nextID =  id;

			if (controller.move.sqrMagnitude <= 0.0f)
				nextID = CharacterStateID.Idle;

			return nextID;
		}

	}
}

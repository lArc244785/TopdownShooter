using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TopdownShooter.FSM
{
	public class CharacterFsmIdle : CharacterStateBase
	{
		public CharacterFsmIdle(CharacterMachine machine) : base(machine){}

		public override CharacterStateID id => CharacterStateID.Idle;

		public override bool canExecute => true;

		public override void OnStateEnter()
		{
			base.OnStateEnter();
			animator.Play("Idle");
		}

		public override CharacterStateID OnStateUpdate()
		{
			var nextID = base.OnStateUpdate();
			if (nextID == CharacterStateID.None)
				nextID =  id;

			if (controller.move.sqrMagnitude > 0.0f)
				nextID = CharacterStateID.Move;


			return nextID;
		}


	}
}

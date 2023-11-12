using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopdownShooter.FSM
{
	public class CharacterFsmDie : CharacterStateBase
	{
		public CharacterFsmDie(CharacterMachine machine) : base(machine){}

		public override CharacterStateID id => CharacterStateID.Die;

		public override bool canExecute => true;

		public override void OnStateEnter()
		{
			base.OnStateEnter();
			animator.Play("Die");
		}

		public override CharacterStateID OnStateUpdate()
		{
			var nextID = base.OnStateUpdate();
			if (nextID == CharacterStateID.None)
				nextID =  id;

			if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
				controller.OnDead();

			return nextID;
		}

	}
}

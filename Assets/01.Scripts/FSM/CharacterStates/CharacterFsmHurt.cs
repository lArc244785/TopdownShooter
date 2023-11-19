using UnityEngine;

namespace TopdownShooter.FSM
{
	public class CharacterFsmHurt : CharacterStateBase
	{
		public CharacterFsmHurt(CharacterMachine machine) : base(machine){}

		public override CharacterStateID id => CharacterStateID.Hurt;

		public override bool canExecute => controller.hpValue > controller.minHp;

		public override void OnStateEnter()
		{
			base.OnStateEnter();
			controller.isMoveable = false;
			animator.Play("Hurt");
		}

		public override CharacterStateID OnStateUpdate()
		{
			var nextID = base.OnStateUpdate();
			if (nextID == CharacterStateID.None)
				nextID = id;

			if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hurt") &&
				animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
			{
				nextID = CharacterStateID.Idle;
			}

			return nextID;
		}

		public override void OnStateExit()
		{
			base.OnStateExit();
			controller.isMoveable = true;
	
		}

	}
}

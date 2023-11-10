using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CharacterController = TopdownShooter.Characters.CharacterController;
using UnityEngine;

namespace TopdownShooter.FSM
{
	public enum CharacterStateID
	{
		None,
		Idle,
		Move,
		Hurt,
		Die,
	}

	public abstract class CharacterStateBase : StateBase<CharacterStateID>
	{
		protected CharacterMachine machine { get; private set; }
		protected CharacterController controller { get; private set; }
		protected Transform transform { get; private set; }
		protected Rigidbody2D rigidbody2D { get; private set; }
		protected Animator animator { get; private set; }

		public CharacterStateBase(CharacterMachine machine)
		{
			this.machine = machine;
			controller = machine.owner;
			transform = controller.transform;
			rigidbody2D = controller.rigidbody2D;
			animator = controller.animator;
		}

	}
}

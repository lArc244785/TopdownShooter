using System;


namespace TopdownShooter.FSM
{
	public abstract class StateBase<T> : IState<T> where T : Enum
	{
		public abstract T id { get; }

		public abstract bool canExecute { get; }

		public virtual void OnStateEnter() { }

		public virtual T OnStateUpdate() { return id; }
		public virtual void OnStaetFixedUpdate() { }

		public virtual void OnStateExit() { }

	}
}

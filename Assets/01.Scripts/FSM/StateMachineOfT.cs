
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace TopdownShooter.FSM
{
	public abstract class StateMachine<T> where T : Enum
	{
		public T currentID { get; private set; }

		private Dictionary<T, IState<T>> _states;

		public virtual void Init(IDictionary<T, IState<T>> setting)
		{
			_states = new Dictionary<T, IState<T>>(setting);
			currentID = _states.First().Key;
			_states[currentID].OnStateEnter();
		}

		public bool ChangeState(T newState)
		{
			if (Comparer<T>.Default.Compare(currentID, newState) == 0)
				return false;

			if (!_states[newState].canExecute)
				return false;

			_states[currentID].OnStateExit();
			currentID = newState;
			_states[currentID].OnStateEnter();
			return true;
		}

		public void OnStateUpdate()
		{
			var nextID = _states[currentID].OnStateUpdate();
			ChangeState(nextID);
		}

	}
}


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace TopdownShooter.FSM
{
	public abstract class StateMachine<T> where T : Enum
	{
		private T _currentID;
		public T currentID => _currentID;

		private Dictionary<T, IState<T>> _states;

		public virtual void Init(IDictionary<T, IState<T>> setting)
		{
			_states = new Dictionary<T, IState<T>>(setting);
			_currentID = _states.First().Key;
			_states[_currentID].OnStateEnter();
		}

		public bool ChangeState(T newState)
		{
			if (Comparer<T>.Default.Compare(_currentID, newState) == 0)
				return false;

			if (!_states[newState].canExecute)
				return false;

			_states[_currentID].OnStateExit();
			_currentID = newState;
			_states[_currentID].OnStateEnter();
			return true;
		}

		public void OnStateUpdate()
		{
			var nextID = _states[_currentID].OnStateUpdate();
			ChangeState(nextID);
		}

	}
}

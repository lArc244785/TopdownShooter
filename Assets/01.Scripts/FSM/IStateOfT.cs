using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopdownShooter.FSM
{
	public interface IState<T> where T : Enum
	{
		public T id { get; }
		public bool canExecute { get; }
		public void OnStateEnter();
		public T OnStateUpdate();
		public void OnStaetFixedUpdate();
		public void OnStateExit();
	}


}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopdownShooter.Interactions
{
	public interface ISwitch
	{
		public void SwitchOn();
		public void SwitchOff();

		public event Action onSwitchOn;
		public event Action onSwitchOff;
	}
}


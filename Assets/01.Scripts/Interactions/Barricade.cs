using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopdownShooter.Interactions
{
	public class Barricade : MonoBehaviour, ISwitch
	{
		public event Action onSwitchOn;
		public event Action onSwitchOff;

		public void SwitchOff()
		{
			onSwitchOff?.Invoke();
			gameObject.SetActive(false);
		}

		public void SwitchOn()
		{
			onSwitchOn?.Invoke();
			gameObject.SetActive(true);
		}
	}
}

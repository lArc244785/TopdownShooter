using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopdownShooter.Items
{
	public enum ItemType 
	{
		None,
		RecoverHp,
		AddAmmo,
	}


	public abstract class ItemBase : MonoBehaviour
	{
		public abstract ItemType Type { get; }

		public virtual bool Effect(Collider2D collider)
		{
			onEffect?.Invoke();
			gameObject.SetActive(false);
			return true;
		}
		public event Action onEffect;
	}

}

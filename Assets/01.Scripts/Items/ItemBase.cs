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
		[SerializeField] private AudioClip _effectSound;
		public abstract ItemType Type { get; }

		public virtual bool Effect(Collider2D collider)
		{
			collider.GetComponent<CharacterSound>().Play(_effectSound);
			gameObject.SetActive(false);
			return true;
		}
	}

}

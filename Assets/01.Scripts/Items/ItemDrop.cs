using UnityEngine;
using TopdownShooter.Characters;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using CharacterController = TopdownShooter.Characters.CharacterController;

namespace TopdownShooter.Items
{
	public class ItemDrop : MonoBehaviour
	{
		[SerializeField, Range(0.0f, 1.0f)] private float _dropRatio;
		[SerializeField] private List<ItemBase> _items;
		public event Action onDrop;

		private void Start()
		{
			var controller = GetComponent<CharacterController>();
			controller.onDead += Drop;
		}

		private void Drop()
		{
			if(TryItemDrop(out var dropItem))
			{
				var item = Instantiate(dropItem, transform.position, Quaternion.identity, null);
				onDrop?.Invoke();
			}
		}

		private bool TryItemDrop(out ItemBase dropItem)
		{
			dropItem = null;
			if(Random.Range(0.0f, 1.0f) <= _dropRatio)
			{
				var randomIndex = (int)Random.Range(0, _items.Count);
				dropItem = _items[randomIndex];
				return true;
			}
			return false;
		}
	}
}

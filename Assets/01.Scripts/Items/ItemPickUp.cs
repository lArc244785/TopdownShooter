using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TopdownShooter.Items
{
	public class ItemPickUp : MonoBehaviour
	{
		private LayerMask _pickUpLayerMask;
		private Collider2D _collider;

		private void Awake()
		{
			_pickUpLayerMask = LayerMask.GetMask("PickUpItem");
			_collider = GetComponent<Collider2D>();
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if((_pickUpLayerMask & (1 << collision.gameObject.layer)) > 0)
			{
				if(collision.TryGetComponent<ItemBase>(out var item))
				{
					PickUp(item);
				}
			}
		}


		private void PickUp(ItemBase item)
		{
			item.Effect(_collider);
		}
	}
}

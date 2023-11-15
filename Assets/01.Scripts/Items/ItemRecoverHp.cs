using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopdownShooter.Characters;
using UnityEngine;

namespace TopdownShooter.Items
{
	public class ItemRecoverHp : ItemBase
	{
		public override ItemType Type => ItemType.RecoverHp;
		[SerializeField] private float _recoverHp;

		public override bool Effect(Collider2D collider)
		{
			if(collider.TryGetComponent<IHP>(out var hp))
			{
				hp.RecoverHp(collider.gameObject,_recoverHp);
				return base.Effect(collider);
			}
			return false;
		}
	}
}

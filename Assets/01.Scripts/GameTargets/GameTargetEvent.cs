using System.Collections.Generic;
using TopdownShooter.Characters;
using Unity.VisualScripting;
using UnityEngine;
namespace TopdownShooter.GameTargets
{
	public class GameTargetEvent : GameTarget
	{
		[SerializeField] private string _targetDescirption;
		public override bool isComplete => _isComplete;
		private bool _isComplete = false;

		public override string ToStringProgress => _targetDescirption;

		public override bool isDrawUiTarget => true;

		[SerializeField] private LayerMask _enterTriggerLayerMask;

		public override void Compelete()
		{
			base.Compelete();
			_isComplete = true;
		}

		public override void UpdateProgress()
		{
			base.UpdateProgress();
			if (GameTargetManager.instance.completeCount + 1 == GameTargetManager.instance.allCompeleteCount)
				Compelete();
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if((_enterTriggerLayerMask & 1 << collision.gameObject.layer) > 0)
			{
				UpdateProgress();
			}
		}

		private void Start()
		{
			MiniMap.instance?.ReginsterObject(transform, MiniMapIconType.Target);
		}

	}
}

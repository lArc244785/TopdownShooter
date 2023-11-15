using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopdownShooter.Characters;

namespace TopdownShooter.GameTargets
{
	public class GameTargetReceiverEnmyKill : GameTargetReceiverOfT<GameTargetEnemyKill>
	{
		private void Awake()
		{
			if (TryGetComponent<CharacterController>(out var controller))
				controller.onDead += Receve;
		}
	}
}

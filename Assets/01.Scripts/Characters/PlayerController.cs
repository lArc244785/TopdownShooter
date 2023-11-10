using System.Collections;
using System.Collections.Generic;
using TopdownShooter.FSM;
using UnityEngine;

namespace TopdownShooter.Characters
{
    public class PlayerController : CharacterController
    {
		protected override void Awake()
		{
			base.Awake();
			machine = new PlayerMachine(this);
			machine.Init(CharacterDataShet.GetPlayerData(machine));
		}
	}
}
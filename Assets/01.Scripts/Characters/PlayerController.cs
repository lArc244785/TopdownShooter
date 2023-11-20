using System.Collections;
using System.Collections.Generic;
using TopdownShooter.FSM;
using UnityEngine;

namespace TopdownShooter.Characters
{
    public class PlayerController : CharacterController
    {
		[SerializeField] private Timer _invincibleTime;
		protected override void Awake()
		{
			base.Awake();
			machine = new PlayerMachine(this);
			onHpDelete += (value) => StartCoroutine(C_Invincible());
		}

		protected override void Start()
		{
			base.Start();
			MiniMap.instance?.ReginsterObject(transform, MiniMapIconType.Player);
		}


		private IEnumerator C_Invincible()
		{
			_invincibleTime.currentTime = 0.0f;
			invincible = true;

			while(_invincibleTime.currentTime <= _invincibleTime.endTime)
			{
				_invincibleTime.currentTime += Time.deltaTime;
				yield return null;
			}

			invincible = false;
		}
	}
}
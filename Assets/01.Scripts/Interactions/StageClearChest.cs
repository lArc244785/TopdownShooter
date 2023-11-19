using TopdownShooter.GameElements;
using UnityEngine;
using TopdownShooter.Characters;
using TopdownShooter.GameTargets;

namespace TopdownShooter.Interactions
{
	public class StageClearChest : MonoBehaviour, IIteraction
	{
		private bool _canInteraction = true;
		public bool canInteraction => _canInteraction;
		[SerializeField] private Animator _animator;
		[SerializeField] private GameTargetEvent _event;

		[SerializeField] private GameObject _interactionCanvas;

		private void Awake()
		{
			_interactionCanvas.SetActive(false);
		}

		public void Interaction()
		{
			_interactionCanvas.SetActive(false);
			_animator.Play("Open");
		}

		public void OnTriggerEnter2D(Collider2D collision)
		{
			if(collision.gameObject.layer == LayerMask.NameToLayer("PlayerTrigger"))
			{
				collision.GetComponent<InputController>().SetInteraction(this);
				_interactionCanvas.SetActive(true);
			}
		}

		public void OnTriggerExit2D(Collider2D collision)
		{
			if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerTrigger"))
			{
				collision.GetComponent<InputController>().SetInteraction(null);
				_interactionCanvas.SetActive(false);
			}
		}

		private void Update()
		{
			if (!_canInteraction)
				return;

			if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Open") &&
				_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
			{
				_event.Compelete();
				_canInteraction = false;
			}
		}
	}
}

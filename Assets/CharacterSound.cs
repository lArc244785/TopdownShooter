using System.Collections;
using System.Collections.Generic;
using TopdownShooter.Characters;
using UnityEngine;

public class CharacterSound : MonoBehaviour
{
	private IHP _hp;
	private AudioSource _audioSource;
	[SerializeField] private AudioClip _hpDown;
	[SerializeField] private AudioClip _die;
	bool _isDiePlay = false;

	private void Awake()
	{
		_audioSource = GameObject.Find("SFX").GetComponent<AudioSource>();
		_hp = GetComponent<IHP>();
	}

	void Start()
    {
		_hp.onHpDelete += (value) => Play(_hpDown);
		_hp.onHpMin += () =>
		{
			if (_isDiePlay == false)
			{
				Play(_die);
				_isDiePlay = true;
			}
		};
	}

	public void Play(AudioClip clip)
	{
		if (clip == null)
			return;

		_audioSource.PlayOneShot(clip);
	}
}

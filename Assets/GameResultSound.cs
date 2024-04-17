using System.Collections;
using System.Collections.Generic;
using TopdownShooter;
using UnityEngine;

public class GameResultSound : MonoBehaviour
{
	[SerializeField] private AudioSource _source;
	[SerializeField] private AudioSource _bgm;

	[SerializeField] private AudioClip _clear;
	[SerializeField] private AudioClip _over;

	private void Start()
	{
		GameManager manager = GetComponent<GameManager>();
		manager.OnClear += () => _source.PlayOneShot(_clear);
		manager.OnOver += () => _source.PlayOneShot(_over);

		manager.OnClear += () => _bgm.Stop();
		manager.OnOver += () => _bgm.Stop();
	}
}

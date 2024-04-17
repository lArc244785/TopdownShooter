using System.Collections;
using System.Collections.Generic;
using TopdownShooter.Weapons;
using UnityEngine;

public class WeaponSound : MonoBehaviour
{
	[SerializeField] private AudioSource _audioSource;
    [SerializeField] private WeaponBase weapon;
    [SerializeField] private AudioClip[] _fires;
    [SerializeField] private AudioClip[] _reloadStart;
    [SerializeField] private AudioClip[] _reloadFinsh;

	// Start is called before the first frame update
	void Start()
    {
        int random = Random.Range(0, _fires.Length);

        weapon.onAttack += () => Play(_fires);
        weapon.onReloadStart += () => Play(_reloadStart);
        weapon.onReloadFinsh += () => Play(_reloadFinsh);
    }

    private void Play(AudioClip[] clips)
	{
        if (clips.Length == 0)
            return;

        int random = Random.Range(0, clips.Length);
        _audioSource.PlayOneShot(clips[random]);
    }


}

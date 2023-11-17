using Cinemachine;
using TopdownShooter;
using UnityEngine;

namespace TopdownShooter
{

	public enum CamreaEffectType
	{
		None,
		Shake,
	}

	public class CameraEffect : MonoBehaviour
	{
		public static CameraEffect instance => _instance;
		private static CameraEffect _instance;

		private CinemachineBrain _brain;

		private CamreaEffectType _type;

		private float _amplutubeGain;
		private float _frequencyGain;
		private Timer _shakeTimer;
		private CinemachineBasicMultiChannelPerlin _multiChannelPerlin;

		private void Awake()
		{
			if (instance != null)
				throw new System.Exception($"CameraEffect instance not null");

			_brain = GetComponent<CinemachineBrain>();
			_shakeTimer = new();
			_instance = this;
		}

		private void Update()
		{
			switch (_type)
			{
				case CamreaEffectType.Shake:
					Shake();
					break;
			}
		}


		public void CameraShake(float amplitubeGain, float frequencyGain, float duration)
		{
			var currentVirtualCam = _brain.ActiveVirtualCamera as CinemachineVirtualCamera;
			_multiChannelPerlin = currentVirtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
			_type = CamreaEffectType.Shake;
			_amplutubeGain = amplitubeGain;
			_frequencyGain = frequencyGain;
			_shakeTimer.currentTime = 0;
			_shakeTimer.endTime = duration;
		}

		private void Shake()
		{
			_shakeTimer.currentTime += Time.deltaTime;

			_multiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(_amplutubeGain, 0.0f, _shakeTimer.progress);
			_multiChannelPerlin.m_FrequencyGain = _frequencyGain;

			if (_shakeTimer.currentTime >= _shakeTimer.endTime)
				_type = CamreaEffectType.None;
		}

	}
}
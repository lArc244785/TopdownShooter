using System.Collections.Generic;
using UnityEngine;

namespace BumblingKitchen
{
	public class IngameFps : MonoBehaviour
	{
		float deltaTime = 0.0f;
		int minFps = 300;
		float avgFps;
		Queue<int> fpsQueue = new Queue<int>(60);

		Rect rect;

		private void Start()
		{
			rect = new Rect(Screen.width * 0.05f, Screen.height * 0.3f, 100, 100);
		}

		void Update()
		{
			deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
		}

		void OnGUI()
		{
			int fps = Mathf.RoundToInt(1.0f / deltaTime);

			if (fps < 0)
				return;

			if (fpsQueue.Count == 60)
				fpsQueue.Dequeue();

			fpsQueue.Enqueue(fps);
			if (fps < minFps)
			{
				minFps = fps;
			}
			avgFps = GetAvaFps();

			string text = $"FPS: {fps}\nMin{minFps}\nAvg{avgFps}";

			GUIStyle style = new GUIStyle();
			style.fontSize = 50;
			style.normal.textColor = Color.cyan;

			GUI.Label(rect, text, style);
		}

		float GetAvaFps()
		{
			float totalFps = 0.0f;
			foreach (var fps in fpsQueue)
			{
				totalFps += fps;
			}
			return totalFps / (float)fpsQueue.Count;
		}
	}
}


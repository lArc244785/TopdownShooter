using System.Collections;
using System.Collections.Generic;
using TopdownShooter.Pathfinders;
using UnityEngine;

public class TestPathFinder : MonoBehaviour
{
	[SerializeField] private PathFinder _pathFinder;
	[SerializeField] private Transform _startPos;
	[SerializeField] private Transform _endPos;

	private Vector2[] _paths;


	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Q))
			_pathFinder.TryGetPath(_startPos.position, _endPos.position, out _paths);

	}


	private void OnDrawGizmos()
	{
		if (_paths == null)
			return;

		for (int i = 1; i < _paths.Length; i++)
		{
			Gizmos.DrawLine(_paths[i - 1], _paths[i]);
		}
	}

}

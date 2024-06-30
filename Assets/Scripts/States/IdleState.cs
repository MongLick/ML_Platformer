using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class IdleState<T> : IState where T : Monster
{
	[SerializeField] T owner;
	[SerializeField] float findRange = 5f;

	private Transform playerTransform;

	public void Enter()
	{
		playerTransform = GameObject.FindWithTag("Player").transform;
	}

	public void Update()
	{
		if (Vector2.Distance(playerTransform.position, owner.transform.position) < findRange)
		{
			//owner.ChangeState("Trace");
		}
	}

	public void Exit()
	{

	}
}

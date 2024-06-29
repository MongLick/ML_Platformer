using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : Monster
{
	public enum State
	{
		Idle,
		Trace,
		Return,
		Die
	}

	private Transform playerTransform;
	private State curState;
	private Vector3 startPos;

	[SerializeField] float moveSpeed;
	[SerializeField] float findRange;

	private void Start()
	{
		curState = State.Idle;
		playerTransform = GameObject.FindWithTag("Player").transform;
		startPos = transform.position;
	}

	private void Update()
	{
		switch (curState)
		{
			case State.Idle:
				IdleUpdate();
				break;
			case State.Trace:
				TraceUpdate();
				break;
			case State.Return:
				ReturnUpdate();
				break;
			case State.Die:
				DieUpdate();
				break;

		}
	}

	private void IdleUpdate()
	{
		if(Vector2.Distance(playerTransform.position, transform.position) < findRange)
		{
			curState = State.Trace;
		}
	}

	private void TraceUpdate()
	{
		Vector3 dir = (playerTransform.position - transform.position).normalized;
		transform.Translate(dir * moveSpeed * Time.deltaTime);

		if (Vector2.Distance(playerTransform.position, transform.position) > findRange)
		{
			curState = State.Return;
		}
	}

	private void ReturnUpdate()
	{
		Vector3 dir = (startPos - transform.position).normalized;
		transform.Translate(dir * moveSpeed * Time.deltaTime);

		if(Vector2.Distance( transform.position, startPos) < 0.01f)
		{
			curState = State.Idle;
		}
	}

	private void DieUpdate()
	{

	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : Monster
{
	public Transform playerTransform;
	public Vector3 startPos;

	public enum State{ Idle, Trace, Return, Die}
	[Header("FSM")]
	private StateMachine<State, Eagle> fsm;

	[SerializeField] IdleState<Eagle> idleState;


	private void Awake()
	{
		fsm = new StateMachine<State, Eagle>();

		fsm.AddState(State.Idle, idleState);
	}

	private void Start()
	{
		fsm.SetInitState(State.Idle);
		startPos = transform.position;
	}

	private void Update()
	{
		fsm.Update();

		if(false)
		{
			fsm.ChangeState(State.Trace);
		}
	}

	private void ChangeState(string stateName)
	{
		switch (stateName)
		{
			case "Idle":
				fsm.ChangeState(State.Idle);
				break;
			case "Trace":
				fsm.ChangeState(State.Trace);
				break;
			case "Return":
				fsm.ChangeState(State.Return);
				break;
		}
	}

	private class IdleState : IState
	{
		private Eagle owner;
		private Transform playerTransform;
		private float findRange = 5f;

		public IdleState(Eagle owner)
		{
			this.owner = owner;
		}

		public void Enter()
		{
			playerTransform = GameObject.FindWithTag("Player").transform;
		}

		public void Update()
		{
			if (Vector2.Distance(playerTransform.position, owner.transform.position) < findRange)
			{
				owner.ChangeState("Trace");
			}
		}

		public void Exit()
		{

		}
	}

	private class TraceState : IState
	{
		private Eagle owner;
		private Transform playerTransform;
		private float findRange = 5f;
		private float moveSpeed = 2f;

		public TraceState(Eagle owner)
		{
			this.owner = owner;
		}

		public void Enter()
		{
			playerTransform = GameObject.FindWithTag("Player").transform;
		}

		public void Update()
		{
			Vector3 dir = (playerTransform.position - owner.transform.position).normalized;
			owner.transform.Translate(dir * moveSpeed * Time.deltaTime);

			if (Vector2.Distance(playerTransform.position, owner.transform.position) > findRange)
			{
				owner.ChangeState("Return");
			}
		}

		public void Exit()
		{

		}
	}

	private class ReturnState : IState
	{
		private Eagle owner;
		private float moveSpeed = 10f;

		public ReturnState(Eagle owner)
		{
			this.owner = owner;
		}

		public void Enter()
		{

		}

		public void Update()
		{
			Vector3 dir = (owner.startPos - owner.transform.position).normalized;
			owner.transform.Translate(dir * moveSpeed * Time.deltaTime);

			if (Vector2.Distance(owner.transform.position, owner.startPos) < 0.01f)
			{
				owner.ChangeState("Idle");
			}
		}

		public void Exit()
		{

		}
	}




	// 아래는 상태패턴 위에는 유한상태머신 FSM

	/*
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
		private State curState;

		private void Start()
		{
			curState = State.Idle;
		}

		public enum State
		{
			Idle,
			Trace,
			Return,
			Die
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
			if(Vector2.Distance(transform.position, playerTransform.position) < findRange)
			{
				curState= State.Trace;
			}
		}

		private void DieUpdate()
		{

		}
	*/
}

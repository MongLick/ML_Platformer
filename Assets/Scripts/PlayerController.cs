using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
	[Header("Component")]
	[SerializeField] Rigidbody2D rigid;
	[SerializeField] SpriteRenderer render;
	[SerializeField] Animator animator;

	[Header("Property")]
	[SerializeField] Vector2 moveDir;
	[SerializeField] float moveSpeed;
	[SerializeField] float brakeSpeed;
	[SerializeField] float maxYSpeed;
	[SerializeField] float maxXSpeed;
	[SerializeField] float jumpSpeed;

	private bool isGround;

	private void OnMove(InputValue value)
	{
		moveDir = value.Get<Vector2>();

		if(moveDir.x < 0)
		{
			render.flipX = true;
			animator.SetBool("Run", true);
		}
		else if(moveDir.x > 0)
		{
			render.flipX= false;
			animator.SetBool("Run", true);
		}
		else
		{
			animator.SetBool("Run", false);
		}
	}

	private void OnJump(InputValue value)
	{
		if(value.isPressed && isGround)
		{
			Jump();
		}
	}

	private void Move()
	{
		if (moveDir.x < 0 && rigid.velocity.x > -maxXSpeed)
		{
			rigid.AddForce(Vector2.right * moveDir.x * moveSpeed);
		}
		else if(moveDir.x > 0 && rigid.velocity.x < maxXSpeed)
		{
			rigid.AddForce(Vector2.right * moveDir.x * moveSpeed);
		}
		else if(moveDir.x == 0 && rigid.velocity.x > 0.1f)
		{
			rigid.AddForce(Vector2.left * brakeSpeed);
		}
		else if(moveDir.x == 0 && rigid.velocity.x < -0.1f)
		{
			rigid.AddForce(Vector2.right * brakeSpeed);
		}

		if(rigid.velocity.y < -maxYSpeed)
		{
			Vector2 velocity = rigid.velocity;
			velocity.y = -maxYSpeed;
			rigid.velocity = velocity;
		}

		animator.SetFloat("YSpeed", rigid.velocity.y);
	}

	private void Jump()
	{
		Vector2 velocity = rigid.velocity;
		velocity.y = jumpSpeed;
		rigid.velocity = velocity;
	}

	private void FixedUpdate()
	{
		Move();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		isGround = true;
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		isGround = false;
	}
}

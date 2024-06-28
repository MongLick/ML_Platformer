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
	[SerializeField] LayerMask groundCheckLayer;

	private bool isGround;
	private int groundCount;

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

	private void OnTriggerEnter2D(Collider2D collision)
	{
		/*if(((1 << collision.gameObject.layer) & groundCheckLayer) != 0)
		{
			// 원리는 이거임
		}*/

		// 확장 매서드를 만듬
		if(groundCheckLayer.Contain(collision.gameObject.layer))
		{
			groundCount++;
			isGround = groundCount > 0;
			animator.SetBool("IsGround" , isGround);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (groundCheckLayer.Contain(collision.gameObject.layer))
		{
			groundCount--;
			isGround = groundCount > 0;
			animator.SetBool("IsGround", isGround);
		}
	}
}

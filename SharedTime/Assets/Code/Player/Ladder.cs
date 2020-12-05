using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    public float speed;
    PlayerMovement player;
	
    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
    }
	
    private void OnTriggerStay2D(Collider2D other)
    {
		if (other.CompareTag("Player"))
		{
			if (Input.GetKey(KeyCode.W)|| Input.GetKey(KeyCode.UpArrow))
			{
				if (!player.onLadder){
					player.snapXToPosition(gameObject.transform.position.x);
					player.onLadder = true;
					player.characterShooting = false;
				}
				
				player.isJumping = false;
				other.GetComponent<Rigidbody2D>().velocity = new Vector2(0, speed);
				
				if (player.Character2)
					player.SetAnimation(24);
				else
					player.SetAnimation(4);
			}
			else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
			{
				if (!player.onLadder){
					player.snapXToPosition(gameObject.transform.position.x);
					player.onLadder = true;
					player.characterShooting = false;
				}
				
				player.isJumping = false;
				other.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -speed);
				
				if (player.Character2)
					player.SetAnimation(24);
				else
					player.SetAnimation(4);
			}
			else
			{
				if (player.onLadder)
				{
					other.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0f);
					
				if (player.Character2)
					player.SetAnimation(23);
				else
					player.SetAnimation(3);
				}
			}
		}
    }
	
	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			player.onLadder = false;
		}
	}
}

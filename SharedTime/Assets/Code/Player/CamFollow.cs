using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.11f;
	
    public Vector3 offset;
	public Rigidbody2D characterBody;
	private Vector3 finalOffset;
	private Vector3 previousOffset;
	private Vector3 nextOffset;
	
	//FOR BACKGROUNDS AND PARALLAXES
	public Transform background_position;
	public SpriteRenderer background_sprite;
	
	public Transform parallax_position;
	public SpriteRenderer parallax_sprite;
	
	void Start(){
		finalOffset = offset;
	}
	
    private void FixedUpdate()
    {
        Vector3 desiedPosition = target.position + finalOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiedPosition, smoothSpeed);
        transform.position = smoothedPosition;
		
		//IN CASE OF JUMPING/FALLING
		previousOffset = Vector3.Lerp(previousOffset, nextOffset, smoothSpeed);
		
		float new_x, new_y;
		
		if (characterBody.velocity.x < 7.1f && characterBody.velocity.x > 7.1f)
			new_x = offset.x + characterBody.velocity.x/12;
		else
			new_x = offset.x + characterBody.velocity.x/4;
		
		if (characterBody.velocity.y < 0.6f && characterBody.velocity.y > 1.3f)
			new_y = offset.y + characterBody.velocity.y/10;
		else
			new_y = offset.y + characterBody.velocity.y/4;
		
		nextOffset = new Vector3(new_x, new_y, offset.z);
		
		//SMOOTH IT OUT
		finalOffset = Vector3.Lerp(previousOffset, offset, smoothSpeed*2);
		
		//SET BACKGROUND TO CAMERA POSITION
		background_position.position = new Vector3(target.position.x, target.position.y + 2.1f, background_position.position.z);
		
		updateParallax();
    }
	
	private void updateParallax(){
		float x_parallax, y_parallax;
		x_parallax = characterBody.position.x/20;
		
		if (x_parallax > 10.0f)
			x_parallax = 10.0f;
		else if (x_parallax < -10.0f)
			x_parallax = -10.0f;
		
		y_parallax = characterBody.position.y/15;
		
		Vector3 new_parallax = new Vector3(target.position.x - x_parallax, target.position.y + 1.1f - y_parallax, background_position.position.z);
		parallax_position.position = new_parallax;
	}
	
	public void setBackground(Sprite new_background){
		background_sprite.sprite = new_background;
	}
	
	public void setParallax(Sprite new_parallax){
		parallax_sprite.sprite = new_parallax;
	}
}

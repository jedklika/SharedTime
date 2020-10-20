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
    }
}

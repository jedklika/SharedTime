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
		
		if (characterBody.velocity.y < 0)
			nextOffset = new Vector3(offset.x + characterBody.velocity.x/8, offset.y + characterBody.velocity.y/2, offset.z);
		else
			nextOffset = new Vector3(offset.x + characterBody.velocity.x/8, offset.y + characterBody.velocity.y/6, offset.z);
		
		//SMOOTH IT OUT
		finalOffset = Vector3.Lerp(previousOffset, offset, smoothSpeed);
    }
}

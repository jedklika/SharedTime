﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyGround : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void OnTriggerEnter2D(Collider2D other)
    {
		Debug.Log(other.tag);
		Debug.Log(other.name);
		
		if (other.CompareTag("explodableDoor")){
			Debug.Log("Destroying Door");
			Destroy(other.gameObject);
		}
	}
}

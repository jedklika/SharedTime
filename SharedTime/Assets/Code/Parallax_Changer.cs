using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Parallax_Changer : MonoBehaviour
{
	public GameObject to_set_inactive;
	public GameObject to_set_active;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void OnTriggerEnter2D(Collider2D col){
		if (col.CompareTag("Player")){
			if (to_set_inactive != null)
				to_set_inactive.SetActive(false);
				
			if (to_set_active != null)
				to_set_active.SetActive(true);
		}
	}
}

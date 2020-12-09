using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetDoor : MonoBehaviour
{
	public GameObject door_to_delete;
	public bool is_delete;
	
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
		if (other.CompareTag("bullet")){
			if (is_delete){
				//Debug.Log("Destroying Door");
				Destroy(door_to_delete);
			} else {
				door_to_delete.SetActive(true);
			}
			
			Destroy(gameObject);
		}
	}
}

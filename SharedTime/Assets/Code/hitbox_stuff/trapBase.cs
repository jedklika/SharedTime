using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trapBase : MonoBehaviour
{
    public GameObject trapTrigger;
	public bool trapActivated = false;

	void Start()
	{
		
	}
	
    // Update is called once per frame
    void Update()
    {

    }
	
	public IEnumerator delayDelete(float delay){
		yield return new WaitForSeconds(delay);
		
		Destroy(gameObject);
	}
}

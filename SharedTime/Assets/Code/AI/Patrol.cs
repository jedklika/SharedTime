using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    [SerializeField]
    Transform[] waypoints;
    public float speed;
    public bool canFlip;
    public bool FoeFlipped;
    public int health;
    Rigidbody2D Rb;
    int waypointIndex = 0;
	public SpriteRenderer S;
	
	public int enemyID;

    // Start is called before the first frame update
    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        transform.position = waypoints[waypointIndex].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, waypoints[waypointIndex].transform.position, speed * Time.deltaTime);
        if (transform.position == waypoints[waypointIndex].transform.position)
        {
            waypointIndex += 1;
        }
        if (waypointIndex == waypoints.Length)
        {
            waypointIndex = 0;
        }
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }
	
    void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Hitting trigger of tag name " + collision.gameObject.tag);

        if (collision.gameObject.CompareTag("Right") && canFlip)
        {
            //transform.localScale = new Vector3(-1f, 1f, 1f);
            FoeFlipped = S.flipX = true;
        }

        if (collision.gameObject.CompareTag("Left") && canFlip)
        {
            //transform.localScale = new Vector3(1f, 1f, 1f);
            FoeFlipped = S.flipX =  false;
        }
    }
   
   public void TakeDamage(int damage)
   {
        health -= damage;

        //Check if dead
        if (health <= 0)
        {
            GameObject.Destroy(this.gameObject);
        }
   }
}

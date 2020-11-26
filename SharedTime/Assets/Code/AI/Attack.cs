using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float speed;
    public float stopDistance;
    private float TimeBtwShots;
    public float StartTimeBtwShots;
    public Transform player;
    private Vector2 Movement;
    Rigidbody2D Rb;
    public GameObject Bullet;
    Patrol P;
	
    // Start is called before the first frame update
    void Start()
    {
        TimeBtwShots = StartTimeBtwShots;
        P = FindObjectOfType<Patrol>();
        Rb = this.GetComponent<Rigidbody2D>();
        Rb.bodyType = RigidbodyType2D.Dynamic;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void updateAttack(){
		Rb.bodyType = RigidbodyType2D.Dynamic;
        Rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (Vector2.Distance(transform.position, player.position) > stopDistance)
        {
            Rb.bodyType = RigidbodyType2D.Dynamic;
            Vector3 direction = player.position - transform.position;
            direction.Normalize();
            Movement = direction;
            moveCharacter(Movement);
            Rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        }

        if (Vector2.Distance(transform.position, player.position) < stopDistance)
        {
            Rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        if (TimeBtwShots <= 0)
        {
            Instantiate(Bullet, transform.position, Quaternion.identity);
            TimeBtwShots = StartTimeBtwShots;
        }
        else
        {
            TimeBtwShots -= Time.deltaTime;
        }

        if (P.FoeFlipped)
        {
            //transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        if (P.FoeFlipped == false)
        {
            //transform.localScale = new Vector3(1f, 1f, 1f);
        }
	}
	
    void moveCharacter(Vector2 direction)
    {
        Rb.MovePosition((Vector2)transform.position + (direction * speed * Time.deltaTime));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoeBullet : MonoBehaviour
{
    public float speed;
    private Transform player;
    private Vector2 target;
    GameManager Gm;
    public int damageDealt;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        target = new Vector2(player.position.x, player.position.y);
        Gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (transform.position.x == target.x && transform.position.y == target.y)
        {
            DestroyProjectie();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DestroyProjectie();
            Gm.setDamage(damageDealt);
            Debug.Log(Gm.playerHealth);
        }
    }
    void DestroyProjectie()
    {
        Destroy(gameObject);
    }
}

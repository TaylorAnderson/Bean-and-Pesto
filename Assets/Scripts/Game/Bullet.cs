using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;
public enum BulletTeam
{
    PLAYER,
    ENEMIES,
}
public class Bullet : Entity
{
    public Vector2 velocity;
    public float lifetime = 1;

    [HideInInspector]
    public float power = 1;
    [HideInInspector]
    public EntityType owner;
    public BulletTeam team;
    public bool explodeOnHit = false;
    public GameObject explosion;
    public float angleOffset = 0;
    public ShieldColors color;

    public bool isReflectable = false;


    // Update is called once per frame
    public override void DoUpdate()
    {

        lifetime -= Time.deltaTime;

        if (lifetime < 0)
        {
            Die();
        }
        this.transform.eulerAngles = Vector3.forward * (Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg + angleOffset);
        this.transform.position += (Vector3)this.velocity;
    }
    public void Die()
    {
        if (explodeOnHit)
        {
            Instantiate(explosion).transform.position = transform.parent.position;
        }
        if (transform.parent) Destroy(transform.parent.gameObject); //we do this becasue we often use a pattern where an enemy will have a childed bullet to do damage
        Destroy(gameObject);
    }
}


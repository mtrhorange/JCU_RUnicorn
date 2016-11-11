﻿using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    //laser prefab
	public GameObject eLaser;

    //health
	public bool weakened = false;
	protected int HP = 1;

    //movement
	protected bool goDown = false, faceLeft = false;
    protected float moveSpeed = 60f;

    //attack
    public float attackTimer;
    protected float minTime = 5f, maxTime = 8f;


	//Start
	protected virtual void Start()
    {
        //random whether face up or down
        if (Random.Range(0, 2) == 1)
        {
            transform.Rotate(Vector3.forward, -90f);
            faceLeft = true;
        }
        else
        {
            transform.Rotate(Vector3.forward, 90f);
        }

        //attack timer
        setAttackTimer();
	}

	// Update is called once per frame
	protected virtual void Update()
    {
        Move();
	}

    //Move
    protected void Move()
    {
        if (goDown)
        {
            if (faceLeft)
            {
                GetComponent<Rigidbody2D>().velocity = transform.right * Time.deltaTime * moveSpeed;
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = -transform.right * Time.deltaTime * moveSpeed;
            }
            if (transform.position.y < AIManager.instance.cameraBounds.min.y)
            {
                goDown = false;
            }
        }
        else
        {
            if (faceLeft)
            {
                GetComponent<Rigidbody2D>().velocity = -transform.right * Time.deltaTime * moveSpeed;
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = transform.right * Time.deltaTime * moveSpeed;
            }
            if (transform.position.y > AIManager.instance.cameraBounds.max.y)
            {
                goDown = true;
            }
        }


        //attack once timer is over
        if (attackTimer <= 0)
        {
            Attack();
            //reset timer
            setAttackTimer();
        }

        attackTimer -= Time.deltaTime;
    }

    //Attack
    protected void Attack()
    {
        Instantiate(eLaser, transform.position, transform.rotation);
    }

	//called when this entity receives damage from another source
	public void GetDamage(int dmg)
	{
		HP -= dmg;
		//check die
		if (HP <= 0)
		{
			Destroy(this.gameObject);
		}
	}

    //set attack timer
    protected void setAttackTimer()
    {
        attackTimer = Random.Range(minTime, maxTime);
    }
}
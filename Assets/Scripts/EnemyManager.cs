﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
    public ColourObject myColour;
    Rigidbody2D myRB;
    Animator myAnim;
    Transform myTrans;
    public LayerMask enemyMask;

    bool toRight = true;
    bool changedDirection = true;
    public float changeTime = 2;
    public float speed = 1;
    public float myWidth, myHeight;


	// Use this for initialization
	void Start () {
        myRB = this.GetComponent<Rigidbody2D>();
        myAnim = this.GetComponent<Animator>();
        myTrans = this.GetComponent<Transform>();
        //StartCoroutine(changeDirectionAfter(changeTime)); //Use to change direction after time instead of collision
	}
	
	// Update is called once per fraction of sec
	void FixedUpdate () {
        /* Change Direction AfterGiven Time
         * Calls coroutine after changing direction
         
        if (!changedDirection)
        {
            StartCoroutine(changeDirectionAfter(changeTime));
        }
        */

        /*Change direction with Line Cast
         */
        Vector2 lineCastPos = new Vector2(myTrans.position.x,myTrans.position.y)- new Vector2(myTrans.right.x,myTrans.right.y) * myWidth - Vector2.up * (myHeight / 2);

        Debug.DrawLine(lineCastPos, lineCastPos + Vector2.down);
        //Cast line to check grounded
        bool isGrounded = !Physics2D.Linecast(lineCastPos, lineCastPos + Vector2.down, enemyMask);

        Debug.DrawLine(lineCastPos, lineCastPos - new Vector2(myTrans.right.x, myTrans.right.y) * .05f);
        //Case line to check if blocked
        bool isBlocked = Physics2D.Linecast(lineCastPos, lineCastPos - new Vector2(myTrans.right.x, myTrans.right.y) * .05f, enemyMask);

        //If theres no ground, turn around. Or if I hit a wall, turn around
        if (!isGrounded || isBlocked)
        {
            Vector3 currRot = myTrans.eulerAngles;
            currRot.y += 180;
            myTrans.eulerAngles = currRot;
            toRight = !toRight;
        }

        if (toRight)
            myRB.velocity = new Vector3(speed , myRB.velocity.y, 0);
        else
            myRB.velocity = new Vector3(-speed, myRB.velocity.y, 0);


    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "BULLET")
        {
            this.GetComponent<EnemyHealth>().damage(collision.gameObject.GetComponent<BulletManager>().bulletCol);
        }
    }
    IEnumerator changeDirectionAfter(float time)
    {
        changedDirection = true;
        yield return new WaitForSecondsRealtime(time);
        toRight = !toRight;
        changedDirection = false;
    }
}
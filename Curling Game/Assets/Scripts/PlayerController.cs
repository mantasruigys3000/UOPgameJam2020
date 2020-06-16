﻿using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public GameObject swinger;
    public Camera cam;
    public GameObject ballPrefab;
    public float weight = 5f;
    public float vspd = 0;
    public float hspd = 0;
    public float vel = 0;
    float maxVel = 7f;
    float sin = 0;
    float cos = 0;

    [SyncVar]
    Boolean hasBall = false;


    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update() {

    

        if (isLocalPlayer) {
            
            Vector3 mouse_pos;
            Transform target = gameObject.transform; //Assign to the object you want to rotate
            Vector3 object_pos;
            float angle;

            mouse_pos = Input.mousePosition;
            mouse_pos.z = 5.23f; //The distance between the camera and object
            object_pos = cam.WorldToScreenPoint(target.position);
            mouse_pos.x = mouse_pos.x - object_pos.x;
            mouse_pos.y = mouse_pos.y - object_pos.y;
            angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
            //transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            swinger.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            //swinger.transform.position = gameObject.transform.position;
            float d = (float)(angle * Math.PI) / 180;
            float asin = Mathf.Sin(d);
            float acos = Mathf.Cos(d);

            swinger.transform.position = new Vector3(transform.position.x + (acos * 1f), transform.position.y + (asin * 1f ), transform.position.z);





            //swinger.transform.RotateAround(transform.position, swinger.transform.right, angle);




            float spd = 10f;

            if (Input.GetMouseButton(1)) {

                float sinDiff = 0;
                float cosDiff = 0;

                if (sin !=0 && cos != 0) {
                     sinDiff =Mathf.Abs(sin) - Mathf.Abs(Mathf.Sin(d));
                     cosDiff =Mathf.Abs(cos) - Mathf.Abs(Mathf.Cos(d));
                }
                float diff = sinDiff + cosDiff;

                sin = Mathf.Sin(d);
                cos = Mathf.Cos(d);

                vel += 0.05f;
                vel -= (diff*17);
                vel = (vel > maxVel) ? maxVel : vel;



                



            } else {
                vel -= weight;
                vel = (vel < 0) ? 0 : vel;

                if (vel == 0) {
                    hspd = 0;
                    vspd = 0;
                }
            }
            hspd = (vel * cos);
            vspd = (vel * sin);





            //vspd -= weight;
            //hspd -= weight;

            //hspd = (hspd < 0) ? 0 : hspd;
            //vspd = (vspd < 0) ? 0 : vspd;




            Vector3 currentPos = transform.position;
            Vector3 newPos = new Vector3(currentPos.x + (hspd * Time.deltaTime), currentPos.y + (vspd * Time.deltaTime), currentPos.y+ 0.5f);
            transform.position = newPos;
        } else {
            gameObject.GetComponent<PlayerController>().enabled = false;
            cam.enabled = false;
        }

        if (Input.GetMouseButtonDown(0)) {
            if (hasBall) {
                CmdYeet();
            }
            
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "curlingStone") {
            CmdSetStone();

        }
    }

    [Command]
    void CmdYeet() {
        GameObject ball = Instantiate(ballPrefab, transform.position, Quaternion.identity);
        curlingStone stone = ball.GetComponent<curlingStone>();
        ball.GetComponent<Rigidbody2D>().velocity = -ball.transform.right * 20f;
        
        stone.owner = gameObject;


        NetworkServer.Spawn(ball);

        RpcYeet();
    }

    void CmdSetStone() {
        hasBall = true;

    }

    [ClientRpc]
    void RpcYeet() {
        swingScript swingerScript = swinger.GetComponent<swingScript>();
        if (swingerScript.inCollision != null) {
            GameObject obj = swingerScript.inCollision;
            //obj.transform.position = Vector3.zero;
            Rigidbody2D srb = obj.GetComponent<Rigidbody2D>();
            srb.velocity = swinger.transform.right* 2;



        }
    }

    void RpcSetStone(curlingStone stone) {
        
    }
}

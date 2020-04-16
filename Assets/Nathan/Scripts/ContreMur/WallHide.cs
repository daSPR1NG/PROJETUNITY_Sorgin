﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHide : MonoBehaviour
{

    public GameObject Wall;
    public GameObject Player;


    Vector3 ClosestPt;

    public GameObject ball; //DEBUG

    Quaternion wantedRotation;
    int layer_mask;
    bool Hided;

    bool Left = true;
    bool Right = true;

    public Detector LeftDetector;
    public Detector RightDetector;
    public Detector BehindDetector;

    float Timer;


    // Start is called before the first frame update
    void Start()
    {
        layer_mask = LayerMask.GetMask("Wall");
    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;

        if (Wall != null && !Hided)
        {
            ClosestPt = Wall.GetComponent<Collider>().ClosestPoint(transform.position);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, ClosestPt-transform.position, out hit, Mathf.Infinity,layer_mask))
            {
                Debug.Log("bordel ca devrait marcher");
                wantedRotation = Quaternion.LookRotation(hit.normal);

                if (hit.transform.CompareTag("Wall"))
                {
                    ball.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("ControllerA") )
                    {
                        if (Timer <= 0)
                        {
                            Invoke("ChangeHided", 0.05f);
                            Timer = 1;
                        }
                    }
                }
                else
                {
                    ball.SetActive(false);
                }
            }
        }
        else
        {
            ball.SetActive(false);
        }


        if ((Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("ControllerA")) && Hided)
        {
            if (Timer <= 0)
            {
                Player.GetComponent<MoveScript>().TopDownCamera();

                Invoke("ChangeHided", 1f);
                Timer = 1;
            }
        }

        if (Hided)
        {
            ball.SetActive(false);

            Player.transform.rotation = Quaternion.RotateTowards(Player.transform.rotation, wantedRotation, 500f * Time.deltaTime);

            if(Player.transform.rotation == wantedRotation)
            {
                Hide();
            }
        }

        //DEBUG

        ball.transform.position = ClosestPt;

        Debug.DrawRay(transform.position, ClosestPt-transform.position, Color.red);
        //DEBUG

        Player.GetComponent<MoveScript>().OnWall = Hided;

    }

    void Hide()
    {

        Player.GetComponent<MoveScript>().MoveWall(RightDetector.On, LeftDetector.On, BehindDetector.On);

        if (BehindDetector.On == false)
        {
            Player.GetComponent<MoveScript>().WallBack(5);
        }
        else
        {
            Player.GetComponent<MoveScript>().WallBack(0);
        }

    }

    void ChangeHided()
    {
        Hided = !Hided;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            Wall = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            Wall = null;
        }
    }
}

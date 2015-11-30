﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof (PlayerMotor))]

public class PlayerController: MonoBehaviour {

    public float cameraStayTime;

    public bool alive { get; set; }
    public bool finished { get; set; }
    public bool canLoad { get; set; }
    public bool start { get; set; }
    public bool wait { get; set; }
    public bool startFade { get; set; }
    public bool startReturn { get; set; }
    public Transform[] checkpoints { get; set; }
    public Vector3 checkpointPosition { get; set; }
    public float cameraBackToPositionTime { get; set; }
    public int checkpointNumber { get; set; }

    private WaitForSeconds halfBackTime;
    private WaitForSeconds fullBackTime;
    private Rigidbody2D rb;  
    private PlayerMotor motor;
    private Throwable throwable;
    private bool canPlay = false;
    private bool died = false;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        motor = GetComponent<PlayerMotor>();
        throwable = GetComponent<Throwable>();
        GameObject globalGM = GameObject.FindGameObjectWithTag("globalGM");
        if (globalGM != null) { cameraBackToPositionTime = globalGM.GetComponent<UIManager>().fadeTime; }
        halfBackTime = new WaitForSeconds(cameraBackToPositionTime / 2);
        fullBackTime = new WaitForSeconds(cameraBackToPositionTime);
        wait = false;
        alive = true;
        canLoad = true;
        StartCoroutine(WaitAtStart());
    }

	void Update () {
        if (canPlay)
        {
            if (!wait)
            {
                if (!died)
                {
                    if (Input.GetButton("Fire1"))
                    {
                        motor.moveUp = true;
                        start = true;

                    }
                    else if (Input.touchCount > 0)
                    {
                        motor.moveUp = true;
                        start = true;

                    }
                    motor.holdRotation = true;
                    motor.moveRight = true;
                }
                if (Input.GetButtonDown("Fire1"))
                {
                    died = false;
                }
                else if (Input.touchCount > 0)
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        died = false;
                    }
                }
                if (!alive)
                {
                    checkpointPosition = checkpoints[checkpointNumber].position;
                    if (!finished) { StartCoroutine(Respawn(checkpointPosition)); }
                }
            }
        }
    }

    IEnumerator Respawn(Vector3 position)
    {
        
        wait = true;
        transform.position = new Vector3(0, -10, 0);
        transform.rotation = new Quaternion();
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        rb.isKinematic = true;
        yield return fullBackTime;
        startFade = true;
        yield return halfBackTime;
        System.GC.Collect();
        Resources.UnloadUnusedAssets();
        startReturn = true;
        transform.position = position;
        yield return halfBackTime;
        rb.isKinematic = false;
        start = false;
        startFade = false;
        startReturn = false;
        alive = true;
        died = true;
        wait = false;
    }

    IEnumerator WaitAtStart()
    {
        yield return halfBackTime;
        if (throwable != null)
        {
            throwable.Launch(motor.throwDirection);
            throwable.enabled = false;
        }
        yield return fullBackTime;
        yield return fullBackTime;
        canPlay = true;
    }
}
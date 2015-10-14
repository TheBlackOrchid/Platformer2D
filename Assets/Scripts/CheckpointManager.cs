﻿using UnityEngine;
using System.Collections;

public class CheckpointManager : MonoBehaviour {

    public Transform checkpointObject;

    private Transform[] checkpoints;
    private PlayerController playerController;

	// Use this for initialization
	void Start () {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        checkpoints = new Transform[checkpointObject.childCount];
        int i = 0;
        foreach (Transform child in checkpointObject)
        {
            checkpoints[i] = child;
            i++;
        }
    }

	
	// Update is called once per frame
	void Update () {
        float playerPositionX = playerController.gameObject.transform.position.x;

        if (!playerController.alive)
        {
            for (int i = 1; i < checkpoints.Length; i++)
            {
                if (playerPositionX >= checkpoints[i - 1].position.x && playerPositionX <= checkpoints[i].position.x)
                {
                    //StartCoroutine(playerController.Respawn(checkpoints[i - 1].position));
                    playerController.respawnPosition = checkpoints[i - 1].position;
                }

            }
        }
	}



}

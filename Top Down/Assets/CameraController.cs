using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject Player;       //Public variable to store a reference to the player game object

    private Vector3 offset;         //Private variable to store the offset distance between the player and camera

    public float followRate = 5f;
    float followHeight = -20f;

    void LateUpdate()
    {
        if (Player == null)
            return;


        Vector3 startPos = transform.position;

        startPos.z = followHeight;

        Vector3 endPos = Player.transform.position;

        endPos.z = followHeight;

        transform.position = Vector3.Lerp(startPos, endPos, Time.deltaTime * followRate);

    }

}

     
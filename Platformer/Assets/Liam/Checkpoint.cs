using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    public string CheckpointName = "";
    public bool IsFirstInLevel = false;

    void Start()
    {
        if (IsFirstInLevel)
        {
            PlayerController2D pc = GameObject.Find("Basic Player Controller").GetComponent<PlayerController2D>();
            if (pc != null)
            {
                SetCheckPoint(pc);
            }
            else { Debug.LogWarning("Didnt' find the player..."); }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController2D pc = other.gameObject.GetComponent<PlayerController2D>();
        if (pc != null)
        {
            SetCheckPoint(pc);
        }
        
    }


    void SetCheckPoint(PlayerController2D pc)
    {
        Debug.Log("Hit checkpoint: " + CheckpointName);

        if (pc.GetLastCheckpoint() != null)
            pc.GetLastCheckpoint().gameObject.GetComponent<MeshRenderer>().materials[0].color = Color.blue;

        pc.SetLastCheckpoint(this);

        this.gameObject.GetComponent<MeshRenderer>().materials[0].color = Color.green;
    }
}

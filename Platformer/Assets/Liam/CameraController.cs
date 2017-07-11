using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public LevelManager Manager;
    public Transform FollowTarget;
    public PlatformerMotor2D Motor;
    public Vector2 FocusAreaSize;

    FocusArea focusArea;
    struct FocusArea
    {
        public Vector2 center;
        //public Vector2 velocity;
        float left, right, top, bottom;

        float offset;

        public FocusArea(Bounds targetBounds, Vector2 size)
        {
            offset = 2;
            left = targetBounds.center.x - size.x / 2;
            right = targetBounds.center.x + size.x / 2;
            bottom = (targetBounds.min.y - size.y / 2f) + offset;
            top = (targetBounds.min.y + size.y - size.y) +offset;

            //velocity = Vector2.zero;   
            center = new Vector2((left + right) / 2, (top + bottom) / 2);
        }

        public void Update(Bounds targetBounds)
        {
            float shiftX = 0;
            if (targetBounds.min.x < left)
            {
                shiftX = targetBounds.min.x - left;
            }
            else if (targetBounds.max.x > right)
            {
                shiftX = targetBounds.max.x - right;
            }
            left +=  shiftX;
            right +=  shiftX;

            float shiftY = 0;
            if (targetBounds.min.y < bottom)
            {
                shiftY = targetBounds.min.y - bottom;
            }
            else if (targetBounds.max.y > top)
            {
                shiftY = targetBounds.max.y - top;
            }
            top += shiftY;
            bottom += shiftY;

            center = new Vector2((left + right) / 2, (top + bottom) / 2 );
            //velocity = new Vector2(right, shiftY);
        }

    }



	// Use this for initialization
	void Start () {
        FollowTarget = GameObject.FindGameObjectWithTag("Player").transform;
        focusArea = new FocusArea(FollowTarget.gameObject.GetComponent<Collider2D>().bounds, FocusAreaSize);
	}

    void LateUpdate()
    {
        if (FollowTarget == null)
        {
            Debug.LogWarning("no target");
            return;
        }

        if (Manager.GetGameState() == LevelManager.GameStates.Running || Manager.GetGameState() == LevelManager.GameStates.Starting)
        {

            focusArea.Update(FollowTarget.gameObject.GetComponent<Collider2D>().bounds);
            Vector3 pos = focusArea.center;
            pos.x = FollowTarget.position.x + 3.5f;
            if (Motor.motorState == PlatformerMotor2D.MotorState.OnGround)
            {
                pos.y = Mathf.Lerp(this.transform.position.y, FollowTarget.position.y + 2f, Time.deltaTime);
            }
            else
            {
                pos.y = Mathf.Lerp(this.transform.position.y, pos.y, Time.deltaTime);
            }
            pos.z = -20;
            this.transform.position = pos;
        }
        else if (Manager.GetGameState() == LevelManager.GameStates.PlayerRespawningStarting)
        {
            this.transform.position = new Vector3(FollowTarget.position.x + 3.5f, FollowTarget.position.y + 2f, -20f);
        }
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(focusArea.center, FocusAreaSize);
    }
}

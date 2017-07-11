using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchController : MonoBehaviour {

    public GameObject PopupUi;
    public RectTransform DPad;
    public RectTransform TouchVis;
    public bool UseTouch;

    public float x;
    public float y;
    public bool jump;
    public bool attack;

    private float fullWidth = 137f;
    private float deadWidth = 55.8f;
    private float halfScreenHeight;


    public UnityEngine.UI.Text TouchOnePos;
    public UnityEngine.UI.Text TouchPointX;

    private Vector2 touchOrigin = -Vector2.one;

	// Use this for initialization
	void Start () {
        halfScreenHeight = Screen.height / 2f;
//#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR_WIN

//#else
            UseTouch = true;
            PopupUi.gameObject.SetActive(true);

//#endif


    }

    // Update is called once per frame
    void Update () {

        x = 0f;
        y = 0f;
        jump = false;
        attack = false;

        //  for (int i = 0; i < Input.touchCount; i++)
        //  {
        if (!Input.GetMouseButton(0))
            return;
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);

            //Ray2D ray = Camera.main.ScreenPointToRay2D(Input.GetTouch(i).position);
            //RaycastHit2D hit = Physics2D.Raycast(Input.touches[i].position, -Vector2.up);
            if (hit != null && hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "UI")
                {
                    if (hit.collider.gameObject.name == "Move Left")
                        x = -1f;
                    if (hit.collider.gameObject.name == "Move Right")
                        x = 1f;
                    if (hit.collider.gameObject.name == "Jump")
                        jump = true;
                    if (hit.collider.gameObject.name == "Attack")
                        attack = true;
                }
            }
    
       // }



        //if (Input.touchCount > 0)
        //{
        //    Touch myTouch = Input.touches[0];
            
        //    if (myTouch.phase == TouchPhase.Began)
        //    {
        //        touchOrigin = myTouch.position;
        //        DPad.position = touchOrigin;
                
        //    }
        //    else if (myTouch.phase == TouchPhase.Moved || myTouch.phase == TouchPhase.Stationary)
        //    {
        //        //UpdateJoystick(myTouch);

        //    }
        //    else if (myTouch.phase == TouchPhase.Ended || myTouch.phase == TouchPhase.Canceled)
        //    {

        //        //UpdateJoystick(myTouch);


        //    }
        //    else
        //    {
        //        x = 0f;
        //        y = 0f;
        //    }


        //    if (Input.touchCount > 1)
        //    {
        //        for (int i = 1; i < Input.touchCount; i++)
        //        {
        //            myTouch = Input.touches[i];
        //            //TouchOnePos.text = "Touch1 Height: " + myTouch.position.y;

        //            if (myTouch.position.y < halfScreenHeight)
        //            {
        //                if (myTouch.phase == TouchPhase.Began)
        //                {
        //                    jump = true;
        //                }
        //                else if (myTouch.phase == TouchPhase.Ended || myTouch.phase == TouchPhase.Canceled)
        //                {
        //                    jump = false;
        //                }
        //            }
        //            else
        //            {
        //                if (myTouch.phase == TouchPhase.Began)
        //                {
        //                    attack = true;
        //                }
        //                else if (myTouch.phase == TouchPhase.Ended || myTouch.phase == TouchPhase.Canceled)
        //                {
        //                    attack = false;
        //                }
        //            }


        //        }
                
                

                
        //    }
        //    else
        //    {
        //        jump = false;
        //        attack = false;
        //    }
        //}
        //else
        //{
        //    x = 0f;
        //    y = 0f;
        //}

    }


    private void UpdateJoystick(Touch myTouch)
    {
        Vector2 touchEnd = myTouch.position;
        float newX = 0f;
        if (Mathf.Abs(touchEnd.x - touchOrigin.x) > fullWidth)
        {
            if (touchEnd.x > touchOrigin.x)
            {
                touchEnd.x = touchOrigin.x + fullWidth;
            }
            else
            {
                touchEnd.x = touchOrigin.x - fullWidth;
            }
        }
       

        if (Mathf.Abs(touchEnd.x - touchOrigin.x) > deadWidth)
        {
            if (touchEnd.x > touchOrigin.x)
            {
                x = (touchEnd.x - (touchOrigin.x + deadWidth)) / (fullWidth - deadWidth);
            }
            else
            {
                x = (touchEnd.x - (touchOrigin.x - deadWidth)) / (fullWidth - deadWidth);
            }

        }
        else
        {
            x = 0f;
        }



        TouchOnePos.text = "Run%: " + x;
        //touchEnd.x = newX;
        touchEnd.y = touchOrigin.y;
        TouchVis.position = touchEnd;
    }

}

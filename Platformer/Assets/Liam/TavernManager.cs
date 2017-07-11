using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TavernManager : MonoBehaviour {

    public UnityEngine.UI.Image BlackScreenImage;
    bool fadeScreenOut = false;

	// Use this for initialization
	void Start () {
        Color c = BlackScreenImage.color;
        c.a = 0;
        BlackScreenImage.color = c;
    }
	
	// Update is called once per frame
	void Update () {
		

        if (fadeScreenOut)
        {
            Color c = BlackScreenImage.color;
            c.a += Time.deltaTime;
            BlackScreenImage.color = c;

            if (c.a >= 1.0f)
                SceneManager.LoadScene("TestScene");

        }


    }



    public void OnQuestClick()
    {
        fadeScreenOut = true;
        BlackScreenImage.raycastTarget = true;
    }

}

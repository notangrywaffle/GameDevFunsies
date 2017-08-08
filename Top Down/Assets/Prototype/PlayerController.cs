using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{

    void Start()
    {
        //This is a basic player setup... kind of messy but works
        //this.transform.rotation = Quaternion.Euler(90f, 0, 0);

        CameraController cc = Camera.main.GetComponent<CameraController>();
        cc.Player = this.gameObject;
    }

    void Update()
    {
        //if (!isLocalPlayer)
        //{
        //    return;
        //}

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Rotate(0, 0, -x);
        transform.Translate(0, z, 0);
    }
}
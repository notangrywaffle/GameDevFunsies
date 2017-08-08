using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    //Player stats
    private int attack = 100;
    private int health = 10;
    private int hitRange = 1000;


    void Start()
    {
        //This is a basic player setup... kind of messy but works
        //this.transform.rotation = Quaternion.Euler(90f, 0, 0);

        CameraController cc = Camera.main.GetComponent<CameraController>();
        cc.Player = this.gameObject;
    }

    void Update()
    {

        //// Check player instance 
        //if (!isLocalPlayer)
        //{
        //    return;
        //}

        // Check for player movement
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Rotate(0, 0, -x);
        transform.Translate(0, z, 0);

        //Check for player attack
        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
    }

    void Attack()
    {
        Debug.Log("AttemptAttack");

        RaycastHit hit;
        Vector3 forward = transform.TransformDirection(Vector3.up);
        Vector3 origin = transform.position;

        if (Physics.Raycast(origin, forward, out hit, hitRange))
        {
            if (hit.transform.gameObject != null && hit.transform.tag == "Enemy")
            {
                Debug.Log("Hit!");
                hit.transform.gameObject.SendMessage("TakeDamage", 30);
            }
        }
    }
    
}
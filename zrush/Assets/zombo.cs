using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombo : MonoBehaviour
{
    [HideInInspector]
    public GameObject target;
    [HideInInspector]
    public GameObject player;
    public bool isMove = true;
    private Rigidbody rb;
    public float speed;
    public float maxSpeed;
    [HideInInspector]
    public float coolDown;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        target = player;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            coolDown = 0;
            target = player;
            isMove = true;
        }
        else if (target != player)
        {
            isMove = false;
        }

        //transform.LookAt(target.transform);

        var lookPos = target.transform.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5);


        if (isMove && rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(transform.forward * speed * Random.Range(.7f, 1.1f));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isMove = false;
            print("hurt the player");
        }
        else if (other.tag == "placed")
        {
            target = other.gameObject;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "placed" && target == other.gameObject)
        {
            if (coolDown % 60 == 0)
            {
                other.GetComponent<placedObject>().takeDamage(5);
                print("punch box");
            }
            coolDown += 1;
        }
        if (other.tag == "wood" && target == player && isMove)
        {
            if (rb.velocity.magnitude < maxSpeed * 1.5)
            {
                rb.AddForce(transform.forward * -speed * 1.5f);
                rb.AddForce(transform.right * speed * 2);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isMove = true;
            print("chase!");
        }
    }
}

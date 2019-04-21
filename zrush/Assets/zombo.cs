using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Image healthBar;
    [HideInInspector]
    public int health = 50;

    [HideInInspector]
    public float coolDown;

    private Animator anim;
    private Canvas canva;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        target = player;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        canva = GetComponentInChildren<Canvas>();
        canva.worldCamera = Camera.main;
        canva.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            coolDown = 0;
            target = player;
            isMove = true;
            anim.SetBool("isMove", true);
        }
        else if (target != player)
        {
            isMove = false;
            anim.SetBool("isMove", false);
        }

        //transform.LookAt(target.transform);

        var lookPos = target.transform.position - transform.position;

        anim.SetBool("isMove", isMove);
        anim.SetFloat("Distance", (Vector3.Distance(target.transform.position, transform.position))/10);


        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5);


        if (isMove && rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(transform.forward * speed * Random.Range(.7f, 1.1f));
        }
    }

    public void takeDamage(int damage)
    {
        canva.gameObject.SetActive(true);
        health = health - damage;
        healthBar.rectTransform.anchorMax = new Vector2((float)health/50,.9f);
        rb.AddForce(transform.forward * -100);
        rb.AddForce(transform.up * 70);
        if (health <= 0)
        {
            isMove = false;
            StartCoroutine(timeToDie());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isMove = false;
            anim.SetTrigger("AttackPlayer");
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
                anim.SetTrigger("AttackBox");
                other.GetComponent<placedObject>().takeDamage(5);
                print("punch box");
            }
            coolDown += 1;
        }
        if ((other.tag == "wood" || other.tag == "steel" || other.tag == "enemy") && target == player && isMove)
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

    private IEnumerator timeToDie()
    {
        anim.SetTrigger("Death");
        transform.Translate(-Vector3.up * Time.deltaTime);
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}

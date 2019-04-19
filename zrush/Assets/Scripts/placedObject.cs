using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class placedObject : MonoBehaviour
{
    public int health;
    private int full_health;

    public Color lerpedColor = Color.blue;

    // Start is called before the first frame update
    void Start()
    {
        full_health = health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(int damage)
    {
        

        health = health - damage;
        

        if (health > 0)
        {
            float percent = 0f;
            percent = (float)health / full_health;
            print(health + " / " + full_health + " = " + percent);
            //lerpedColor = Color.Lerp(lerpedColor, Color.black, percent);
            GetComponent<MeshRenderer>().material.color = Color.Lerp( Color.black, lerpedColor, percent);
        }
        else
        {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            gameObject.GetComponentInChildren<ParticleSystem>().Play();
            StartCoroutine(timeToDie());
        }
    }

    private IEnumerator timeToDie()
    {
        yield return new WaitForSeconds(2f);
        //Destroy(GetComponentInParent<GameObject>());
        //Destroy(GetComponentInChildren<GameObject>());
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player : MonoBehaviour
{

    public Image[] inventory;
    private int selectedInventory = 0;
    public GameObject pickaxe;
    private bool placeMode;
    public GameObject ghost;
    public Text resource1;

    public int wood = 0;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<ParticleSystem>().emissionRate = 0;

        inventory[0].GetComponent<Outline>().enabled = true;
        for (int i = 1; i < inventory.Length; i++)
        {
            inventory[i].GetComponent<Outline>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (placeMode)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                ghost.SetActive(true);
                ghost.transform.position = hit.point;
                if (Input.GetMouseButtonDown(0) && wood >= 5)
                {
                    Instantiate(Resources.Load("PlaceableObject"), ghost.transform.position, ghost.transform.rotation);
                    wood -= 5;
                    resource1.text = "Wood: " + wood;
                }
            } else
            {
                ghost.SetActive(false);
            }
        }

        if (pickaxe.active == true)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 2.0f))
            {
                if (Input.GetMouseButtonDown(0) &&  hit.transform.tag == "wood")
                {                    
                    // Destroy(hit.transform.gameObject);
                    wood += 1;
                    resource1.text = "Wood: " + wood;

                    pickaxe.GetComponent<Animator>().SetTrigger("play");
                    pickaxe.GetComponent<AudioSource>().pitch = Random.Range(3, 5);
                    pickaxe.GetComponent<AudioSource>().Play();
                    GetComponent<ParticleSystem>().Emit(30);
                }
                if (Input.GetMouseButtonDown(0) && hit.transform.tag == "placed")
                {
                    hit.transform.gameObject.GetComponent<placedObject>().takeDamage(10);
                    wood += 1;
                    resource1.text = "Wood: " + wood;

                    pickaxe.GetComponent<Animator>().SetTrigger("play");
                    pickaxe.GetComponent<AudioSource>().pitch = Random.Range(3, 5);
                    pickaxe.GetComponent<AudioSource>().Play();
                    GetComponent<ParticleSystem>().Emit(30);
                }
            }
        }


        if (Input.mouseScrollDelta.y > 0)
        {
            inventory[selectedInventory].GetComponent<Outline>().enabled = false;
            selectedInventory++;
            if (selectedInventory > (inventory.Length - 1))
            {
                selectedInventory = 0;
            }
            inventory[selectedInventory].GetComponent<Outline>().enabled = true;
            if(selectedInventory == 0)
            {
                pickaxe.SetActive(true);
            } else
            {
                pickaxe.SetActive(false);
            }
            if(selectedInventory == 1)
            {
                ghost.SetActive(true);
                placeMode = true;
            } else
            {
                placeMode = false;
                ghost.SetActive(false);
            }

        } else if(Input.mouseScrollDelta.y < 0)
        {
            inventory[selectedInventory].GetComponent<Outline>().enabled = false;
            selectedInventory--;
            if(selectedInventory < 0)
            {
                selectedInventory = inventory.Length - 1;
            }
            inventory[selectedInventory].GetComponent<Outline>().enabled = true;
            if (selectedInventory == 0)
            {
                pickaxe.SetActive(true);
            }
            else
            {
                pickaxe.SetActive(false);
            }
            if (selectedInventory == 1)
            {
                placeMode = true;
                ghost.SetActive(true);
            }
            else
            {
                placeMode = false;
                ghost.SetActive(false);
            }
        }
    }
}

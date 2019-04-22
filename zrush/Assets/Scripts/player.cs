using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class player : MonoBehaviour
{

    public Image[] inventory;
    private int selectedInventory = 0;
    public GameObject pickaxe;
    private bool placeMode;
    private int selectedType = 1;

    public Image panel;
    public Slider healthBar;
    public int health = 100;

    public GameObject ghost;
    public Text resource1;
    public Text resource2;

    public int wood = 0;
    public int steel = 0;
    private float alpha = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<ParticleSystem>().emissionRate = 0;

        inventory[0].GetComponent<Outline>().enabled = true;
        for (int i = 1; i < inventory.Length; i++)
        {
            inventory[i].GetComponent<Outline>().enabled = false;
        }

        panel.color = new Color(1, 0, 0, alpha);
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
                if ((selectedType == 1 && wood >=5) || (selectedType == 2 && steel >= 10 && wood >=2))
                {
                    ghost.SetActive(true);
                    ghost.transform.position = hit.point;
                    if (Input.GetMouseButtonDown(0) && selectedType == 1)
                    {
                        Instantiate(Resources.Load("PlaceableWood"), ghost.transform.position, ghost.transform.rotation);
                        wood -= 5;
                        resource1.text = "Wood: " + wood;
                    }
                    else if (Input.GetMouseButtonDown(0) && selectedType == 2)
                    {
                        Instantiate(Resources.Load("PlaceableSteel"), ghost.transform.position, ghost.transform.rotation);
                        steel -= 10;
                        wood -= 2;
                        resource1.text = "Wood: " + wood;
                        resource2.text = "Steel: " + steel;
                    }
                }
                else
                {
                    ghost.SetActive(false);
                }

                //if (Input.GetMouseButtonDown(0) && wood >= 5 && selectedType == 1)
                //{
                //    Instantiate(Resources.Load("PlaceableWood"), ghost.transform.position, ghost.transform.rotation);
                //    wood -= 5;
                //    resource1.text = "Wood: " + wood;
                //}
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
                if (Input.GetMouseButtonDown(0) && hit.transform.tag == "steel")
                {
                    steel += 1;
                    resource2.text = "Steel: " + steel;

                    pickaxe.GetComponent<Animator>().SetTrigger("play");
                    pickaxe.GetComponent<AudioSource>().pitch = Random.Range(3, 5);
                    pickaxe.GetComponent<AudioSource>().Play();
                    GetComponent<ParticleSystem>().Emit(30);
                }
                if (Input.GetMouseButtonDown(0) && hit.transform.tag == "enemy")
                {
                    hit.transform.gameObject.GetComponent<zombo>().takeDamage(3);

                    pickaxe.GetComponent<Animator>().SetTrigger("play");
                    pickaxe.GetComponent<AudioSource>().pitch = Random.Range(.2f, .8f);
                    pickaxe.GetComponent<AudioSource>().Play();
                }

                if (Input.GetMouseButtonDown(0) && hit.transform.tag == "placed")
                {
                    hit.transform.gameObject.GetComponent<placedObject>().takeDamage(10);
                     
                    if (hit.transform.name.Contains("Wood"))
                    {
                        wood += 1;
                        resource1.text = "Wood: " + wood;
                    } else if (hit.transform.name.Contains("Steel"))
                    {
                        steel += 1;
                        resource2.text = "Steel: " + steel;
                    }

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
                placeMode = false;
            } else
            {
                pickaxe.SetActive(false);
            }

            if(selectedInventory == 1)
            {
                ghost.SetActive(true);
                selectedType = 1;
                placeMode = true;
            }
            else if (selectedInventory == 2)
            {
                selectedType = 2;
                ghost.SetActive(true);
                placeMode = true;
            }
            else
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
                placeMode = false;
            }
            else
            {
                pickaxe.SetActive(false);
            }

            if (selectedInventory == 1)
            {
                selectedType = 1;
                ghost.SetActive(true);
                placeMode = true;
            }
            else if (selectedInventory == 2)
            {
                selectedType = 2;
                ghost.SetActive(true);
                placeMode = true;
            }
            else
            {
                placeMode = false;
                ghost.SetActive(false);
            }
        }

        if (alpha != 0)
        {
            alpha = alpha - 0.05f;
            panel.color = new Color(1, 0, 0, alpha);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Pickup"))
        {
            if (other.name.Contains("wood"))
            {
                wood += 1;
                resource1.text = "Wood: " + wood;
            }
            if (other.name.Contains("steel"))
            {
                steel += 1;
                resource2.text = "Steel: " + steel;
            }
            Destroy(other.gameObject);
        }
    }

    public void takeDamage(int damage)
    {
        health = health - damage;
        healthBar.value = health;
        
        if (health <= 0)
        {
            StartCoroutine(timeToDie());
        }

        alpha = 0.6f;
        panel.color = new Color(1, 0, 0, alpha);
    }

    private IEnumerator timeToDie()
    {
        transform.Translate(-Vector3.up * Time.deltaTime);
        transform.Rotate(new Vector3(90,0,30));
        alpha = 0.8f;
        panel.color = new Color(.5f, 0, 0, alpha);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

}

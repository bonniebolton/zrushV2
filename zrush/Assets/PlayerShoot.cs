using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour
{
    private const string PLAYER_TAG = "Player";

    [SerializeField]
    private Camera cam;

    //reference to gunshot prefab
    public GameObject gunshot;
    //timer for gunshot sound
    float soundTime = 2.5f;

    //amount of damage the gun does
    public int damage = 25;
    //reference to muzzle flash particle system
    public ParticleSystem Muzzle;
    //reference to impact prefab
    public GameObject impactEffect;
    //float used to push back a rigidbody upon being hit by the gun
    public float impactForce = 30f;
    //float used to change firerate
    public float fireRate = 3f;
    //private float that is used to keep track of when the next shot will occur
    private float nextTimeToFire = 0f;
    //bool used to control when the combat functions are used
    [HideInInspector]
    public bool combatMode;

    zombo zombo;
    

    private void Update()
    {
        //if combatMode is on
            if (combatMode)
            {
                //if the left mouse button is being pressed and Time passes the next time to fire
                if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire)
                {
                    //the higher fireRate is, the quicker the shots happen
                    nextTimeToFire = Time.time + 1f / fireRate;
                    //calls shoot function
                    Shoot();
                }
            }
        

    }

    

    //called on server when hit something
    //takes hit point and normal
    [Command]
    void CmdBulletLand(Vector3 _pos, Vector3 _normal)
    {
        RpcDoImpactEffect(_pos, _normal);
    }

    

    //called on all CLIENTS
    //spawn the impact effect
    [ClientRpc]
    void RpcDoImpactEffect(Vector3 _pos, Vector3 _normal)
    {
        GameObject impactGO = (GameObject)Instantiate(impactEffect, _pos, Quaternion.LookRotation(_normal));
        impactGO.GetComponent<ParticleSystem>().Play();
        Destroy(impactGO, 2f);
    }
    
    void Shoot()
    {
        
        //plays the referenced muzzle flash
        Muzzle.Play();

        //instantiates a gameobject at the player position
        GameObject gunshotSource = Instantiate(gunshot, gameObject.transform.position, gameObject.transform.rotation);
        //plays the gunshot sound
        gunshotSource.GetComponent<AudioSource>().Play();
        //destroys the instantiated gunshotSource after 2 seconds
        Destroy(gunshotSource, 2f);

        RaycastHit bullet;
        //sends out a ray from the camera's position, into the forward direction for 100 meters
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out bullet, 100))
        {

            zombo zombo = bullet.transform.GetComponent<zombo>();
            if (zombo != null)
            {
                zombo.takeDamage(damage);
            }


            GameObject impactGO = (GameObject)Instantiate(impactEffect, bullet.transform.position, Quaternion.LookRotation(bullet.normal));
            impactGO.GetComponent<ParticleSystem>().Play();
            Destroy(impactGO, 2f);


        }
    }
    
}

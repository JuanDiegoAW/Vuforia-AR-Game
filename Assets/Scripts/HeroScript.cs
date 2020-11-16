using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroScript : MonoBehaviour
{
    public GameObject zombie;
    public GameObject zombieOrigin;

    private int zombiesKilledCount = 0;
    private Animator heroAnimations;
    private bool isHeroAlive = true;
    private float zombieSpeed = 0.06f;
    private int zombiesToSpawn = 1;
    private int zombiesPerLevel = 0;
    private int round = 0;
    private float maxZombieSpeed = 0.4f;
    private bool hasUsedHeart = false;

    // Start is called before the first frame update
    void Start()
    {
        heroAnimations = GetComponent<Animator>();
        GameObject zombies =  GameObject.FindWithTag("Zombie");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void KilledZombie()
    {
        zombiesKilledCount++;
        zombiesPerLevel++;
        if (zombiesKilledCount % 7 == 0 && zombieSpeed < maxZombieSpeed)
        {
            this.zombieSpeed += 0.08f;
        }
        if (zombiesPerLevel == zombiesToSpawn)
        {
            this.zombiesPerLevel = 0;
            this.round += 1;
            if (round == zombiesToSpawn)
            { 
                this.zombiesToSpawn += 1;
                round = 0;
            }
            for (int i = 0; i < this.zombiesToSpawn; i++)
            { 
                StartCoroutine(awaitToInstantiateZombie(zombieOrigin.transform.position, i));
            }         
        }
    }

    private IEnumerator awaitToInstantiateZombie(Vector3 zombieOrigin, float delay)
    {
        yield return new WaitForSeconds(1 + ((delay*9)/10));
        Instantiate(zombie, zombieOrigin, Quaternion.identity).GetComponent<ZombieScript>().setMoveSpeed(this.zombieSpeed);
    }

    public void ActivateShootAnimation(GameObject zombie)
    {
        gameObject.transform.LookAt(zombie.transform.position);
        heroAnimations.SetTrigger("ShootTrigger");
    }

    public bool GetIsHeroAlive()
    {
        return this.isHeroAlive;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Zombie")
        {           
            if (collision.collider.gameObject.GetComponent<ZombieScript>().GetIsZombieActive())
            {
                collision.gameObject.GetComponent<ZombieScript>().SetIsZombieActive(false);
                if (this.isHeroAlive)
                {
                    collision.gameObject.GetComponent<ZombieScript>().StartAttackAnimation();
                    StartCoroutine(KillPlayer());
                    this.isHeroAlive = false;
                }            
            }
        }
        else if (collision.collider.gameObject.tag == "Live" && !this.isHeroAlive && !hasUsedHeart)
        {
            GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");
            foreach (GameObject zombie in zombies)
            {
                Destroy(zombie, 2);
                zombie.GetComponent<ZombieScript>().StartKillAnimation();
            }
            this.restartStats();
            heroAnimations.SetTrigger("ReviveTrigger");
            hasUsedHeart = true;
        }
    }

    private void restartStats()
    {
        zombiesKilledCount = 0;
        isHeroAlive = true;
        zombieSpeed = 0.06f;
        zombiesToSpawn = 1;
        zombiesPerLevel = 0;
        round = 0;
        StartCoroutine(RestartZombieRespawn());
    }

    private IEnumerator RestartZombieRespawn()
    {
        yield return new WaitForSeconds(4);
        Instantiate(zombie, zombieOrigin.transform.position, Quaternion.identity).GetComponent<ZombieScript>().setMoveSpeed(this.zombieSpeed);
    }

    private IEnumerator KillPlayer()
    {
        yield return new WaitForSeconds(0.4f);
        heroAnimations.SetTrigger("DeathTrigger");
    }
}

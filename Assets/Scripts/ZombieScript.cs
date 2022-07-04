using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieScript : MonoBehaviour
{
    public int lives;
    public float minDist;
    public float moveSpeed;

    private Animator zombieAnimations;
    private GameObject hero;
    private bool isZombieActive = true;

    // Start is called before the first frame update
    void Start()
    {
        zombieAnimations = GetComponent<Animator>();
        hero = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (hero == null)
        {
            hero = GameObject.FindWithTag("Player");
            if (hero != null)
            { 
                this.StartWalkAnimation();
            }
        }
        else if (isZombieActive)
        {
            gameObject.transform.LookAt(hero.transform.position);
            if (Vector3.Distance(transform.position, hero.transform.position) >= minDist)
            {
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            }
        }
    }

    public void StartWalkAnimation()
    {
        zombieAnimations.SetTrigger("WalkTrigger");
    }

    public void StartAttackAnimation()
    {
        zombieAnimations.SetTrigger("AttackTrigger");
    }

    public void SetIsZombieActive(bool isZombieActive)
    {
        this.isZombieActive = isZombieActive;
        if (!isZombieActive)
        {
            zombieAnimations.SetTrigger("IdleTrigger");
        }
    }

    public bool GetIsZombieActive()
    {
        return this.isZombieActive;
    }

    public void setMoveSpeed(float newMoveSpeed)
    {
        this.moveSpeed = newMoveSpeed;
    }

    public void StartKillAnimation()
    {
        zombieAnimations.SetTrigger("KillTrigger");
    }

    public void TakeDamage ()
    {
        if (this.isZombieActive)
        { 
            if (lives > 0)
            {
                lives--;
            }
            else
            {
                this.isZombieActive = false;
                Destroy(gameObject, 2);
                this.StartKillAnimation();
                hero.GetComponent<HeroScript>().KilledZombie();
            }
        }
    }
}

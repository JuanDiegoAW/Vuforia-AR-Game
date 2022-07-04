using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillZombie : MonoBehaviour
{

    Ray touchVector;
    RaycastHit objectPressed;
    GameObject hero;

    // Start is called before the first frame update
    void Start()
    {
        hero = GameObject.Find("TT_demo_police");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            touchVector = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            if (Physics.Raycast(touchVector, out objectPressed))
            {
                if (objectPressed.collider.gameObject.tag=="Zombie")
                {
                    if (hero.GetComponent<HeroScript>().GetIsHeroAlive())
                    {
                        hero.GetComponent<HeroScript>().ActivateShootAnimation(objectPressed.collider.gameObject);
                        objectPressed.collider.GetComponent<ZombieScript>().TakeDamage();
                    }
                }
            }
        }
    }
}

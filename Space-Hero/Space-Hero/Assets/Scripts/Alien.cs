using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {           
            PlayerMovement.instance.AddAlien(1);
            Destroy(gameObject);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Transform playerPosition;
    public Transform respawnPosition;
    [HideInInspector]
    public bool playerHit = false;
    [HideInInspector]
    public bool Gameover = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {

        if (playerHit)
        {
            playerPosition.position = respawnPosition.position;
            playerHit = false;
        }
    }
}

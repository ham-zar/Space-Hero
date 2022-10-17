using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    Rigidbody rb;
    private bool playerIsOnTheGround = true;
    public int PlayerLife;
    public GameObject ShowGameoverUi;
    public TMP_Text livesText;


    public static PlayerMovement instance;
    public TMP_Text alienText;
    int alienAmount;
    public GameObject ShowYouWinUi;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        livesText.SetText("Lives " + PlayerLife.ToString());

        alienText.SetText("Alien : " + alienAmount.ToString());

        float horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float vertical = Input.GetAxis("Vertical") * Time.deltaTime * speed;
        
        if(!GameManager.instance.Gameover)
        {
            transform.Translate(horizontal, 0, vertical);
        }
        
        if(Input.GetButtonDown("Jump") && playerIsOnTheGround)
        {
            rb.AddForce(new Vector3(0, 5, 0), ForceMode.Impulse);
            playerIsOnTheGround = false;
        }
        if (GameObject.FindGameObjectsWithTag("Alien").Length == 0)
        {
            ShowYouWinUi.SetActive(true);
            StartCoroutine(LevelFinish());
        }

        
    }

    public void AddAlien(int alien)
    {
        alienAmount = alien + alienAmount;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if(collision.gameObject.tag == "Alien")
        //{
            
        //}
        

        if (collision.gameObject.tag == "Ground")
        {
            playerIsOnTheGround = true;
            
        }

        if (collision.gameObject.tag == "Bullet"&&  PlayerLife > 0)
        {
            PlayerLife --;
        }

        if (collision.gameObject.tag == "Plane" && PlayerLife > 0)
        {
            ShowGameoverUi.SetActive(true);
            StartCoroutine(Restartscence());
        }
        if (collision.gameObject.tag=="Enemy"&& PlayerLife > 0)
        {
            PlayerLife--;
        }

        else if (PlayerLife <= 0)
        {
           GameManager.instance.Gameover =true;
            ShowGameoverUi.SetActive(true);
            StartCoroutine(Restartscence());
        }
        
    }
    IEnumerator Restartscence()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(0);
        GameManager.instance.Gameover = false;
    }
    IEnumerator LevelFinish()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(0);
    }
}

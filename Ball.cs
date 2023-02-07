using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text highScoreText;

    [SerializeField]
    private SpriteRenderer sr;
    [SerializeField]
    private Sprite halfBroken;
    [SerializeField]
    private Sprite almostBroken;
    [SerializeField]
    private GameObject brick;


    public BreakOutManager manager;

    private bool isWon;
    private int score = 0;
    private float speed;

    void Start()
    {
        isWon = false;
        speed = -5f;
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.up * speed;

        highScoreText.text = "High Score: "+ PlayerPrefs.GetInt("BreakOutHighScore", 0).ToString();
    }

    void Update()
    {
        brick.SetActive(true);
        scoreText.text = "Score: " + score.ToString();

        if (score > PlayerPrefs.GetInt("BreakOutHighScore", 0))
        {
            PlayerPrefs.SetInt("BreakOutHighScore", score);
            highScoreText.text = "High Score: " + score.ToString();
        }

        if(score >= 10 && score < 20)
        {
            sr.sprite = halfBroken;
        }
        else if(score >= 20 && score < 30)
        {
            sr.sprite = almostBroken;
        }
        else if(score >= 30 && isWon == false)
        {
            brick.SetActive(false);
            rb.velocity = Vector3.zero;
            manager.GameWon();
            isWon = true;
            
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Brick"))
        {
            score++;
            speed -= -0.2f;
        }

        if (other.gameObject.CompareTag("Paddle"))
        {
            FindObjectOfType<AudioManager>().Play("HitPaddle");
        }

        if (other.gameObject.CompareTag("Brick"))
        {
            FindObjectOfType<AudioManager>().Play("HitBrick");
        }

        if (other.gameObject.CompareTag("Ground"))
        {
            rb.velocity = Vector3.zero;
            manager.GameOver();
            
        }
    }
}


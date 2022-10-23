using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;
    public float speed;
    public Text score;
    public Text health;
    public GameObject WinText;
    public GameObject LoseText;
    public GameObject player;
    private int scoreValue = 0;
    private int lifeValue = 3;
    private int hozMovement = 0;
    private bool facingRight = true;
    public AudioClip LevelMusic;
    public AudioClip VictorMusic;
    public AudioSource musicSource;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = scoreValue.ToString();
        health.text = lifeValue.ToString();
        WinText.SetActive(false);
        LoseText.SetActive(false);
        musicSource.clip = LevelMusic;
        musicSource.Play();
        musicSource.loop = true;
        anim = GetComponent<Animator>();
        anim.SetInteger("Walk", 0);
        anim.SetInteger("Jump", 0);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetInteger("Walk", 1);
            hozMovement = 1;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetInteger("Walk", 0);
            hozMovement = 0;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetInteger("Walk", 1);
            hozMovement = -1;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetInteger("Walk", 0);
            hozMovement = 0;
        }
        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }
    }
    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
        if (lifeValue == 0)
        {
            LoseText.SetActive(true);
            player.SetActive(false);
        }        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = scoreValue.ToString();
            Destroy(collision.collider.gameObject);
        }
        if (collision.collider.tag == "Enemy")
        {
            lifeValue = lifeValue - 1;
            health.text = lifeValue.ToString();
            Destroy(collision.collider.gameObject);
        }
        if (scoreValue == 4)
        {
            transform.position = new Vector2(105.0f, 1.0f);
            lifeValue = 3;
            health.text = lifeValue.ToString();
        }
        if (scoreValue == 8)
        {
            WinText.SetActive(true);
            musicSource.Stop();
            musicSource.clip = VictorMusic;
            musicSource.Play();
            musicSource.loop = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            anim.SetInteger("Jump", 0);
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
                anim.SetInteger("Jump", 1);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private float hSpeed = 10f;
    [SerializeField] private float vSpeed = 6f;
    private Rigidbody2D rb2D;
    [SerializeField] private bool canMove = true;
    private bool facingRight = false;
    [Range(0, 1.0f)]
    [SerializeField] float movementSmoothing = 0f;
    private Vector3 velocity = Vector3.zero;
    Animator animator;
    SpriteRenderer sr;
    CircleCollider2D ac;
    [SerializeField] Camera cam;
    private bool canBeHit = true;
    [SerializeField] public int hp = 7;
    [SerializeField] SpriteRenderer hud;

    [SerializeField] public Sprite h1;
    [SerializeField] public Sprite h2;
    [SerializeField] public Sprite h3;
    [SerializeField] public Sprite h4;
    [SerializeField] public Sprite h5;
    [SerializeField] public Sprite h6;
    [SerializeField] public Sprite h7;

    public CircleCollider2D enemySword;
    [SerializeField] CapsuleCollider2D playerWeakPoint;
    private bool dead = false;

    private void Awake(){
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        //cam = GetComponent<Camera>();
        ac = GetComponent<CircleCollider2D>();
        playerWeakPoint = GetComponent<CapsuleCollider2D>();
    }
    
    public void Move(float hMove, float vMove, bool jump){
        if(canMove){
            Vector3 targetVelocity = new Vector2(hMove * hSpeed, vMove * vSpeed);
            rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, targetVelocity, ref velocity, movementSmoothing);
            
            if(hMove != 0 || vMove != 0){
                animator.SetBool("isMoving", true);
            }else{
                animator.SetBool("isMoving", false);
            }

            if (hMove > 0 && !facingRight){
                flip();
            }else if(hMove < 0 && facingRight){
                flip();
            }
        }
    }

    void Update(){
        cam.transform.position = new Vector3(cam.transform.position.x, 1, cam.transform.position.z);
    }

    private void flip(){
        facingRight = !facingRight;
        sr.flipX = facingRight;
        if(facingRight){
            ac.offset = new Vector2(0.09f, ac.offset.y);
        }else{
            ac.offset = new Vector2(-0.09f, ac.offset.y);
        }
    }

    public void Attack(){
        animator.SetTrigger("attack");
    }

    // void FixedUpdate(){
    //     if(playerWeakPoint.IsTouching(enemySword) && canBeHit){
    //         StartCoroutine(GetDamaged());
    //     }
    // }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.name == "Entity" && other.isTrigger && other.GetType().ToString() == "UnityEngine.CircleCollider2D" && canBeHit){
            StartCoroutine(GetDamaged());
        }
    }

    IEnumerator GetDamaged(){
        canBeHit = false;
        hp -= 1;
        //Debug.Log("Player health: " + hp);
        switch(hp){
            case 6:
                hud.sprite = h1;
            break;
            case 5:
                hud.sprite = h2;
            break;
            case 4:
                hud.sprite = h3;
            break;
            case 3:
                hud.sprite = h4;
            break;
            case 2:
                hud.sprite = h5;
            break;
            case 1:
                hud.sprite = h6;
            break;
            case 0:
                hud.sprite = h7;
                animator.SetTrigger("death");
                dead = true;
            break;
            default:
            break;
        }
        sr.color = new Color(1f,1f,1f,.5f);
        yield return new WaitForSeconds(1);
        if(dead){
            string scene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }
        sr.color = new Color(1f,1f,1f,1f);
        canBeHit = true;
    }
}
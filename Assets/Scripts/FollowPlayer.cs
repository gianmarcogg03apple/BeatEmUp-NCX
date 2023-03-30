using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform target;
    public Rigidbody2D rb;
    public Vector2 movement;
    public float moveSpeed = 5f;
    public PolygonCollider2D rangeColl;
    public Animator animator;
    public SpriteRenderer sr;
    public CircleCollider2D ac;

    bool isTargetInRange = false;
    private bool facingRight = false;
    private bool attacking = false;

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
        rangeColl = GetComponent<PolygonCollider2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        ac = GetComponent<CircleCollider2D>();
    }
 
    void Update(){
        if(!attacking){
            Vector3 direction = target.position - transform.position;
            direction.Normalize();
            movement = direction;
            float range = target.position.x - transform.position.x;
            Debug.Log(range);
            if(range >= -2 && range <= 2 && !attacking){
                StartCoroutine(Attack());
            }
        }
    }

    IEnumerator Attack(){
        attacking = true;
        animator.SetTrigger("attack");
        yield return new WaitForSeconds(2);
        attacking = false;
    }

    void MoveCharacter(Vector2 direction){
        rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
        
        if(direction.x > 0 && facingRight){
            flip();
        }else if(direction.x < 0 && !facingRight){
            flip();
        }
    }

    void FixedUpdate(){
        if(isTargetInRange){
            MoveCharacter(movement);
        }
    }

    private void flip(){
        facingRight = !facingRight;
        sr.flipX = facingRight;
        if(facingRight){
            ac.offset = new Vector2(-0.09f, ac.offset.y);
        }else{
            ac.offset = new Vector2(0.09f, ac.offset.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Player"){
            isTargetInRange = true;
            animator.SetBool("isMoving", true);
            //Debug.Log("Player in range!!!!!!!");
        }else{
            isTargetInRange = false;
        }
    }

    private void OnTriggerStay2D(Collider2D other){
        if(other.tag == "Player"){
            isTargetInRange = true;
            animator.SetBool("isMoving", true);
            //Debug.Log("Player is still in range.");
        }
    }

    private void OnTriggerExit2D(Collider2D other){
        if(other.tag == "Player"){
            isTargetInRange = false;
            animator.SetBool("isMoving", false);
            //Debug.Log("Player no longer in range.");
        }
    }
}
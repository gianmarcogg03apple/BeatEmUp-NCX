using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] CircleCollider2D playerSword;
    [SerializeField] CapsuleCollider2D enemyWeakPoint;
    private bool canBeHit = true;
    [SerializeField] public int hp = 3;
    [SerializeField] SpriteRenderer hud;
    SpriteRenderer sr;
    public Animator animator;

    [SerializeField] public Sprite h1;
    [SerializeField] public Sprite h2;
    [SerializeField] public Sprite h3;

    [SerializeField] bool dead = false;

    void Awake(){
        enemyWeakPoint = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate(){
        if(enemyWeakPoint.IsTouching(playerSword) && canBeHit){
            StartCoroutine(GetDamaged());
        }
    }

    IEnumerator GetDamaged(){
        canBeHit = false;
        hp -= 1;
        //Debug.Log("Enemy health: " + hp);
        switch(hp){
            case 2:
                hud.sprite = h1;
            break;
            case 1:
                hud.sprite = h2;
            break;
            case 0:
                hud.sprite = h3;
                animator.SetTrigger("death");
            break;
            default:
            break;
        }
        sr.color = new Color(1f,1f,1f,.5f);
        yield return new WaitForSeconds(1);
        if(dead){
            Destroy(gameObject);
        }
        sr.color = new Color(1f,1f,1f,1f);
        canBeHit = true;
    }

    // public void Attack(){
    //     animator.SetTrigger("attack");
    // }
}

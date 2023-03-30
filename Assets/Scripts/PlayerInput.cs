using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Player player;
    float horizontalMove;
    float verticalMove;
    bool attack;

    private void Awake(){
        player = GetComponent<Player>();
    }

    void Update(){
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");

        if (Input.GetKeyDown("space")){
            player.Attack();
        }
    }

    void FixedUpdate(){
        player.Move(horizontalMove, verticalMove, false);
    }
}

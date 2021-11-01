using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;

    private float verticalMovementInput;
    private float horizontalMovementInput;
    private Rigidbody2D rb2d;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void GetMovementInput()
    {
        verticalMovementInput = Input.GetAxisRaw("Vertical");
        horizontalMovementInput = Input.GetAxisRaw("Horizontal");
    }

    // Update is called once per frame
    void Update()
    {
        GetMovementInput();
        var position = transform.position;
        if (verticalMovementInput != 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 90 * verticalMovementInput);
        }else if (horizontalMovementInput != 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 90 - 90 * horizontalMovementInput);
        }
        
        rb2d.velocity = new Vector2(horizontalMovementInput, verticalMovementInput) * (speed * Time.deltaTime);
    }

    void PlayerDied()
    {
        GameMaster.Instance.EndGame();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            PlayerDied();
        }else if (other.CompareTag("End"))
        {
            GameMaster.Instance.PlayerWon();
        }
    }

}

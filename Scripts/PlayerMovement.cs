using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;

    private float verticalMovementInput;
    private float horizontalMovementInput;
    private Rigidbody2D rb2d;
    private Health hp;

    
    
    
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        hp = GetComponent<Health>();
        
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

    void PlayerHit()
    {
        hp.currentHealth -= 1;
        UpdateHpText();
        if (hp.currentHealth <= 0)
        {
            PlayerDied();
        }
    }

    public void PlayerHealthUp()
    {
        hp.currentHealth += 1;
        UpdateHpText();
    }

    public void UpdateHpText()
    {
        GameMaster.Instance.hpText.SetText("Player HP: " + hp.currentHealth);
    }

    void PlayerDied()
    {
        GameMaster.Instance.EndGame();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var gameMaster = GameMaster.Instance;
        if (other.CompareTag("Enemy"))
        {
            PlayerHit();
            gameMaster.enemies.Remove(other.gameObject);
            Destroy(other.gameObject);
        }else if (other.CompareTag("End"))
        {
            gameMaster.PlayerWon();
        }else if (other.CompareTag("Potion"))
        {
            if (gameMaster.eqManager.GetNumberOfPotions() >= 3) return;
            gameMaster.PickedUpPotion(other.gameObject);
            Destroy(other.gameObject);
            
        }
    }

}

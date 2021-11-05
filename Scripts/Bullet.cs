using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb2d;

    private void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector2.right) * speed;
        StartCoroutine(FadeAway());
    }

    IEnumerator FadeAway()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            GameMaster.Instance.enemies.Remove(other.gameObject);
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }
}

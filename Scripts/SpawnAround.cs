using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAround : MonoBehaviour
{

    public GameObject EnemyPrefab;
    public float spawnDelay;
    
    // Start is called before the first frame update
    void Start()
    {
        EnemyPrefab = (GameObject) Resources.Load("Prefabs/Moving/Enemy");
        StartCoroutine(SpawnEnemy());
    }
    
    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            var randomPosition = Random.Range(0, 4);
            Vector2 addOffset = Vector2.zero;
            
            if (randomPosition == 0)
            {
                addOffset = Vector2.down;
            }
            else if(randomPosition == 1)
            {
                addOffset = Vector2.up;
            }else if(randomPosition == 2)
            {
                addOffset = Vector2.left;
            }else if(randomPosition == 3)
            {
                addOffset = Vector2.right;
            }
            
            Instantiate(EnemyPrefab, (Vector2) transform.position + addOffset, Quaternion.identity);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}

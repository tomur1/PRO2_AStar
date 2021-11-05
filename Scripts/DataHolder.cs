using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHolder
{
    public int playerHealth;
    public int playerPotions;
    public List<int[]> maze;
    public List<Vector3> enemyPositions;
    public Vector3 playerPosition;
    
    public void GetDataToSave()
    {
        var gameMaster = GameMaster.Instance;
        
        playerHealth = gameMaster.playerObject.GetComponent<Health>().currentHealth;
        playerPotions = gameMaster.eqManager.GetNumberOfPotions();
        maze = gameMaster.maze;
        
        enemyPositions = new List<Vector3>();
        foreach (var enemy in gameMaster.enemies)
        {
            enemyPositions.Add(enemy.transform.position);
        }

        playerPosition = gameMaster.playerObject.transform.position;
    }
}

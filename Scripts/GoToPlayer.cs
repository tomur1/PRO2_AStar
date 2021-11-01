using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class GoToPlayer : MonoBehaviour
{
    private Vector2 nextPosToGo;
    private Vector2 lastPos;
    private float progress;
    public float speed;

    private Rigidbody2D rb2d;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        progress = -1f;
    }

    // Update is called once per frame
    void Update()
    {
        var gameMaster = GameMaster.Instance;
        if (progress >= 1 || progress < 0)
        {
            var from = gameMaster.GetMazeCoord(transform.position);
            var to = gameMaster.GetMazeCoord(gameMaster.playerObject.transform.position);
            var path = Pathfinding.Instance.FindPath(from, to);

            nextPosToGo = gameMaster.GetWorldPositionForUnit((int) path[1].x, (int) path[1].y);
            lastPos = gameMaster.GetWorldPositionForUnit((int) path[0].x, (int) path[0].y);
            progress = 0;
        }
        
        progress += Time.deltaTime * speed;
        // gameMaster.TestPath(gameObject, gameMaster.playerObject);
        rb2d.MovePosition(Vector2.Lerp(lastPos, nextPosToGo, progress));
        
    }
}
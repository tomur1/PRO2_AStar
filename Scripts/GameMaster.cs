using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class GameMaster : MonoBehaviour
{
    public static GameMaster Instance;
    
    // Start is called before the first frame update
    public int width;
    public int height;
    public int numberOfSpawners;
    public List<int[]> maze;
    private GameObject mazeObject;

    public GameObject WallPrefab;
    public GameObject StartPrefab;
    public GameObject EndPrefab;
    public GameObject SpawnerPrefab;
    
    public GameObject StartDebug;
    public GameObject EndDebug;
    public GameObject DebugStep;
    private List<GameObject> debugSteps;

    [SerializeField] GameObject playerPrefab;
    public GameObject playerObject;

    public TextMeshProUGUI gameOverText;

    private Dictionary<Vector2, GameObject> mazeElements;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        maze = TxtMazeConverter.ConvertToArray(Application.dataPath + "\\maze.txt");
        debugSteps = new List<GameObject>();
        mazeElements = new Dictionary<Vector2, GameObject>();
        height = maze.Count;
        width = maze[0].Length;
        GenerateMazeObjects();
        new Pathfinding();
    }

    void GenerateMazeObjects()
    {
        //Create walls from txt file
        mazeObject = new GameObject("Maze");
        for (var i = 0; i < maze.Count; i++)
        {
            var list = maze[i];
            for (var j = 0; j < list.Length; j++)
            {
                var value = list[j];
                if (value == 1)
                {
                    mazeElements.Add(new Vector2(i,j), Instantiate(WallPrefab, new Vector3(j + 0.5f,  (height - i) - 0.5f, 0), Quaternion.identity, mazeObject.transform));
                }else if (value == 2)
                {
                    mazeElements.Add(new Vector2(i,j), Instantiate(StartPrefab, new Vector3(j + 0.5f,  (height - i) - 0.5f, 0), Quaternion.identity, mazeObject.transform));
                    playerObject = Instantiate(playerPrefab, new Vector3(j + 0.5f, (height - i) - 0.5f),
                        Quaternion.identity);
                }else if (value == 3)
                {
                    mazeElements.Add(new Vector2(i,j), Instantiate(EndPrefab, new Vector3(j + 0.5f,  (height - i) - 0.5f, 0), Quaternion.identity, mazeObject.transform));
                }
            }
        }

        GenerateSpawners();
    }
 
    void GenerateSpawners()
    {
        List<Vector2> takenPlaces = new List<Vector2>();
        var rand = new Random();
        for (int i = 0; i < numberOfSpawners; i++)
        {
            // find a place
            Vector2 coordToTake = new Vector2();
            do
            {
                var x = rand.Next(2, width - 3);
                var y = rand.Next(2, height - 3);
                coordToTake = new Vector2(x, y);
            } while (takenPlaces.Contains(coordToTake));

            var coordsAround = CoordsAroundPoint(coordToTake);
            foreach (var coordinateToClear in coordsAround)
            {
                var coordinateToClearInt = Vector2Int.FloorToInt(coordinateToClear);
                maze[coordinateToClearInt.x][coordinateToClearInt.y] = 0;
                if (mazeElements.ContainsKey(coordinateToClearInt))
                {
                    Destroy(mazeElements[coordinateToClearInt]);
                    mazeElements.Remove(coordinateToClearInt);
                }
            }
            takenPlaces.Add(coordToTake);
            mazeElements.Add(new Vector2(coordToTake.x,coordToTake.y), Instantiate(SpawnerPrefab, new Vector3(coordToTake.y + 0.5f,  (height - coordToTake.x) - 0.5f, 0), Quaternion.identity, mazeObject.transform));
            maze[(int) coordToTake.x][(int) coordToTake.y] = 1;
        }
    }

    public void DebugPath()
    {
        TestPath(StartDebug, EndDebug);
    }

    public void TestPath(GameObject fromObj, GameObject toObj)
    {
        var from = GetMazeCoord(fromObj.transform.position);
        var to = GetMazeCoord(toObj.transform.position);
        var path= Pathfinding.Instance.FindPath(from, to);
        
        foreach (var step in debugSteps)
        {
            Destroy(step);
        }
        debugSteps.Clear();
        
        if (path == null)
        {
            Debug.Log("No path found");
            return;
        }
        
        foreach (var coordToTake in path)
        {
            debugSteps.Add(Instantiate(DebugStep, new Vector3(coordToTake.y + 0.5f, (height - coordToTake.x) - 0.5f, 0),
                Quaternion.identity, mazeObject.transform));
        }
    }

    public List<Vector2> CoordsAroundPoint(Vector2 point)
    {
        List<Vector2> coordsAround = new List<Vector2>();
        coordsAround.Add(point);
        coordsAround.Add(point + new Vector2(-1,1));
        coordsAround.Add(point + new Vector2(0,1));
        coordsAround.Add(point + new Vector2(1,1));
        coordsAround.Add(point + new Vector2(1,0));
        coordsAround.Add(point - new Vector2(-1,1));
        coordsAround.Add(point - new Vector2(0,1));
        coordsAround.Add(point - new Vector2(1,1));
        coordsAround.Add(point - new Vector2(1,0));
        return coordsAround;
    }

    public Vector2 GetWorldPosition(int x, int y)
    {
        return new Vector2(y, height - x);
    }
    
    public Vector2 GetWorldPositionForUnit(int x, int y)
    {
        return new Vector2(y + 0.5f, (height - x) - 0.5f);
    }
    
    public Vector2 GetMazeCoord(Vector2 worldPosition)
    {
        return new Vector2((width - worldPosition.y), worldPosition.x);
    }
    
    public void ShowLines()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Debug.DrawLine(GetWorldPosition(i,j), GetWorldPosition(i,j+1), Color.blue, Time.deltaTime);
                Debug.DrawLine(GetWorldPosition(i,j), GetWorldPosition(i+1,j), Color.blue, Time.deltaTime);
                var cell = maze[i][j];
                if (cell != 0)
                {
                    Debug.DrawLine(GetWorldPosition(i,j), GetWorldPosition(i+1,j+1), Color.red, Time.deltaTime);
                    Debug.DrawLine(GetWorldPosition(i,j+1), GetWorldPosition(i+1,j), Color.red, Time.deltaTime);
                }
            }
        }
        Debug.DrawLine(GetWorldPosition(0,height), GetWorldPosition(width,height), Color.blue, Time.deltaTime);
        Debug.DrawLine(GetWorldPosition(width,0), GetWorldPosition(width,height), Color.blue, Time.deltaTime);
    }

    private void PrintMaze(List<int[]> maze)
    {
        foreach (var list in maze)
        {
            foreach (var value in list)
            {
                Console.Write(value);
            }
            Console.WriteLine();
        }
    }

    // Update is called once per frame
    void Update()
    {
        ShowLines();
    }

    public void EndGame()
    {
        gameOverText.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void PlayerWon()
    {
        gameOverText.SetText("YOU WIN");
        EndGame();
    }
}

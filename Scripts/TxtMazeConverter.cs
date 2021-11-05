using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = System.Random;

//0 is empty
//1 is wall
//2 is start
//3 is end
//4 is spawner
//5 is potion

public class TxtMazeConverter
{
    public static List<int[]> ConvertToArray(string pathToMazeFile)
    {
        List<int[]> convertedMaze = new List<int[]>();
        using (StreamReader sr = new StreamReader(pathToMazeFile))
        {
            int i = 0;
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                convertedMaze.Add(new int[line.Length]);
                for (var index = 0; index < line.Length; index++)
                {
                    var character = (int) line[index];
                    if (character == 9608)
                    {
                        //Wall
                        convertedMaze[i][index] = 1;
                    }else if (character == 32)
                    {
                        //Empty space
                        convertedMaze[i][index] = 0;
                    }
                    else if (character == 79)
                    {
                        // O is start
                        convertedMaze[i][index] = 2;
                    }
                    else if (character == 88)
                    {
                        // X is end
                        convertedMaze[i][index] = 3;
                    }
                    else
                    {
                        throw new Exception("Wrong content of maze file");
                    }
                }

                i++;
            }
            
            
        }

        return convertedMaze;
    }

    public static void AddSpawners(List<int[]> maze, int numberOfSpawners)
    {
        var gameMaster = GameMaster.Instance;
        
        List<Vector2> takenPlaces = new List<Vector2>();
        var rand = new Random();
        for (int i = 0; i < numberOfSpawners; i++)
        {
            // find a place
            Vector2 coordToTake = new Vector2();
            do
            {
                var x = rand.Next(2, gameMaster.width - 3);
                var y = rand.Next(2, gameMaster.height - 3);
                coordToTake = new Vector2(x, y);
            } while (takenPlaces.Contains(coordToTake));

            var coordsAround = gameMaster.CoordsAroundPoint(coordToTake);
            foreach (var coordinateToClear in coordsAround)
            {
                var coordinateToClearInt = Vector2Int.FloorToInt(coordinateToClear);
                maze[coordinateToClearInt.x][coordinateToClearInt.y] = 0;
            }

            takenPlaces.Add(coordToTake);
            maze[(int) coordToTake.x][(int) coordToTake.y] = 4;
        }
    }

    public static void AddPotions(List<int[]> maze, int numberOfPotions)
    {
        var gameMaster = GameMaster.Instance;
        List<Vector2> takenPlaces = new List<Vector2>();
        var rand = new Random();
        for (int i = 0; i < numberOfPotions; i++)
        {
            // find a place
            Vector2 coordToTake = new Vector2();
            do
            {
                var x = rand.Next(1, gameMaster.width - 2);
                var y = rand.Next(1, gameMaster.height - 2);
                coordToTake = new Vector2(x, y);
            } while (takenPlaces.Contains(coordToTake) || maze[(int) coordToTake.x][(int) coordToTake.y] != 0);

            maze[(int) coordToTake.x][(int) coordToTake.y] = 5;
            takenPlaces.Add(coordToTake);
            // Instantiate(PotionPrefab, new Vector3(coordToTake.y + 0.5f, (gameMaster.height - coordToTake.x) - 0.5f, 0),
            //     Quaternion.identity, mazeObject.transform);
        }
    }
}

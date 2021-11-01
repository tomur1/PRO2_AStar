using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//0 is empty
//1 is wall
//2 is start
//3 is end
//4 is spawner
//5 is player
//6 is enemy

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
}

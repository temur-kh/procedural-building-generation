using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public bool visited = false;
    public int roomIdx;
    public int floorIdx;
    public int size;
    public int rightCornerPos;
    public int leftCornerPos;
    public bool doorEnding = false;
    public HashSet<int> doorPositions;
}

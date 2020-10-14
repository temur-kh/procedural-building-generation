using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    public static void build(Building building, GameObject door, GameObject door2, GameObject wall)
    {
        const float roomStep = 1.0f;
        const float separatorShift = 0.5f;
        const float floorStart = 4.75f;
        const float floorStep = 2.0f;

        for (int floorIdx = 0; floorIdx < building.floors.Count; floorIdx++)
        {
            Floor curFloor = building.floors[floorIdx];
            for (int roomIdx = 0; roomIdx < curFloor.rooms.Count; roomIdx++)
            {
                Room curRoom = curFloor.rooms[roomIdx];
                if (roomIdx == 0 && floorIdx == 0)
                {
                    Instantiate(door2, new Vector3(-2 * separatorShift, floorStart + floorIdx * floorStep, (curFloor.rightCornerPos - 1) * roomStep + separatorShift), Quaternion.Euler(0, 90, 0));
                } else if (roomIdx == 0)
                {
                    Instantiate(wall, new Vector3(-roomStep + separatorShift, floorStart + floorIdx * floorStep, (curFloor.rightCornerPos - 1) * roomStep + separatorShift), Quaternion.Euler(0, 90, 0));
                    Instantiate(wall, new Vector3(-2 * roomStep + separatorShift, floorStart + floorIdx * floorStep, (curFloor.rightCornerPos - 1) * roomStep + separatorShift), Quaternion.Euler(0, 90, 0));
                }
                for (int pos = curRoom.rightCornerPos; pos < curRoom.leftCornerPos; pos++)
                {
                    if (curRoom.doorPositions.Contains(pos))
                    {
                        Instantiate(door, new Vector3(0, floorStart + floorIdx * floorStep, pos * roomStep), Quaternion.identity);
                    } else
                    {
                        Instantiate(wall, new Vector3(0, floorStart + floorIdx * floorStep, pos * roomStep), Quaternion.identity);
                    }
                    Instantiate(wall, new Vector3(0, floorStart + floorIdx * floorStep, pos * roomStep), Quaternion.Euler(0, 0, 90));
                    Instantiate(wall, new Vector3(0, floorStart + (floorIdx + 1) * floorStep, pos * roomStep), Quaternion.Euler(0, 0, 90));
                }
                if (curRoom.doorEnding)
                {
                    Instantiate(door2, new Vector3(-2 * separatorShift, floorStart + floorIdx * floorStep, (curRoom.leftCornerPos - 1) * roomStep + separatorShift), Quaternion.Euler(0, 90, 0));
                } else
                {
                    Instantiate(wall, new Vector3(-roomStep + separatorShift, floorStart + floorIdx * floorStep, (curRoom.leftCornerPos - 1) * roomStep + separatorShift), Quaternion.Euler(0, 90, 0));
                    Instantiate(wall, new Vector3(-2 * roomStep + separatorShift, floorStart + floorIdx * floorStep, (curRoom.leftCornerPos - 1) * roomStep + separatorShift), Quaternion.Euler(0, 90, 0));
                }
            }
        }
    }
}

 using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GraphGenerator
{
    // Start is called before the first frame update
    public static Building generate(Config conf)
    {
        Building building = new Building();
        building.floors = new List<Floor>();
        for (int j = 0; j < conf.numberOfFloors; j++) building.floors.Add(new Floor());

        // create building structure
        for (int i = 0; i < conf.numberOfFloors; i++)
        {
            // define parameters of floors
            Floor floor = building.floors[i];
            if (i == 0)
            {
                floor.rightCornerPos = 0;
            } else
            {
                Floor prevFloor = building.floors[i - 1];
                int shift = Random.Range(-conf.floorShift, conf.floorShift + 1);
                floor.rightCornerPos = prevFloor.rightCornerPos + shift;
            }
            floor.size = Random.Range(conf.minFloorSize, conf.maxFloorSize + 1);
            floor.leftCornerPos = floor.rightCornerPos + floor.size;

            // create rooms and define parameters
            int numberOfRooms = Random.Range(Mathf.CeilToInt((float) floor.size / (float) conf.maxRoomSize), Mathf.FloorToInt((float) floor.size / (float) conf.minRoomSize + 1));
            floor.rooms = new List<Room>();
            for (int j = 0; j < numberOfRooms; j++) floor.rooms.Add(new Room());
            int positionsCnt = floor.size;
            foreach (Room room in floor.rooms)
            {
                room.size = conf.minRoomSize;
                positionsCnt -= conf.minRoomSize;
            }
            while (positionsCnt > 0)
            {
                int randomRoom = Random.Range(0, numberOfRooms);
                if (floor.rooms[randomRoom].size < conf.maxRoomSize)
                {
                    floor.rooms[randomRoom].size++;
                    positionsCnt--;
                }
            }
            for (int j = 0; j < numberOfRooms; j++)
            {
                floor.rooms[j].roomIdx = j;
                floor.rooms[j].floorIdx = i;
                if (j == 0)
                {
                    floor.rooms[j].rightCornerPos = floor.rightCornerPos;
                } else
                {
                    floor.rooms[j].rightCornerPos = floor.rooms[j - 1].leftCornerPos;
                }
                floor.rooms[j].leftCornerPos = floor.rooms[j].rightCornerPos + floor.rooms[j].size;
                floor.rooms[j].doorPositions = new HashSet<int>();
            }
        }

        dfs(building, 0, 0, conf);

        return building;
    }

    static void dfs(Building building, int floorIdx, int roomIdx, Config conf)
    {
        Floor curFloor = building.floors[floorIdx];
        Room curRoom = curFloor.rooms[roomIdx];
        curRoom.visited = true;

        List<Room> neighbors = new List<Room>();
        if (roomIdx < curFloor.rooms.Count - 1)
        {
            neighbors.Add(curFloor.rooms[roomIdx + 1]);
        }
        if (roomIdx > 0)
        {
            neighbors.Add(curFloor.rooms[roomIdx - 1]);
        }
        if (floorIdx < building.floors.Count - 1)
        {
            foreach (Room room in building.floors[floorIdx+1].rooms)
            {
                if (Mathf.Min(curRoom.leftCornerPos, room.leftCornerPos)-2 >= Mathf.Max(curRoom.rightCornerPos, room.rightCornerPos) + 1)
                {
                    neighbors.Add(room);
                }
            }
        }
        neighbors = neighbors.OrderBy(x => Random.Range(0.0f, 1.0f)).ToList();
        for (int i = 0; i < neighbors.Count; i++)
        {
            Room nextRoom = neighbors[i];
            if (!nextRoom.visited || Random.Range(0.0f, 1.0f) < conf.cycleProbability)
            {
                if (nextRoom.floorIdx == curRoom.floorIdx && nextRoom.roomIdx == curRoom.roomIdx + 1)
                {
                    curRoom.doorEnding = true;
                    if (!nextRoom.visited) dfs(building, floorIdx, roomIdx + 1, conf);
                } else if (nextRoom.floorIdx == curRoom.floorIdx && nextRoom.roomIdx == curRoom.roomIdx - 1) {
                    nextRoom.doorEnding = true;
                    if (!nextRoom.visited) dfs(building, floorIdx, roomIdx - 1, conf);
                }
                else
                {
                    List<int> potentialDoorPositions = new List<int>();
                    for (int j = Mathf.Max(curRoom.rightCornerPos, nextRoom.rightCornerPos) + 1; j < Mathf.Min(curRoom.leftCornerPos, nextRoom.leftCornerPos) - 1; j++)
                    {
                        if (!curRoom.doorPositions.Contains(j) && !nextRoom.doorPositions.Contains(j))
                        {
                            potentialDoorPositions.Add(j);
                        }
                    }
                    if (potentialDoorPositions.Count > 0)
                    {
                        int doorPos = potentialDoorPositions[Random.Range(0, potentialDoorPositions.Count)];
                        curRoom.doorPositions.Add(doorPos);
                        nextRoom.doorPositions.Add(doorPos);
                        if (!nextRoom.visited) dfs(building, floorIdx + 1, nextRoom.roomIdx, conf);
                    }
                }
            }
        }
        
    }

}

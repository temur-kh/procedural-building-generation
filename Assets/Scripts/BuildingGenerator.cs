using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGenerator : MonoBehaviour
{
    public int seed = 42;
    public int numberOfFloors = 3;
    public int minFloorSize = 5;
    public int maxFloorSize = 10;
    public int minRoomSize = 3;
    public int maxRoomSize = 5;
    public int floorShift = 1;
    public float cycleProbability = 0.25f;

    public GameObject door;
    public GameObject door2;
    public GameObject wall;

    // Start is called before the first frame update
    void Start()
    {
        Run();
    }

    void Run()
    {
        Random.InitState(seed);
        Config conf = new Config();
        conf.numberOfFloors = numberOfFloors;
        conf.minFloorSize = minFloorSize;
        conf.maxFloorSize = maxFloorSize;
        conf.minRoomSize = minRoomSize;
        conf.maxRoomSize = maxRoomSize;
        conf.floorShift = floorShift;
        conf.cycleProbability = cycleProbability;

        Building building = GraphGenerator.generate(conf);
        Builder.build(building, door, door2, wall);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungenGenerator : MonoBehaviour
{
    
    public DungenGenerationData dungenGenerationData;
    public List<Vector2Int> dungeonRooms;
    private void Start()
    {
        dungeonRooms = DungencrawlerController.GenerateDungeon(dungenGenerationData);
        SpawRooms(dungeonRooms);
    }

    private void SpawRooms(IEnumerable<Vector2Int> rooms)
    {
        RoomController.instance.LoadRoom("Start", 0, 0);
        foreach(Vector2Int roomLocation in rooms)
        {
            //마지막에 보스룸으로 소환하는 법
            //if (roomLocation == dungeonRooms[dungeonRooms.Count-1] && !(roomLocation == Vector2Int.zero))
            //{
            //    RoomController.instance.LoadRoom("End", roomLocation.x, roomLocation.y);
            //}
            //else
            //{
                RoomController.instance.LoadRoom(RoomController.instance.GetRandomRoomName(), roomLocation.x, roomLocation.y);
            //}
        }
    }
}

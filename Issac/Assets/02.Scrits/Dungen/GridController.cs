using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Room room;
    [System.Serializable]
    public struct Grid
    {
        public int colums, rows;
        public float verticalOffset, horizontalOffset;
    }
    public Grid grid;
    public GameObject grideTile;
    public List<Vector2> availablePoints = new List<Vector2>();
    private void Awake()
    {
        room = GetComponentInParent<Room>();
        // -2 벽에 생성 안되게 하기위해서
        grid.colums = room.width - 2;
        grid.rows = room.hight - 2;
        GeneraterGrid();
    }


    public void GeneraterGrid()
    {
        grid.verticalOffset += room.transform.localPosition.y;
        grid.horizontalOffset += room.transform.localPosition.x;
        for (int y = 0; y < grid.rows; y++)
        {
            for(int x = 0; x < grid.colums; x++)
            {
                GameObject go = Instantiate(grideTile, transform);
                go.GetComponent<Transform>().position = new Vector2(x - (grid.colums - grid.horizontalOffset), y - (grid.rows - grid.verticalOffset));
                go.name = "X : " + x + "Y : " + y;
                availablePoints.Add(go.transform.position);
                go.SetActive(false);
            }
        }
        if(GetComponentInParent<ObjectRoomSpawner>() != null)
            GetComponentInParent<ObjectRoomSpawner>().InitialiseObjectSpawning();
    }
}

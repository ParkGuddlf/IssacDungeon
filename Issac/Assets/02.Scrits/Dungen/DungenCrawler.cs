using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungenCrawler : MonoBehaviour
{
    public Vector2Int Position { get; set; }
    public DungenCrawler(Vector2Int startPos)
    {
        Position = startPos;
    }
    //던전 가지생성 진행방향
    public Vector2Int Move(Dictionary<Direction, Vector2Int> directionMovementMap)
    {
        Direction toMove = (Direction)Random.Range(0, directionMovementMap.Count);
        Position += directionMovementMap[toMove];
        return Position;
    }
}

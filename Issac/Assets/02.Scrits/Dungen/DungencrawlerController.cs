using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//던전 생성 진행 방향
public enum Direction
{
    up = 0,
    left = 1,
    down =2,
    right=3

}

public class DungencrawlerController : MonoBehaviour
{
    public static List<Vector2Int> positionVisited = new List<Vector2Int>();
    private static readonly Dictionary<Direction, Vector2Int> directionMoventMap = new Dictionary<Direction, Vector2Int>
    {
        { Direction.up,Vector2Int.up },
        { Direction.left,Vector2Int.left },
        { Direction.down,Vector2Int.down },
        { Direction.right,Vector2Int.right }
    };
    //DungenGenerationData의 값으로 던전을 생성
    public static List<Vector2Int> GenerateDungeon(DungenGenerationData dungenData)
    {
        List<DungenCrawler> dungenCrawlers = new List<DungenCrawler>();
        //던전의 가지수를 생성하고 가지의 시작을 0,0부터 시작하게만든다
        for (int i = 0; i < dungenData.numberOffCrawlers; i++)
        {
            dungenCrawlers.Add(new DungenCrawler(Vector2Int.zero));
        }
        //한가지에서 소환될 던전 개수
        int iterations = Random.Range(dungenData.iterationMin, dungenData.iterationMax);
        
        for (int i = 0; i < iterations; i++)
        {
            //각각의 가지의 i번째를 positionVisited집어 넣는다
            foreach (DungenCrawler dungenCrawler in dungenCrawlers)
            {
                Vector2Int newPos = dungenCrawler.Move(directionMoventMap);
                //받아온 진행방향을 리스트에 넣어준다
                positionVisited.Add(newPos);
            }
        }
        return positionVisited;
    }
}

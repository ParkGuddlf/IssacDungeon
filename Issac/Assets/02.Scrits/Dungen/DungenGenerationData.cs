using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DungenGenerationData.asset",menuName = "DungenGenerationData/Dungen Data")]
public class DungenGenerationData : ScriptableObject
{
    //던전생성 가지수
    public int numberOffCrawlers;
    //최소생성 진행수
    public int iterationMin;
    //최소생성 진행수
    public int iterationMax;
}

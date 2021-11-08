using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Familiar.asset",menuName = "Familiars/FamiliarsObject")]
public class FamiliarData : ScriptableObject
{
    public string familirType;

    public float speed;
    public float fireDelay;
    public GameObject bulletPrefabs;
}

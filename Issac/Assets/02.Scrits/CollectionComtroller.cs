using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string name;
    public string description;
    public Sprite itemImage;
}
public class CollectionComtroller : MonoBehaviour
{
    public Item item;
    public float healthChange;
    public float moveSpeed;
    public float attackSpeedChange;
    public float bulletSizeChange;
    
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = item.itemImage;
        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            PlayerController.collectAmount++;
            GameController.HealPlayer(healthChange);
            GameController.MoveSpeedChange(moveSpeed);
            GameController.FireRateChange(attackSpeedChange);
            GameController.BulletSizeChange(bulletSizeChange);
            GameController.instance.UpdateCollectedItem(this);
            Destroy(gameObject);
        }
    }
}

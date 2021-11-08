using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigi;
    public Text collectedText;
    public static int collectAmount = 0;

    public float speed;

    [Header("Shot-----------")]
    public GameObject bulletPrefabs;
    public float bulletSpeed;
    public float lastFire;
    public float fireDelay;


    void Start()
    {
        rigi = GetComponent<Rigidbody2D>();
    }
    //이동 Bullet발사
    void Update()
    {
        fireDelay = GameController.FireRate;
        speed = GameController.MoveSpeed;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float shootHor = Input.GetAxis("ShootHorizontal");
        float shootVer = Input.GetAxis("ShootVertical");
        //탄발사 주기
        if((shootHor !=0 || shootVer !=0)&&Time.time > lastFire + fireDelay)
        {
            Shoot(shootHor, shootVer);
            lastFire = Time.time;
        }

        rigi.velocity = new Vector3(horizontal * speed, vertical * speed, 0);
        collectedText.text = "Item Collected : " + collectAmount;
    }
    //Bullet발사 BulletSpeed설정
    void Shoot(float x,float y)
    {
        GameObject bullet = Instantiate(bulletPrefabs, transform.position, transform.rotation) as GameObject;
        bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector3(
            (x < 0) ? Mathf.Floor(x) * bulletSpeed : Mathf.Ceil(x) * bulletSpeed,
            (y < 0) ? Mathf.Floor(y) * bulletSpeed : Mathf.Ceil(y) * bulletSpeed,
            0);
    }
}

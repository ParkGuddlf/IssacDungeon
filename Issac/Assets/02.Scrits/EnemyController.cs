using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//상태
public enum EnemyState
{
    Idle, Wander, Fllow, Die, Attack
}
//공격타입
public enum Enemytype
{
    Melee, Ranged
}
public class EnemyController : MonoBehaviour
{

    GameObject player;

    public EnemyState currState = EnemyState.Idle;
    public Enemytype enemyType;

    public float range;
    public float speed;
    public float attackRange;
    public float bulletSpeed;
    public float coolDown;
    private bool chooseDir = false;
    private bool dead = false;
    private bool coolDownAttack = false;
    public bool notInRoom = false;
    private Vector3 randomDir;

    public GameObject bulletPrefab;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        switch (currState)
        {
            case (EnemyState.Idle):
                //Idle();
                break;
            case (EnemyState.Wander):
                Wander();
                break;
            case (EnemyState.Fllow):
                Fllow();
                break;
            case (EnemyState.Die):
                break;
            case (EnemyState.Attack):
                Attack();
                break;
        }
        //플레이어와 같은방일때만
        if (!notInRoom)
        {
            if (IsPlayerInRange(range) && currState != EnemyState.Die)
            {
                currState = EnemyState.Fllow;
            }
            else if (!IsPlayerInRange(range) && currState != EnemyState.Die)
            {
                currState = EnemyState.Wander;
            }
            if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
            {
                currState = EnemyState.Attack;
            }
        }
        else
        {
            currState = EnemyState.Idle;
        }
    }

    public bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }
    //무작위 방향으로 회전
    private IEnumerator ChooseDirection()
    {
        chooseDir = true;
        yield return new WaitForSeconds(Random.Range(2f, 8f));
        randomDir = new Vector3(0, 0, Random.Range(0, 360));
        Quaternion nextRotation = Quaternion.Euler(randomDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, nextRotation, Random.Range(0.5f, 2.5f));
        chooseDir = false;
    }
    void Wander()
    {
        if (!chooseDir)
        {
            StartCoroutine(ChooseDirection());

        }
        //바라보는 방향을 이동
        transform.position += -transform.right * speed * Time.deltaTime;
        if (IsPlayerInRange(range))
        {
            currState = EnemyState.Fllow;
        }
    }
    //플레이어 추적
    void Fllow()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        /* Vector3 target = player.transform.position - transform.position;
         float angle = Mathf.Atan2(target.x, target.y) * Mathf.Rad2Deg;
         transform.rotation = Quaternion.AngleAxis(angle - 180, Vector3.forward);
         transform.position += -transform.right * speed * Time.deltaTime;*/
    }
    void Attack()
    {
        if (!coolDownAttack)
        {
            switch (enemyType)
            {
                case Enemytype.Melee:
                    GameController.DamagePlayer(1);
                    StartCoroutine(CollDown());
                    break;
                case Enemytype.Ranged:
                    GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
                    bullet.GetComponent<BulletController>().GetPlayer(player.transform);
                    bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
                    bullet.GetComponent<BulletController>().isEnemyBullet = true;
                    StartCoroutine(CollDown());
                    break;
            }

        }
    }

    private IEnumerator CollDown()
    {
        coolDownAttack = true;
        yield return new WaitForSeconds(coolDown);
        coolDownAttack = false;
    }
    public void Death()
    {
        Destroy(gameObject);
    }
}

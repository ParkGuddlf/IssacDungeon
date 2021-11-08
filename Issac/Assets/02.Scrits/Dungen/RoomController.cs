using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class RoomInfo
{
    public string name;

    public int x;
    public int y;
}

public class RoomController : MonoBehaviour
{
    public static RoomController instance;

    string currentWorldName = "Basement";

    RoomInfo currentLoadRoomData;

    Room currRoom;
    //방의 생성순서 대기열
    Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();
    //생성될 방의 목록
    public List<Room> loadedRooms = new List<Room>();

    bool isLoadingRoom = false;
    bool spawnedBossRoom = false;
    bool updatedRoom = false;

    private void Awake()
    {
        instance = this;
        
    }
    private void Start()
    {
        //LoadRoom("Start", 0, 0);
        //LoadRoom("Empty", 1, 0);
        //LoadRoom("Empty", -1, 0);
        //LoadRoom("Empty", 0, 1);
        //LoadRoom("Empty", 0, -1);
    }
    private void Update()
    {
        UpdateRoomQueue();
    }

    private void UpdateRoomQueue()
    {
        //방로딩을 한번에 한개씩
        if (isLoadingRoom)
        {
            return;
        }
        //대기열이 없으면
        if (loadRoomQueue.Count == 0)
        {
            if (!spawnedBossRoom)
            {
                StartCoroutine(SpawnedBossRoom());
            }
            else if (spawnedBossRoom && !updatedRoom)
            {
                //모든 방을 생성후 문을 지운다
                foreach (Room room in loadedRooms)
                {
                    room.RemoveUnconnectedDoors();
                }
                UpDataRooms();
                updatedRoom = true;
            }
            return;
        }
        //loadRoomQueue대기열의 정보를 현재소환할 방의 정보에 넣는다
        currentLoadRoomData = loadRoomQueue.Dequeue();
        isLoadingRoom = true;
        StartCoroutine(LoadRoomRoutine(currentLoadRoomData));
    }
    //목록중 마지막를 지우고 그위치에 보스룸 생성
    IEnumerator SpawnedBossRoom()
    {
        spawnedBossRoom = true;
        yield return new WaitForSeconds(0.5f);
        if (loadRoomQueue.Count == 0)
        {
            Room bossRoom = loadedRooms[loadedRooms.Count - 1];
            Room tempRoom = new Room(bossRoom.X, bossRoom.Y);
            Destroy(bossRoom.gameObject);
            var roomToRemove = loadedRooms.Single(r => r.X == tempRoom.X && r.Y == tempRoom.Y);
            loadedRooms.Remove(roomToRemove);
            LoadRoom("End", tempRoom.X, tempRoom.Y);
        }
    }

    //방이름 좌표를 받아서 방을 부른다
    public void LoadRoom(string name, int x, int y)
    {
        if (DoesRoomExist(x, y))
        {
            return;
        }
        RoomInfo newRoomData = new RoomInfo();
        newRoomData.name = name;
        newRoomData.x = x;
        newRoomData.y = y;
        //방정보를 loadRoomQueue대기열에 집어넣는다
        loadRoomQueue.Enqueue(newRoomData);
    }
    IEnumerator LoadRoomRoutine(RoomInfo info)
    {
        //Scene의 이름으로 방을 불러온다
        string roomName = currentWorldName + info.name;
        AsyncOperation loadRoom = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);
        while (loadRoom.isDone == false)
        {
            yield return null;
        }
    }

    public void RegisterRoom(Room room)
    {
        if (!DoesRoomExist(currentLoadRoomData.x, currentLoadRoomData.y))
        {
            //방이 소환될때 위치 높이와 길이를 좌표값에 곱해서 생성
            room.transform.position = new Vector3(currentLoadRoomData.x * room.width, currentLoadRoomData.y * room.hight, 0);
            //좌표와 이름설정
            room.X = currentLoadRoomData.x;
            room.Y = currentLoadRoomData.y;
            room.name = currentWorldName + "-" + currentLoadRoomData.name + "-" + room.X + "," + room.Y;
            //RoomController오브젝트의 자식으로 집어넣는다
            room.transform.parent = transform;
            //다음방소환가능하게
            isLoadingRoom = false;
            //시작하는 방을 처음에 넣어준다
            if (loadedRooms.Count == 0)
            {
                CameraController.instance.currRoom = room;
            }

            loadedRooms.Add(room);
            //인접한방이 없는 문 삭제
            //room.RemoveUnconnectedDoors();
        }
        //중복되는 방을 지운다
        else
        {
            Destroy(room.gameObject);
            isLoadingRoom = false;
        }

    }
    //방을 소환된곳인지 참이면 소환됨 거짓이면 소환안됨
    public bool DoesRoomExist(int x, int y)
    {
        return loadedRooms.Find(item => item.X == x && item.Y == y) != null;
    }
    public Room FindRoom(int x, int y)
    {
        return loadedRooms.Find(item => item.X == x && item.Y == y);
    }

    public string GetRandomRoomName()
    {
        string[] possibleRooms = new string[]{
            "Empty",
            "Basic1"
        };

        return possibleRooms[UnityEngine.Random.Range(0, possibleRooms.Length)];
    }
    //방 이동시 카메라의의 방정보 변경
    public void OnPlayerEnterRoom(Room room)
    {
        CameraController.instance.currRoom = room;
        currRoom = room;

        UpDataRooms();
    }


    //플레이어와 방이같다면 Enemystate Idle -> Another
    public void UpDataRooms()
    {
        foreach (Room room in loadedRooms)
        {
            if (currRoom != room)
            {
                EnemyController[] enemies = room.GetComponentsInChildren<EnemyController>();
                if (enemies != null)
                {
                    foreach (EnemyController enemy in enemies)
                    {
                        enemy.notInRoom = true;
                        Debug.Log("Not in Room");
                    }
                }
            }
            else
            {
                EnemyController[] enemies = room.GetComponentsInChildren<EnemyController>();
                if (enemies.Length > 0)
                {
                    foreach (EnemyController enemy in enemies)
                    {
                        enemy.notInRoom = false;
                        Debug.Log("in Room");
                    }
                }
            }
        }
    }
}

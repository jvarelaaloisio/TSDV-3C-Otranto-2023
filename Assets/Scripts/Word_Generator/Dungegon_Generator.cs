using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Directions {Up,Down,Right,Left}
//TODO: TP2 - Remove unused methods/variables
public enum RoomType { Spawn_Room,Medium_Room,L_Room,Large_Room}
public class Dungegon_Generator : MonoBehaviour
{
    public class Cell
    {
        public bool visited = false;
        public bool[] status = new bool[4];
    }

    private int currentRoomIndex = -1;
    private Room_Behaviour currentRoom;

    [Header("SetUp")]
    [SerializeField] private Vector2Int mazeSize;
    [SerializeField] private Vector2Int offSet;
    [SerializeField] private int starPos = 0;
    [SerializeField] private Room_Rule[] rooms;
    [SerializeField] private List<Room_Behaviour> dungegonRooms;
    [SerializeField] private Transform player;
    //TODO: TP2 - Syntax - Consistency in access modifiers (private/protected/public/etc)
    List<Cell> board;


    // Start is called before the first frame update
    void Start()
    {
        MazeGenerator();
    }

    private void DungegonGenerator() 
    {
        StartCoroutine(_SpawnCorutine());
    }

    private void Update()
    {
        checkPlayerPos();
    }

    //TODO: TP2 - Syntax - Consistency in naming convention
    private IEnumerator _SpawnCorutine() 
    {
        for (int i = 0; i < mazeSize.x; i++)
        {
            for (int j = 0; j < mazeSize.y; j++)
            {
                Cell currentCell = board[(i + j * mazeSize.x)];
                if (currentCell.visited)
                {
                    //TODO - Fix - Code is in Spanish or is trash code
                    int randomRoom = -1;
                    List<int> availableRoom = new List<int>();

                    for (int k = 0; k < rooms.Length; k++)
                    {
                        int p = rooms[k].ProbabilityOfSpawning(i, j);

                        if (p == (int)Spawn_Type.Has_To_Spawn)
                        {
                            randomRoom = k;
                            break;
                        }
                        else if (p == (int)Spawn_Type.Can_Spawn)
                        {
                            availableRoom.Add(k);
                        }
                    }

                    if (randomRoom == -1)
                    {
                        if (availableRoom.Count > 0)
                        {
                            randomRoom = availableRoom[Random.Range(0, availableRoom.Count)];
                        }
                        else
                        {
                            randomRoom = 0;
                        }
                    }

                    var newRoom = Instantiate(rooms[randomRoom].GetRoom(), new Vector3(i * offSet.x, 0f, -j * offSet.y), Quaternion.identity, transform).GetComponent<Room_Behaviour>();
                    newRoom.UpdateRoom(board[(i + j * mazeSize.x)].status);

                    newRoom.name += " " + i + " - " + j;
                    dungegonRooms.Add(newRoom);
                }
            }
        }
        foreach (var item in dungegonRooms)
        {
            item.SetAdjRooms();
        }


        yield return null;
    }

    void checkPlayerPos()
    {
        var aux = currentRoom;
        var count = 0f;

        for (int i = 0; i < dungegonRooms.Count; i++)
        {
            if (dungegonRooms[i].isPointInside(player.position))
            {
                currentRoom = dungegonRooms[i];
                dungegonRooms[i].SetRoomVisible(true);
                dungegonRooms[i].SetRoomCheked(true);
            }

            else
            {
                dungegonRooms[i].SetRoomVisible(false);
                dungegonRooms[i].SetRoomCheked(false);
                count++;
            }

        }

        if (count == dungegonRooms.Count)
        {
            currentRoom = aux;
            currentRoom.SetRoomVisible(true);
            currentRoom.SetRoomCheked(true);
        }
        currentRoom.ShowAdjRooms();
    }

    private void MazeGenerator() 
    {
        board = new List<Cell>();
        for (int i = 0; i < mazeSize.x; i++)
        {
            for (int j = 0; j < mazeSize.y; j++)
            {
                board.Add(new Cell());
            }
        }

        int currentCell = starPos;
        Stack<int> path = new Stack<int>();

        int k = 0;

        int mazeMaxSize = 1000;

        while (k < mazeMaxSize)
        {
            k++;

            board[currentCell].visited = true;

            if (currentCell == board.Count - 1)
            {
                break;
            }

            // Check Cells Neighbors
            List<int> neightbors = CheckAdjacentCell(currentCell);

            if (neightbors.Count == 0)
            {
                if (path.Count == 0)
                {
                    break;
                }
                else
                {
                    currentCell = path.Pop();
                }
            }
            else
            {
                path.Push(currentCell);

                int newCell = neightbors[Random.Range(0, neightbors.Count)];

                if (newCell > currentCell)
                {
                    //Down or Right
                    if (newCell - 1 == currentCell)
                    {
                        board[currentCell].status[(int)Directions.Right] = true;
                        currentCell = newCell;
                        board[currentCell].status[(int)Directions.Left] =  true;
                    }
                    else
                    {
                        board[currentCell].status[(int)Directions.Down] = true;
                        currentCell = newCell;   
                        board[currentCell].status[(int)Directions.Up] =  true;
                    }
                }
                else
                {
                    //Up or Left
                    if (newCell + 1 == currentCell)
                    {
                        board[currentCell].status[(int)Directions.Left] = true;
                        currentCell = newCell;
                        board[currentCell].status[(int)Directions.Right] = true;
                    }
                    else
                    {
                        board[currentCell].status[(int)Directions.Up] = true;
                        currentCell = newCell;
                        board[currentCell].status[(int)Directions.Down] = true;
                    }
                }
            }
        }

        DungegonGenerator();
    }


    /// <summary>
    /// Check The Adjacent Cells To the Current Cell (Up ,Down , Right , Left)
    /// </summary>
    /// <param name="cell"></param>
    /// <returns></returns>
    private List<int> CheckAdjacentCell(int cell)
    {
        List<int> neightbors = new List<int>();

        // Check Up Neightbor
        if (cell - mazeSize.x >= 0 && !board[(cell - mazeSize.x)].visited)
        {
            neightbors.Add((cell - mazeSize.x));
        }

        // Check Down Neightbor
        if (cell + mazeSize.x < board.Count && !board[(cell + mazeSize.x)].visited)
        {
            neightbors.Add((cell + mazeSize.x));
        }

        // Check Right Neightbor
        if ((cell + 1) % mazeSize.x != 0 && !board[(cell + 1)].visited)
        {
            neightbors.Add((cell + 1));
        }

        // Check Left Neightbor
        if (cell % mazeSize.x != 0 && !board[(cell - 1)].visited)
        {
            neightbors.Add((cell - 1));
        }

        return neightbors;
    }
}

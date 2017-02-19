using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pathfinding;

public class BoardCreator : MonoBehaviour
{
    // The type of tile that will be laid in a specific position.
    public enum TileType
    {
        Wall, Floor,
    }


    public int columns = 100;                                 // The number of columns on the board (how wide it will be).
    public int rows = 100;                                    // The number of rows on the board (how tall it will be).
    public IntRange numRooms = new IntRange (15, 20);         // The range of the number of rooms there can be.
    public IntRange roomWidth = new IntRange (3, 10);         // The range of widths rooms can have.
    public IntRange roomHeight = new IntRange (3, 10);        // The range of heights rooms can have.
    public IntRange corridorLength = new IntRange (6, 10);    // The range of lengths corridors between rooms can have.
    public IntRange endTargetDistance = new IntRange(15, 20);
    public IntRange enemySafeDistance = new IntRange(5, 8);
    public IntRange enemyCount = new IntRange(10, 10);
    public GameObject[] floorTiles;                           // An array of floor tile prefabs.
    public GameObject[] wallTiles;                            // An array of wall tile prefabs.
    public GameObject[] outerWallTiles;                       // An array of outer wall tile prefabs.
    public GameObject player;
    public GameObject endTarget;
    public GameObject[] enemies;
	public int GridX = 32;
	public int GridY = 32;
    public AstarPath Pathfinder;
    private TileType[][] tiles;                               // A jagged array of tile types representing the board, like a grid.
    private Room[] rooms;                                     // All the rooms that are created for this board.
    private Corridor[] corridors;                             // All the corridors that connect the rooms.
    private GameObject boardHolder;                           // GameObject that acts as a container for all other tiles.
    private int playerX;
    private int playerY;
	private GameController gameController;

    private void Start ()
    {
        // Create the board holder.
        boardHolder = new GameObject("BoardHolder");
		gameController = FindObjectOfType<GameController>();
        SetupTilesArray ();

        CreateRoomsAndCorridors ();

        SetTilesValuesForRooms ();
        SetTilesValuesForCorridors ();

        InstantiateTiles ();
        InstantiateOuterWalls ();
        CreateEntities ();

        Pathfinder.Scan();
    }


    void SetupTilesArray ()
    {
        // Set the tiles jagged array to the correct width.
        tiles = new TileType[columns][];
        
        // Go through all the tile arrays...
        for (int i = 0; i < tiles.Length; i++)
        {
            // ... and set each tile array is the correct height.
            tiles[i] = new TileType[rows];
        }
    }


    void CreateRoomsAndCorridors ()
    {
        // Create the rooms array with a random size.
        rooms = new Room[numRooms.Random];

        // There should be one less corridor than there is rooms.
        corridors = new Corridor[rooms.Length - 1];

        // Create the first room and corridor.
        rooms[0] = new Room ();
        corridors[0] = new Corridor ();

        // Setup the first room, there is no previous corridor so we do not use one.
        rooms[0].SetupRoom(roomWidth, roomHeight, columns, rows);

        // Setup the first corridor using the first room.
        corridors[0].SetupCorridor(rooms[0], corridorLength, roomWidth, roomHeight, columns, rows, true);

        for (int i = 1; i < rooms.Length; i++)
        {
            // Create a room.
            rooms[i] = new Room ();
            
            // Setup the room based on the previous corridor.
            rooms[i].SetupRoom (roomWidth, roomHeight, columns, rows, corridors[i - 1]);

            // If we haven't reached the end of the corridors array...
            if (i < corridors.Length)
            {
                // ... create a corridor.
                corridors[i] = new Corridor ();

                // Setup the corridor based on the room that was just created.
                corridors[i].SetupCorridor(rooms[i], corridorLength, roomWidth, roomHeight, columns, rows, false);
            }
            
            if (i == Mathf.RoundToInt(rooms.Length *.5f))
            {
                playerX = rooms[i].xPos;
                playerY = rooms[i].yPos;
                Vector3 playerPos = new Vector3 (playerX * GridX,
                                                 playerY * GridY, 0);
                GameObject playerObject = Instantiate(player, playerPos, Quaternion.identity);
				gameController.RegisterPlayer(playerObject);
				Debug.Log("Making player");
            }
        }

    }


    void SetTilesValuesForRooms ()
    {
        // Go through all the rooms...
        for (int i = 0; i < rooms.Length; i++)
        {
            Room currentRoom = rooms[i];
            
            // ... and for each room go through it's width.
            for (int j = 0; j < currentRoom.roomWidth; j++)
            {
                int xCoord = currentRoom.xPos + j;

                // For each horizontal tile, go up vertically through the room's height.
                for (int k = 0; k < currentRoom.roomHeight; k++)
                {
                    int yCoord = currentRoom.yPos + k;

                    // The coordinates in the jagged array are based on the room's position and it's width and height.
                    tiles[xCoord][yCoord] = TileType.Floor;
                }
            }
        }
    }


    void SetTilesValuesForCorridors ()
    {
        // Go through every corridor...
        for (int i = 0; i < corridors.Length; i++)
        {
            Corridor currentCorridor = corridors[i];

            // and go through it's length.
            for (int j = 0; j < currentCorridor.corridorLength; j++)
            {
                // Start the coordinates at the start of the corridor.
                int xCoord = currentCorridor.startXPos;
                int yCoord = currentCorridor.startYPos;

                // Depending on the direction, add or subtract from the appropriate
                // coordinate based on how far through the length the loop is.
                switch (currentCorridor.direction)
                {
                    case Direction.North:
                        yCoord += j;
                        break;
                    case Direction.East:
                        xCoord += j;
                        break;
                    case Direction.South:
                        yCoord -= j;
                        break;
                    case Direction.West:
                        xCoord -= j;
                        break;
                }

                // Set the tile at these coordinates to Floor.
                tiles[xCoord][yCoord] = TileType.Floor;
            }
        }
    }


    void InstantiateTiles ()
    {
        // Go through all the tiles in the jagged array...
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = 0; j < tiles[i].Length; j++)
            {
                // If the tile type is Wall...
                if (tiles[i][j] == TileType.Wall)
                {
                    // ... instantiate a wall over the top.
                    InstantiateFromArray (wallTiles, i, j);
                } else {
					// ... otherwise instantiate a floor tile for it.
                	InstantiateFromArray (floorTiles, i, j);
				}
            }
        }
    }


    void InstantiateOuterWalls ()
    {
        // The outer walls are one unit left, right, up and down from the board.
        float leftEdgeX = -1f;
        float rightEdgeX = columns + 0f;
        float bottomEdgeY = -1f;
        float topEdgeY = rows + 0f;

        // Instantiate both vertical walls (one on each side).
        InstantiateVerticalOuterWall (leftEdgeX, bottomEdgeY, topEdgeY);
        InstantiateVerticalOuterWall(rightEdgeX, bottomEdgeY, topEdgeY);

        // Instantiate both horizontal walls, these are one in left and right from the outer walls.
        InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, bottomEdgeY);
        InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, topEdgeY);
    }


    void InstantiateVerticalOuterWall (float xCoord, float startingY, float endingY)
    {
        // Start the loop at the starting value for Y.
        float currentY = startingY;

        // While the value for Y is less than the end value...
        while (currentY <= endingY)
        {
            // ... instantiate an outer wall tile at the x coordinate and the current y coordinate.
            InstantiateFromArray(outerWallTiles, xCoord, currentY);

            currentY++;
        }
    }


    void InstantiateHorizontalOuterWall (float startingX, float endingX, float yCoord)
    {
        // Start the loop at the starting value for X.
        float currentX = startingX;

        // While the value for X is less than the end value...
        while (currentX <= endingX)
        {
            // ... instantiate an outer wall tile at the y coordinate and the current x coordinate.
            InstantiateFromArray (outerWallTiles, currentX, yCoord);

            currentX++;
        }
    }


    void InstantiateFromArray (GameObject[] prefabs, float xCoord, float yCoord)
    {
        // Create a random index for the array.
        int randomIndex = Random.Range(0, prefabs.Length);
		
		float worldX = xCoord * GridX;
		float worldY = yCoord * GridY;

        // The position to be instantiated at is based on the coordinates.
        Vector3 position = new Vector3(worldX, worldY, 0f);

        // Create an instance of the prefab from the random index of the array.
        GameObject tileInstance = Instantiate(prefabs[randomIndex], position, Quaternion.identity) as GameObject;
        tileInstance.GetComponent<StaticEntity>().Initialize();
        // Set the tile's parent to the board holder.
        tileInstance.transform.parent = boardHolder.transform;
    }

    void CreateEntities ()
    {
        int target_distance = endTargetDistance.Random;
        int enemy_safe_distance = enemySafeDistance.Random;
        int enemy_count = enemyCount.Random;

        Debug.Log("Creating end target " + target_distance 
                  + " units away from player.");
        List<int[]> TargetCandidateTiles = new List<int[]>();
        List<int[]> EnemyCandidateTiles = new List<int[]>();
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = 0; j < tiles[i].Length; j++)
            {
                if (tiles[i][j] == TileType.Floor)
                {
                    float tile_distance = (Mathf.Sqrt(Mathf.Pow(i - playerX, 2)
                                               + Mathf.Pow((j - playerY), 2)));
                    if (target_distance == Mathf.RoundToInt(tile_distance))
                    {
                        TargetCandidateTiles.Add(new int[] {i,j});
                    } else if (tile_distance > enemy_safe_distance)
                    {
                        EnemyCandidateTiles.Add(new int [] {i, j});
                    }
                }
            }
        }

        if (TargetCandidateTiles.Count < 1)
        {
            Debug.LogWarning("no tiles for end point!!!!");
            SceneManager.LoadScene("main");
        } else if (EnemyCandidateTiles.Count < enemy_count)
        {   
            Debug.LogWarning("not enough candidate tiles for enemy!");
            SceneManager.LoadScene("main");
        } else {
            // spawn target
            int target_tile = Random.Range(0, TargetCandidateTiles.Count - 1);
            int[] endTargetLocation = TargetCandidateTiles[target_tile];
            GameObject target = Instantiate(
                endTarget, new Vector3(endTargetLocation[0] * GridX,
                                       endTargetLocation[1] * GridY,
                                       0),
                Quaternion.identity);
            gameController.RegisterTarget(target);
            TargetCandidateTiles.RemoveAt(target_tile);
            
            // spawn enemies
            for (int i = 0; i < enemy_count; i++)
            {
                target_tile = Random.Range(0, EnemyCandidateTiles.Count - 1);
                int[] enemyLocation = EnemyCandidateTiles[target_tile];
                int prefab_index = Random.Range(0, enemies.Length);
                GameObject enemy = Instantiate(
                    enemies[prefab_index], new Vector3(
                        enemyLocation[0] * GridX, enemyLocation[1] * GridY,
                        0),
                    Quaternion.identity);
                gameController.RegisterEnemy(enemy);
                EnemyCandidateTiles.RemoveAt(target_tile);
            }
        }
    }
}

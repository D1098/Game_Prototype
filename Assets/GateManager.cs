using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GateManager : MonoBehaviour
{
    // Объект замка
    public GameObject aLock;
    // obstacles
    Tilemap tileMap;
    // Тайл центральной нижней части двери
    public Vector3Int xyz;
    // Набор спрайтов открытой двери (82,83,84,99,100,101)
    public Sprite[] sprites;
    TileBase[] tiles;
    Vector3Int[] vector3Ints;
    void Start()
    {
        tileMap = GameObject.Find("Obstacles").GetComponent<Tilemap>();
        // Это ужасно, но пока что так сойдет
        tiles = new TileBase[6]
        {
            new Tile(){ sprite = sprites[0] },
            new Tile(){ sprite = sprites[1] },
            new Tile(){ sprite = sprites[2] },
            new Tile(){ sprite = sprites[3] },
            new Tile(){ sprite = sprites[4] },
            new Tile(){ sprite = sprites[5] }
        };
        vector3Ints = new Vector3Int[6]
        {
            new Vector3Int(xyz.x-1, xyz.y+1, xyz.z),
            new Vector3Int(xyz.x, xyz.y+1, xyz.z),
            new Vector3Int(xyz.x+1, xyz.y+1, xyz.z),
            new Vector3Int(xyz.x-1, xyz.y, xyz.z),
            new Vector3Int(xyz.x, xyz.y, xyz.z),
            new Vector3Int(xyz.x+1, xyz.y, xyz.z),
        };

        // Создание объекта замка
        aLock = Instantiate(aLock, tileMap.CellToWorld(xyz) + new Vector3(0.08f, 0.16f), Quaternion.identity, transform);
    }

    public void OpenGate()
    {
        // Для открытия двери
        tileMap.SetTiles(vector3Ints, tiles);
        if (GetComponentInChildren<Warp>() != null)
            GetComponentInChildren<Warp>().isOn = true;
        Destroy(aLock);
    }

    // Update is called once per frame
    void Update()
    {
        //tileMap.CellToWorld(xyz)
    }
}

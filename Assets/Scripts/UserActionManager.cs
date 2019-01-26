using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserActionManager : MonoBehaviour
{
    public static UserActionManager INSTANCE;

    public Grid grid;

    void Awake() {
        INSTANCE = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var cellCoordsForMouse = GetCellCoordsForMouse();
    }

    private Vector3Int GetCellCoordsForMouse()
    {
        Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 cellPosition = grid.WorldToCell(mousepos);
        return Vector3Int.CeilToInt(cellPosition);
    }

    public Vector3Int GetTilePositionForWorldPoint(Vector2 wp)
    {
        Vector3 mousepos = Camera.main.ScreenToWorldPoint(wp);
        Vector3 cellPosition = grid.WorldToCell(mousepos);
        return Vector3Int.CeilToInt(cellPosition);
    }
}

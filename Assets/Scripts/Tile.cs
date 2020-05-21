using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private SpriteRenderer rend;
    public Sprite[] tileGraphics;

    public float hoverAmount;

    public LayerMask obstacleLayer;

    public Color highlightedColor;
    public bool isWalkable;
    GameMaster gm;

    public Color creatableColor; //indicates what tiles a new unit can be placed on
    public bool isCreatable;

    private void Start()
    {
        //select a random tile to display from the array of chosen tiles for each tile in the scene
        rend = GetComponent<SpriteRenderer>();
        int randTile = Random.Range(0, tileGraphics.Length);
        rend.sprite = tileGraphics[randTile];

        gm = FindObjectOfType<GameMaster>();
    }

    private void OnMouseEnter()
    {
        transform.localScale += Vector3.one * hoverAmount;
    }

    private void OnMouseExit()
    {
        transform.localScale -= Vector3.one * hoverAmount;
    }

    public bool IsClear()
    {
        Collider2D obstacle = Physics2D.OverlapCircle(transform.position, .2f, obstacleLayer); //overlap circle creates invisible circle and stores it in the obstacle variable
        if( obstacle != null)
        {
            return false; //returns false if anything is on that tile
        }
        else
        {
            return true; //a clear tile returns true
        }
    }

    public void Highlight()
    {
        rend.color = highlightedColor;
        isWalkable = true;
    }

    public void Reset()
    {
        rend.color = Color.white;
        isWalkable = false;
        isCreatable = false;
    }

    public void SetCreatable()
    {
        rend.color = creatableColor;
        isCreatable = true;
    }

    private void OnMouseDown()
    {
        if(isWalkable == true && gm.selectedUnit != null) //if the tile is walkable and a unit is selected
        {
            gm.selectedUnit.Move(this.transform.position);
        }

        else if(isCreatable == true)
        {
            BarrackItem item = Instantiate(gm.purchasedItem, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            gm.ResetTiles(); //instantiate the chosen unit type and reset tiles
            Unit unit = item.GetComponent<Unit>();
            if (unit != null) //if a unit was purchased, not a chest, make sure it is unable to move that same turn
            {
                unit.hasMoved = true;
                unit.hasAttacked = true;
            }
        }
    }
}

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

    private void Start()
    {
        //i havent added the specific sprites I want to use as tiles to the tile prefab in the Unity editor yet, there are just dots as placeholders for clarity rn 
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
    }

    private void OnMouseDown()
    {
        if(isWalkable == true && gm.selectedUnit != null) //if the tile is walkable and a unit is selected
        {
            gm.selectedUnit.Move(this.transform.position);
        }
    }
}

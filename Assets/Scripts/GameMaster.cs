using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public Unit selectedUnit;

    public int playerTurn = 1; //1 means blue is moving, 2 means red is moving
  
    public void ResetTiles() //loops through all tiles and calls the reset function
    {
        foreach (Tile tile in FindObjectsOfType<Tile>())
        {
            tile.Reset();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) //switch turns if spacebar is pressed
        {
            EndTurn();
        }
    }

    void EndTurn() //function to end a players turn
    {
        if (playerTurn == 1)
        {
            playerTurn = 2;
        }
        else if (playerTurn == 2)
        {
            playerTurn = 1;
        }

        if (selectedUnit != null) //deselect any selected units
        {
            selectedUnit.selected = false;
            selectedUnit = null;
        }

        ResetTiles(); //deselect tiles

        //loop through all units and set their hasMoved variables to False, so they can move again next turn
        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            unit.hasMoved = false;
            unit.weaponIcon.SetActive(false);
            unit.hasAttacked = false;
        }
    }
}

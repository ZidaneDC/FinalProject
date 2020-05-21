using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Barrack : MonoBehaviour
{

    public Button player1ToggleButton; //will store the buttons allowing the player to toggle buttons on and off
    public Button player2ToggleButton;

    public GameObject player1Menu; //stores player menus
    public GameObject player2Menu;

    GameMaster gm;

    private void Start()
    {
        gm = GetComponent<GameMaster>();
    }

    private void Update()
    {
        //first ensure that players can only click on the buttons/menus for their own team
        if (gm.playerTurn == 1)
        {
            player1ToggleButton.interactable = true;
            player2ToggleButton.interactable = false;
        }
        else
        {
            player1ToggleButton.interactable = false;
            player2ToggleButton.interactable = true;
        }
    }

    //method to activate the player menus
    public void ToggleMenu(GameObject menu)
    {
        menu.SetActive(!menu.activeSelf); //active self returns true or false depending on whether the menu is active or not, so we take the opposite of that bool
    }

    //closes both menus
    public void CloseMenus()
    {
        player1Menu.SetActive(false);
        player2Menu.SetActive(false);
    }

    public void BuyItem(BarrackItem item)
    {
        if(gm.playerTurn ==1 && item.cost <= gm.player1Gold)//check to see if the player has enough gold
        {
            gm.player1Gold -= item.cost;
            player1Menu.SetActive(false);
        }

        else if (gm.playerTurn == 2 && item.cost <= gm.player2Gold)
        {
            gm.player2Gold -= item.cost;
            player2Menu.SetActive(false);
        }

        else //if player doe not have enough gold
        {
            print("Insufficient gold.");
            return; //exit the function
        }

        gm.UpdateGoldText(); //update the ui after a purchase

        gm.purchasedItem = item;

        if (gm.selectedUnit != null) //deselect any selected units to prepare to create a unit
        {
            gm.selectedUnit.selected = false;
            gm.selectedUnit = null;
        }

        GetCreatableTiles();
    }

    public void GetCreatableTiles() //obtains and highlights all tiles a new unit can be created on
    {
        foreach (Tile tile in FindObjectsOfType<Tile>())
        {
            if (tile.IsClear())
            {
                tile.SetCreatable();
            }
        }
    }
}

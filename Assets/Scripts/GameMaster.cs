using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    public Unit selectedUnit;

    public int playerTurn = 1; //1 means blue is moving, 2 means red is moving

    public Image playerIndicator; //will update the sprite indicating which players turn it is
    public Sprite player1Indicator;
    public Sprite player2Indicator;

    public int player1Gold = 100; //default player gold at the start 
    public int player2Gold = 100;

    public Text player1GoldText; 
    public Text player2GoldText;

    public BarrackItem purchasedItem; //stores item purchased by player

    //vairables for the stats menu
    public GameObject statsPanel; //stores stat panel image
    public Vector2 statsPanelShift; //shifts panel over, dont want it directly on top of the character
    public Unit viewedUnit; //stores unit that is currently being inspected

    public Text healthText;
    public Text armorText;
    public Text attackDamageText;
    public Text defenseDamageText;

    private void Start()
    {
        GetGoldIncome(1); //immediately at the start of the game team 1 collects gold from the chests
    }

    public void ToggleStatsPanel(Unit unit) //enables and disables stats panel
    {
        if (unit.Equals(viewedUnit) == false) //enables the stats panel if it wasnt on before
        {
            statsPanel.SetActive(true);
            statsPanel.transform.position = (Vector2)unit.transform.position + statsPanelShift; //moves the stat panel to the new position
            viewedUnit = unit;
            //function that updates the text of stats needs to be called
            UpdateStatsPanel();
        }
        else //turns off panel if the unit clicked on was already being viewed
        {
            statsPanel.SetActive(false);
            viewedUnit = null;
        }
    }

    public void UpdateStatsPanel()
    {
        if (viewedUnit != null)
        {
            healthText.text = viewedUnit.health.ToString();
            armorText.text = viewedUnit.armor.ToString();
            attackDamageText.text = viewedUnit.attackDamage.ToString();
            defenseDamageText.text = viewedUnit.defenseDamage.ToString();
        }
    }

    public void MoveStatsPanel(Unit unit) //will move the stats panel if it is enabled while the selected character moves
    {
        if (unit.Equals(viewedUnit))
        {
            statsPanel.transform.position = (Vector2)unit.transform.position + statsPanelShift;
        }
    }

    public void RemoveStatsPanel(Unit unit)
    {
        if (unit.Equals(viewedUnit))
        {
            statsPanel.SetActive(false);
            viewedUnit = null;
        }
    }
    public void UpdateGoldText()
    {
        player1GoldText.text = player1Gold.ToString();
        player2GoldText.text = player2Gold.ToString();
    }

    void GetGoldIncome(int playerTurn) //takes in the int of player turn to determine which team the gold goes to
    {
        foreach(Chest chest in FindObjectsOfType<Chest>())
        {
            //now filter out chests for the enemy team
            if(chest.playerNumber == playerTurn)
            {
                if(playerTurn == 1)
                {
                    player1Gold += chest.goldPerTurn;
                }
                else
                {
                    player2Gold += chest.goldPerTurn;
                }
            }
        }

        UpdateGoldText();
    }
  
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
            playerIndicator.sprite = player2Indicator;
        }
        else if (playerTurn == 2)
        {
            playerTurn = 1;
            playerIndicator.sprite = player1Indicator;
        }

        GetGoldIncome(playerTurn);

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

        GetComponent<Barrack>().CloseMenus(); //close any open menus when turn is switched 
    }

    public void RestartGame() //function to reset the level
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void EndGame(string exitScene) //this is the function that will exit back to the start menu scene
    {
        SceneManager.LoadScene(exitScene);
    }
}

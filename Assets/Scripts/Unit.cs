using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public bool selected;
    GameMaster gm;

    //variables determining movement
    public int tileSpeed; //determines how many tiles the character can move on
    public bool hasMoved; //tracks if the unit has moved this turn or not

    public float moveSpeed; //speed for moving animation set in unity

    public int playerNumber; //indicates what side a unit belongs to

    //attack range variables
    public int attackRange; //dictates how many tiles of distance you can attack an enemy from
    List<Unit> enemiesInRange = new List<Unit>(); //stores all the enemies a unit can attack
    public bool hasAttacked; //units only attack once per turn

    public GameObject weaponIcon; //creates sprite that will mark attackable units

    //attack damage variables
    public int health;
    public int attackDamage;
    public int defenseDamage; //how much damage is inflicted on a unit attempting to attack
    public int armor; //how much damage a defending unit can absorb

    public DamageIcon damageIcon;
    public GameObject deathEffect;

    private Animator camAnim;

    public Text leaderHealth;
    public bool isLeader; //variables that track team leader's health and whether a unit is a leader or not

    public GameObject victoryPanel; //stores win banner

    void Start()
    {
        gm = FindObjectOfType<GameMaster>(); //gives access to any public variables and functions in the game master script
        camAnim = Camera.main.GetComponent<Animator>();
        UpdateLeaderHealth(); //do it at start in case i want to change the leader's health at some point, and the ui will update accordlingly
    }

    public void UpdateLeaderHealth()
    {
        if (isLeader == true) //if unit is a team leader
        {
            leaderHealth.text = health.ToString(); //use units health as the leaders health, ensure to convert int to string
        }
    }

    private void OnMouseOver()//if mouse is over a unit
    {
        if (Input.GetMouseButtonDown(1)) //the one in this means a right click on a mouse
        {
            gm.ToggleStatsPanel(this);
        }
    }

    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        ResetWeaponIcons(); //turn off any icons that were previosuly activated

        if (selected == true)
        {
            selected = false;
            gm.selectedUnit = null; //deselects unit if it is already selected
            gm.ResetTiles();
        }
        else
        {
            if (playerNumber == gm.playerTurn) //check to make sure selected unit is part of the players team
            {
                if (gm.selectedUnit != null) //if there is a selected unit somewhere, deselect it
                {
                    gm.selectedUnit.selected = false;
                }

                selected = true; //select the new unit
                gm.selectedUnit = this;

                gm.ResetTiles();
                GetEnemies();
                GetWalkableTiles();
            } 
        }


        Collider2D col = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), .15f);
        Unit unit = col.GetComponent<Unit>();
        if(gm.selectedUnit != null) //check if any unit is selected
        {
            if (gm.selectedUnit.enemiesInRange.Contains(unit) && gm.selectedUnit.hasAttacked == false) //if object that is clicked on is in the list of attackable enemies
                //and the unit has not attacked yet
            {
                gm.selectedUnit.Attack(unit); //attack this unit
            }
        }
    }

    void Attack(Unit enemy) //attack function, takes in enemy as an argument
    {
        camAnim.SetTrigger("shake");

        hasAttacked = true;
        int enemyDamage = attackDamage - enemy.armor; //stores the amount of damage that you should do to an enemy, equal to your attacks strength minus the enemy armor stat
        int myDamage = enemy.defenseDamage - armor;//stores how much damage a defending unit should inflict on the attacking unit

        if(enemyDamage >= 1)
        {
            DamageIcon instance = Instantiate(damageIcon, enemy.transform.position, Quaternion.identity); //create a damage icon at the enemies position, no rotation
            //variable is used so a specific amount of damage can be displayed
            instance.Setup(enemyDamage);
            enemy.health -= enemyDamage; //if the attack value after the armor stat is subtracted is at least, do that amount of damage to the enemy
            enemy.UpdateLeaderHealth();
        }

        if (transform.tag == "Ranged Fighter" && enemy.tag != "Ranged Fighter") //if a mage attacks a non mage enemy
        {
            if(Mathf.Abs(transform.position.x - enemy.transform.position.x) + Mathf.Abs(transform.position.y - enemy.transform.position.y) <= 1) //if the two are fighting at close range, then do damage like normal
            {
                if (myDamage >= 1)
                {
                    DamageIcon instance = Instantiate(damageIcon, transform.position, Quaternion.identity);
                    instance.Setup(myDamage);
                    health -= myDamage;
                    UpdateLeaderHealth();
                }
            }
        }
        else //if the attack is not a melee vs a mage
        {
            if (myDamage >= 1)
            {
                DamageIcon instance = Instantiate(damageIcon, transform.position, Quaternion.identity);
                instance.Setup(myDamage);
                health -= myDamage;
                UpdateLeaderHealth();
            }
        }

      

        //now check if either unit is dead
        if(enemy.health <= 0)
        {
            if (isLeader == true) //for some reason this if statement and the matching one in line 172 do not seem to work despite not throwing errors and being coded the same way
                //as the rest of my ui, the death of the leader units was supposed to determine the win/lose state and display the restart button, 
                //however since I cant seem to fix this or even figure out whats causing the problem, the restart button will always be visible
            {
                enemy.victoryPanel.SetActive(true);
            }
            Instantiate(deathEffect, enemy.transform.position, Quaternion.identity);
            Destroy(enemy.gameObject);
            gm.RemoveStatsPanel(enemy);
            GetWalkableTiles(); //destory that enemy, you can now move on the place where the enemy was killed
        }

        if(health <= 0)
        {
            //check if unit is the team Leader
            if (isLeader == true)
            {
                victoryPanel.SetActive(true);
            }
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            gm.ResetTiles(); //reset tiles if the unit is dead
            gm.RemoveStatsPanel(this);
            Destroy(this.gameObject);
        }

        gm.UpdateStatsPanel();
    }

    void GetWalkableTiles()
    {
        if (hasMoved == true)
        {
            return; //no action if the character has already moved so skip the rest of the function
        }

        foreach(Tile tile in FindObjectsOfType<Tile>()) //looks at all the tiles in the game
        {
            if(Mathf.Abs(transform.position.x - tile.transform.position.x) +
                Mathf.Abs(transform.position.y - tile.transform.position.y) <= tileSpeed) //calculates what tiles can be walked on
            {
                if (tile.IsClear() == true) //is the tile clear
                {
                    tile.Highlight();
                }
            }
        }
    }

    void GetEnemies() //retrieves what enemies can be attacked
    {
        enemiesInRange.Clear(); //resets list
        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            if (Mathf.Abs(transform.position.x - unit.transform.position.x) +
               Mathf.Abs(transform.position.y - unit.transform.position.y) <= attackRange) //filters out units that are too far away
            {
                if (unit.playerNumber != gm.playerTurn && hasAttacked == false) //if unit being checked is not from the currently active team, and hasnt attacked
                {
                    enemiesInRange.Add(unit); //then add that unit
                    unit.weaponIcon.SetActive(true); //activate the weapon icon
                }
            }
        }
    }

    public void ResetWeaponIcons() //turns weapon icons off
    {
        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            unit.weaponIcon.SetActive(false);
        }
    }
    public void Move(Vector2 tilePos)
    {
        gm.ResetTiles();
        StartCoroutine(StartMovement(tilePos));
    }

    IEnumerator StartMovement(Vector2 tilePos)
    {
        //only want them to move in one direction at a time
        while(transform.position.x != tilePos.x)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(tilePos.x, transform.position.y), moveSpeed * Time.deltaTime);
            //delta time makes the movement frame rate independent
            yield return null; //skips to next frame, if this isnt here the statement only runs once
        }

        while (transform.position.y != tilePos.y)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, tilePos.y), moveSpeed * Time.deltaTime);
            //delta time makes the movement frame rate independent
            yield return null; //skips to next frame, if this isnt here the statement only runs once
        }

        hasMoved = true; //mark the unit as having taken a turn
        ResetWeaponIcons(); //reset happens bcus which enemies are attackable needs to be reevaluated after moving
        GetEnemies(); //mark enemies that can be attacked after moving
        gm.MoveStatsPanel(this); //move stats panel
    }
}

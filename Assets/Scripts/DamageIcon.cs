using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIcon : MonoBehaviour
{
    public Sprite[] damageSprites; //all damage sprites will be dragged and dropped here

    public float lifetime; //amount of time sprite is visible in the scene before disappearing

    public GameObject effect;

    public void Start()
    {
        Invoke("Destruction", lifetime); //destruction fuction is called after the amount of lifetime seconds 
        //invoke is the same as calling a function except it waits a certain amount of time
    }
    public void Setup(int damage)
    {
        GetComponent<SpriteRenderer>().sprite = damageSprites[damage - 1]; //uses the damageSprite index, which starts at 0, so if damage 3 needs to be displayed, it will be number 2 in the array
    }

    void Destruction()
    {
        Instantiate(effect, transform.position, Quaternion.identity);
        Destroy(gameObject); //the reason why there is a whole function for this is because im adding effects to it later
    }
}

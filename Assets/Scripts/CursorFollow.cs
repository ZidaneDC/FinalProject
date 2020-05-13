using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorFollow : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = false; //makes default cursor invisible
    }
    private void Update()
    {
        //have cursor follow mouse
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //variable is now equal to whatever the mouse position in the scene is
        transform.position = cursorPos; //object will now follow the mouse
    }
}

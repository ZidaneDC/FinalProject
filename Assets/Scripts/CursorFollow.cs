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
        transform.position = Input.mousePosition;
    }
}

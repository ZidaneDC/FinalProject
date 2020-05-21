using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMovement : MonoBehaviour
{
    private float floatingTimer = 0f;
    public float floatingMax = 1f;
    public float floatingDir = 0.01f;

    private void FixedUpdate()
    {
        if (floatingTimer < floatingMax)
        {
            transform.position = new Vector2(transform.position.x,
                transform.position.y + (Time.fixedDeltaTime * floatingDir));
            floatingTimer += Time.fixedDeltaTime;
        }
        else
        {
            floatingDir *= -1;
            floatingTimer = 0f;
        }
    }
}

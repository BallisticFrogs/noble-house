using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    public float speed = 1.0f;
    public float margin = 10.0f;

    public Vector3 mRightDirection = Vector3.right;
    public Vector3 mLeftDirection = Vector3.left;
    public Vector3 mUpDirection = Vector3.up;
    public Vector3 mDownDirection = Vector3.down;
    
    void Update()
    {
        // Move camera with arrow keys
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0));
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
        }
        
        // Move camera when mouse on the edge of the screen
        if (Input.mousePosition.x >= Screen.width - margin)
        {
            transform.position += mRightDirection * Time.deltaTime * speed;
        }
        if (Input.mousePosition.x <= margin)
        {
            transform.position += mLeftDirection * Time.deltaTime * speed;
        }
        if (Input.mousePosition.y >= Screen.height - margin)
        {
            transform.position += mUpDirection * Time.deltaTime * speed;
        }
        if (Input.mousePosition.y <= margin)
        {
            transform.position += mDownDirection * Time.deltaTime * speed;
        }
    }
}

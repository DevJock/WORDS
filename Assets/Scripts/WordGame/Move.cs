using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour
{
    public float speed;
    Rigidbody rigid;
    private void Start()
    {
        if(GetComponent<Rigidbody>())
        {
            rigid = GetComponent<Rigidbody>();
            rigid.velocity = new Vector3(0, speed, 0);
            rigid.useGravity = false;
        }
    }

    private void Update()
    {
        if (GameController.GAME.GameState != GameController.GAME.STATE.Paused)
        {
            Vector3 newPos = transform.position;
            newPos.y -= speed;
            transform.position = newPos;
        }
    }
}
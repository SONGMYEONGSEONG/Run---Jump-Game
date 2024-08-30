using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] protected float speed = 1.0f;
    public float Speed { get { return speed; } set { speed = value; } } 
    [SerializeField] protected int score = 0;
    public int Score { get { return Score; } set { score = value; } }
    protected Vector3 block_pos;
    public Vector3 Block_pos { get { return block_pos; } set { block_pos = value; } }

    public bool isScore;

    private void Start()
    {
        isScore = false;
        transform.position = block_pos;
    }

    protected void MoveBlock()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x <= (int)defineNum.Blokc_End_posX)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        MoveBlock();
    }

     private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isScore && collision.gameObject.CompareTag("Player"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.point.y > transform.position.y + 0.3f)
                {
                    GameManager.Instance.AddScore(score);
                    isScore = true;
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPattern : MonoBehaviour
{
    [SerializeField] float DropSpeed = 0.5f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Drop());
        }
    }

    IEnumerator Drop()
    {
        while(transform.position.y >= -12f)
        {
            transform.position += Vector3.down * DropSpeed * Time.deltaTime;
            yield return null;
        }
    }

}

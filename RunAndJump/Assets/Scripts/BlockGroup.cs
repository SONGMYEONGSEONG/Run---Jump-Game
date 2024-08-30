using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGroup : Block
{
    [SerializeField] protected List<GameObject> blocks;

    private void Update()
    {
        MoveBlock();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int orbCount = 0;

    public bool AttemptUseOrb()
    {
        if(orbCount - 1 >= 0)
        {
            orbCount--;
            return true;
        }
        return false;
    }
}
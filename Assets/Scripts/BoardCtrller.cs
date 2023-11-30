using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCtrller : MonoBehaviour
{
    public GameObject SquareItem;
    public float separacion = 1.25f;

    void Start()
    {
        for (int PosX = 0; PosX < 3; PosX++)
        {
            for (int PosY = 0; PosY < 3; PosY++)
            {
                Instantiate(SquareItem, new Vector3(PosX * separacion, PosY * 1.25f), Quaternion.identity);
            }
        }
    }
}

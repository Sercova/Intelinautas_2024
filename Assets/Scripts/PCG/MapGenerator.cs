using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject[] blocks;
    public Transform abysses;
    public Transform StartPos;
    public int Iterations;
    public ContactFilter2D contactFilter;
    public int corr;

    private IEnumerator coroutine;
    private Transform parentPlatform;
    
    float lowerPlatform;
    void Start()
    {

        lowerPlatform = Mathf.Infinity;
        //Debug.Log("Player PosY INI: " + StartPos.position.y);
        foreach (GameObject block in blocks)
        {

            parentPlatform = block.transform.parent.transform;
            coroutine = null;
            coroutine = CreatePlatform(block);
            StartCoroutine(coroutine);
            abysses.position = new Vector3(0f, -25f, 0f);
            StartPos.position = new Vector3(-5.5f, 40.7f, 0.0f);
        }

        Collider2D[] Colliders = new Collider2D[10];
        bool isClearPlayer = Physics2D.OverlapCircle(new Vector2(0.0f, lowerPlatform), 1.5f, contactFilter, Colliders) == 0;

        if (!isClearPlayer)
            while (!isClearPlayer)
            {
                lowerPlatform -= 1.0f;
                isClearPlayer = Physics2D.OverlapCircle(new Vector2(0.0f, lowerPlatform), 1.5f, contactFilter, Colliders) == 0;
            }

        StartPos.position = new Vector3(0.0f, lowerPlatform, 0.0f);
        //Debug.Log("Player PosY FIN: " + StartPos.position.y);
        


    }

    private IEnumerator CreatePlatform(GameObject arg_block)
    {
        //Debug.Log("Abysse PosY: " + abysses.position);
        corr = 0;
        float lowerBound;
        lowerBound = Mathf.Infinity;
        CreateBlock(arg_block, ref lowerBound);

        if (lowerBound != Mathf.Infinity)
            abysses.position = new Vector3(0f, lowerBound, 0f);
        //Debug.Log("Abysse PosY: " + abysses.position);

        yield return new WaitForSeconds(0.0001f);
    }

    private bool CreateBlock(GameObject arg_block, ref float arg_lowerBound)
    {
        string notClear = "";

        int PN = arg_block.GetComponent<Blk>().P_N;
        int PS = arg_block.GetComponent<Blk>().P_S;
        int PE = arg_block.GetComponent<Blk>().P_E;
        int PW = arg_block.GetComponent<Blk>().P_W;

        //int P_N_aux = PN;
        //int P_S_aux = PS;
        //int P_E_aux = PE;
        //int P_W_aux = PW;

        float inc_x = 0.0f;
        float inc_y = 0.0f;
        float posX;
        float posY;
        //posX = arg_block.transform.position.x;
        //posY = arg_block.transform.position.y;
        posX = arg_block.gameObject.transform.position.x;
        posY = arg_block.gameObject.transform.position.y;

        bool isClear = false;
        while (!isClear)
        {
            int coin = Random.Range(0, PN + PS + PE + PW + 1);
            //Debug.Log("Coin: " + coin);

            if (coin > 0 && coin <= PN)
            {   // Grow northward
                if (!notClear.Contains("N"))
                    notClear = notClear + "N";
                inc_x = 0.0f;
                inc_y = 1.0f;
            }
            else if (coin > PN && coin <= PN + PS)
            {   // Grow southward
                if (!notClear.Contains("S"))
                    notClear = notClear + "S";
                inc_x = 0.0f;
                inc_y = -1.0f;
            }
            else if (coin > PN + PS && coin <= PN + PS + PE)
            {   // Grow eastward
                if (!notClear.Contains("E"))
                    notClear = notClear + "E";
                inc_x = 1.0f;
                inc_y = 0.0f;
            }
            else if (coin > PN + PS + PE && coin <= PN + PS + PE + PW)
            {   // Grow westward
                if (!notClear.Contains("W"))
                    notClear = notClear + "W";
                inc_x = -1.0f;
                inc_y = 0.0f;
            }

            Collider2D[] Colliders = new Collider2D[10];
            isClear = Physics2D.OverlapCircle(new Vector2(posX + inc_x, posY + inc_y), 0.25f, contactFilter, Colliders) == 0;

            if (!isClear && notClear.Length == 4)
                return false;

            if (isClear)
            {
                
                corr++;
                GameObject newBlock = Instantiate(arg_block, new Vector3(posX + inc_x, posY + inc_y, 0.0f), Quaternion.identity, parentPlatform);
                //Debug.Log("arg_block:" + arg_block.name + "; Parent: " + gameObject.transform.parent);
                //newBlock.transform.name = "Blk_" + gameObject.name + "_" + corr.ToString("000");
                newBlock.transform.name = parentPlatform.name + "_Blk_" + corr.ToString("000");

                if (newBlock.transform.position.y < arg_lowerBound)
                    lowerPlatform = newBlock.transform.position.y;
                    arg_lowerBound = newBlock.transform.position.y;
                    //Debug.Log(parentPlatform.name + "_Blk_" + corr.ToString("000") + " is arg_lowerBound: " + arg_lowerBound);

                //if (GameObject.FindGameObjectsWithTag("Ground").Length < Iterations)
                if (corr < Iterations)
                {
                    bool Grew;
                    Grew = CreateBlock(newBlock, ref arg_lowerBound);
                    if (!Grew)
                        isClear = false;

                    notClear = "";

                }
                else
                {
                    return false;
                }
            }


        } // while (!isClear)

        return true;

    }

}

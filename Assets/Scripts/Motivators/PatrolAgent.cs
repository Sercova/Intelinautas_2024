using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PatrolAgent : MonoBehaviour
{
    public bool vulnerable;
    public GameObject Alarm;
    public ContactFilter2D ContactFilter;

    private void Start()
    {
        vulnerable = false;
    }

    private void OnTriggerExit2D(Collider2D arg_collision)
    {
        if (arg_collision.gameObject.tag == "Player")
            vulnerable = false;
    }

    private void OnTriggerEnter2D(Collider2D arg_collision)
    {
        if (arg_collision.gameObject.tag == "Player")
            vulnerable = true;

        if (arg_collision.gameObject.tag == "Abbys")
        {
            vulnerable = true;
            Disappear(gameObject);
        }

        //else
        //    vulnerable = false;
    }

    private void OnTriggerStay2D(Collider2D arg_collision)
    {
        if (arg_collision.gameObject.tag == "Player")
            vulnerable = true;
        //else
        //    vulnerable = false;
    }
    private void Update()
    {
        if (vulnerable)
            Alarm.SetActive(true);
        else
            Alarm.SetActive(false);



        if (Input.GetMouseButtonDown(0) && vulnerable)
        {
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //RaycastHit2D hit_collider = Physics2D.Raycast(touchPos, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Intelli_Motivator"));
            Collider2D[] Colliders = new Collider2D[10];
            bool isTouched = Physics2D.OverlapCircle(touchPos, 0.5f, ContactFilter, Colliders) > 0;

            //if (hit_collider.collider != null)
            if (isTouched)
            {
                //Debug.Log("Desaparecer: " + hit_collider.collider.gameObject.transform.parent.gameObject.name);
                //Disappear(hit_collider.collider.gameObject.transform.parent.gameObject);
                Disappear(Colliders[0].gameObject.transform.parent.gameObject);
            }

        }
    }

    private void Disappear(GameObject arg_gameObj)
    {
        if (vulnerable)
        {
            SaveSceneState.SceneData sceneData = new SaveSceneState.SceneData();

            sceneData.UniqueID  = arg_gameObj.GetComponent<ExecFunc_UniqueID>().uniqueId;
            sceneData.GObjPos_X = arg_gameObj.transform.position.x;
            sceneData.GObjPos_Y = arg_gameObj.transform.position.y;
            sceneData.GObjPos_Z = arg_gameObj.transform.position.z;

            sceneData.GObjLocalScale_X = arg_gameObj.transform.localScale.x;
            sceneData.GObjLocalScale_Y = arg_gameObj.transform.localScale.y;

            ExecFuncsChallengeInfo.GetInstance().DestroyedSceneObjects.Add(sceneData);

            Destroy(arg_gameObj);
        }

    }

}
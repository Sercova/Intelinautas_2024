using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureExit : MonoBehaviour
{
    public int CreatureID;
    private void OnTriggerEnter2D(Collider2D arg_collision)
    {
        CreatureAI ai = arg_collision.gameObject.GetComponentInParent<CreatureAI>();
        if (ai)
        {
            if (ai.CreatureID == this.CreatureID)
            {
                SaveSceneState.SceneData creatureData = new SaveSceneState.SceneData();
                creatureData.UniqueID = ai.gameObject.GetComponent<ExecFunc_UniqueID>().uniqueId;
                creatureData.GObjPos_X = ai.gameObject.transform.position.x;
                creatureData.GObjPos_Y = ai.gameObject.transform.position.y;
                creatureData.GObjPos_Z = ai.gameObject.transform.position.z;
                creatureData.GObjLocalScale_X = ai.gameObject.transform.localScale.x;
                creatureData.GObjLocalScale_Y = ai.gameObject.transform.localScale.y;
                ExecFuncsChallengeInfo.GetInstance().DestroyedSceneObjects.Add(creatureData);

                Destroy(ai.gameObject);

                SaveSceneState.SceneData exitData = new SaveSceneState.SceneData();
                exitData.UniqueID = this.gameObject.GetComponent<ExecFunc_UniqueID>().uniqueId;
                exitData.GObjPos_X = this.gameObject.transform.position.x;
                exitData.GObjPos_Y = this.gameObject.transform.position.y;
                exitData.GObjPos_Z = this.gameObject.transform.position.z;
                exitData.GObjLocalScale_X = this.gameObject.transform.localScale.x;
                exitData.GObjLocalScale_Y = this.gameObject.transform.localScale.y;
                ExecFuncsChallengeInfo.GetInstance().DestroyedSceneObjects.Add(exitData);

                Destroy(this.gameObject);
                return;
            }
                
        }
    }

}

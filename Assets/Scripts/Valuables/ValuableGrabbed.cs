using UnityEngine;

public class ValuableGrabbed : StateMachineBehaviour
{
    public override void OnStateExit(Animator arg_animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //base.OnStateExit(animator, stateInfo, layerIndex);
        SaveSceneState.SceneData sceneData = new SaveSceneState.SceneData();
        sceneData.UniqueID = arg_animator.gameObject.GetComponent<ExecFunc_UniqueID>().uniqueId;
        sceneData.GObjPos_X = arg_animator.gameObject.transform.position.x;
        sceneData.GObjPos_Y = arg_animator.gameObject.transform.position.y;
        sceneData.GObjPos_Z = arg_animator.gameObject.transform.position.z;
        sceneData.GObjLocalScale_X = arg_animator.gameObject.transform.localScale.x;
        sceneData.GObjLocalScale_Y = arg_animator.gameObject.transform.localScale.y;

        ExecFuncsChallengeInfo.GetInstance().DestroyedSceneObjects.Add(sceneData);

        Destroy(arg_animator.gameObject);
    }
}

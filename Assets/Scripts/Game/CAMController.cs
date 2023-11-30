using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CAMController : MonoBehaviour
{
    public CinemachineVirtualCamera CM_MainCam;
    public CinemachineVirtualCamera CM_ZoomOut;
    public GameObject IntelliChar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ZoomOut(GameObject arg_target, float arg_time)
    {
        CM_ZoomOut.m_Follow = arg_target.transform;
        CM_MainCam.gameObject.SetActive(false);
        StartCoroutine(FocusCreature(arg_time));
    }

    public void ZoomOut(GameObject arg_target)
    {
        CM_ZoomOut.m_Follow = arg_target.transform;
        CM_MainCam.gameObject.SetActive(false);
    }

    public void ZoomIn(GameObject arg_target)
    {
        CM_MainCam.m_Follow = arg_target.transform;
        CM_MainCam.gameObject.SetActive(true);
    }

    public void SwitchZoom(GameObject arg_target)
    {
        CM_MainCam.m_Follow = arg_target.transform;

        if (CM_MainCam.gameObject.activeSelf)
            CM_MainCam.gameObject.SetActive(false);
        else
            CM_MainCam.gameObject.SetActive(true);
    }


    private IEnumerator FocusCreature(float arg_time)
    {
        yield return new WaitForSeconds(arg_time);
        CM_MainCam.gameObject.SetActive(true);
        CM_ZoomOut.m_Follow = IntelliChar.gameObject.transform;

    }
}

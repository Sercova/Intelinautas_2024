using System.Collections;
using UnityEngine;
using TMPro;


public class ScreenMsg : MonoBehaviour {

    [SerializeField]
    private TextMeshProUGUI txtMessage;
    //public string Message;

    public float timeElapse = 10.0f;  // 10 seconds just in case timeElapse is not yet assigned

    private IEnumerator MsgCoRoutine;

    //float startTimer = 0.0f;
    private void Start()
    {
        txtMessage.GetComponentInParent<TextMeshBlink>().startTextMeshAnimation();
        //startTimer = 0.0f;

        MsgCoRoutine = ShowMessage();
        StartCoroutine(MsgCoRoutine);
        // This kind of message is static, not blinking
        txtMessage.GetComponentInParent<TextMeshBlink>().stopTextMeshAnimation();
        
    }

    //private void Update()
    //{
    //    startTimer += Time.deltaTime;
    //}

    private IEnumerator ShowMessage()
    {
        //txtMessage.text = Message;
        yield return new WaitForSeconds(timeElapse);
        this.gameObject.SetActive(false);

    }

    // To cancel message from TrapsControllers and Challenge Class
    public void HideMessage()
    {
        this.gameObject.SetActive(false);
    }

    //private void Update()
    //{
    //    if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
    //    {
    //        StopCoroutine(MsgCoRoutine);
    //        this.gameObject.SetActive(false);
    //    }
    //}
}

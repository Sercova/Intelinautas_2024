using System;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Display : MonoBehaviour
{

    public Text TXT;
    public RectTransform rt;
    //public Rigidbody2D RB;
    //public Slider vel_X_bar;
    //public Slider vel_Y_bar;

    void Start()
    {
        rt.position = new Vector3(0.0f, Screen.height, 0.0f);
        rt.sizeDelta = new Vector3(0.0f, Screen.height * 2.0f);
        rt.offsetMin = new Vector2(0.0f, rt.offsetMin.y);
        rt.offsetMax = new Vector2(0.0f, rt.offsetMax.y);

        TXT.text = "v" + Application.version;
    }

    //private void OnGUI()
    //{
    //    vel_X_bar.value = Mathf.Abs(RB.velocity.x);
    //    vel_Y_bar.value = Mathf.Abs(RB.velocity.y);
    //}
}

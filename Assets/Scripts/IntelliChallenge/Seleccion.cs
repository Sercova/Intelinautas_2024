using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seleccion : MonoBehaviour
{
    public float Propiedad1;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit_collider = Physics2D.Raycast (touchPos, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Intelli_Alternative"));
            bool isSelected = hit_collider.collider != null;

            // Determinar de cuál alternativa es el collider que fue tocado
            if (isSelected)
            {
                if (gameObject.name == hit_collider.collider.gameObject.name)
                    Debug.Log("isSelected: " + isSelected + "; gameObject.name: " + gameObject.name);
            }
        }
    }



}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RegularPolygon_Outline : MonoBehaviour
{
    [Range(3, 12)]
    public int sides = 5;
    [Range(0.0f, 5.0f)]
    public float radius = 3;
    public LineRenderer polygonRenderer;
    public int extraSteps = 2;
    public bool isLooped;
    public float width;

    private Vector3 initialPos;
    private Vector3 deltaPos;
    private Quaternion deltaRot;
    private Quaternion RotationXY;

    private void Start()
    {
        initialPos = gameObject.transform.position;
    }

    void Update()
    {
        deltaPos = gameObject.transform.position - initialPos;
        polygonRenderer.startWidth = width;
        polygonRenderer.endWidth = width;
        if (isLooped)
        {
            DrawLooped();
        }
    }


    private void DrawLooped()
    {
        polygonRenderer.positionCount = sides;
        float TAU = 2 * Mathf.PI / sides;
        for (int currentPoint = 0; currentPoint < sides; currentPoint++)
        {
            //float currentRadian = (float)currentPoint * TAU + transform.localEulerAngles.z;
            float currentRadian = (float)currentPoint * TAU + transform.eulerAngles.z/(360.0f/(sides *TAU));

            float x = Mathf.Cos(currentRadian) * radius;
            float y = Mathf.Sin(currentRadian) * radius;

            Vector3 PointPos = new Vector3(x, y, 0) + transform.position;
            polygonRenderer.SetPosition(currentPoint, PointPos);
        }

        polygonRenderer.loop = true;
    }

    private void DrawOverlapped()
    {
        DrawLooped();
        polygonRenderer.loop = false;
        polygonRenderer.positionCount += extraSteps;

        int positionCount = polygonRenderer.positionCount;
        for (int i = 0; i<extraSteps; i++)
        {
            polygonRenderer.SetPosition((positionCount - extraSteps + i), polygonRenderer.GetPosition(i));
        }
    }
}
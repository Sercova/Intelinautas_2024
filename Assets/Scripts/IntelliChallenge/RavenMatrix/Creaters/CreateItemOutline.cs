using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateItemOutline : MonoBehaviour
{
    // Start is called before the first frame update
    public float radio;
    public int numVertices;
    public int layer;

    void Start()
    {
        RegularPolygon_Outline o=gameObject.AddComponent(typeof(RegularPolygon_Outline)) as RegularPolygon_Outline;
        o.radius = radio;
        o.sides = numVertices;
        o.width = 0.1f;
        o.extraSteps = 2;
        o.isLooped = true;
        o.polygonRenderer = gameObject.AddComponent(typeof(LineRenderer)) as LineRenderer;
        o.polygonRenderer.sharedMaterial = Resources.Load("PolyMat_White", typeof(Material)) as Material;
        o.polygonRenderer.sortingOrder = layer;


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

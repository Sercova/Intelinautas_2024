using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class CreateItemSolid : MonoBehaviour
{
    // Start is called before the first frame update
    public float radio;
    public int numVertices;
    public int layer;
   
  
    void Start()
    {
        MeshFilter filter=gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
      
        RegularPolygon Polygon =gameObject.AddComponent(typeof(RegularPolygon)) as RegularPolygon;

        MeshRenderer renderer = gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        renderer.sharedMaterial = Resources.Load("PolyMat_Yellow", typeof(Material)) as Material;
        renderer.shadowCastingMode = ShadowCastingMode.On;
        renderer.receiveShadows = true;
        renderer.lightProbeUsage = LightProbeUsage.BlendProbes;
        renderer.reflectionProbeUsage = ReflectionProbeUsage.BlendProbes;
        renderer.motionVectorGenerationMode = MotionVectorGenerationMode.Object;
        renderer.allowOcclusionWhenDynamic = true;
        renderer.sortingOrder = layer;
        
        Polygon.radius = radio;
        Polygon.numVertices = numVertices;
        Polygon.sortingLayer = layer;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

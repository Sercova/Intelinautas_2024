using UnityEngine;

[ExecuteInEditMode]
public class RegularPolygon : MonoBehaviour
{
    [Range(0.0f, 5.0f)]
    [SerializeField] public float radius = 5;
    [Range(3, 12)]
    [SerializeField] public int numVertices = 5;
    [SerializeField] public int sortingLayer = 0;

    Mesh polyMesh;
    float prevRadius;

    void Start()
    {
        polyMesh = new Mesh();
        prevRadius = 0.0f;
    }

    private void LateUpdate()
    {
        DrawPolygon();
    }

    void DrawPolygon()
    {
        // Create the polygon's vertices
        Vector3[] vertices = new Vector3[numVertices];
        for (int i = 0; i < numVertices; i++)
        {
            float angle = 2 * Mathf.PI / numVertices;
            float vAngle = i * angle;
            vertices[i] = new Vector3(Mathf.Cos(vAngle), Mathf.Sin(vAngle), 0.0f) * radius;
        }

        if ((vertices.Length >= 3 && polyMesh.vertices.Length != vertices.Length) || radius != prevRadius)
        {
            polyMesh.triangles = new int[0];
            polyMesh.vertices = vertices;

            // The triangle vertices must be done in clockwise order.
            int[] triangles = new int[3 * (numVertices - 2)];
            for (int i = 0; i < numVertices - 2; i++)
            {
                triangles[3 * i] = 0;
                triangles[(3 * i) + 1] = i + 1;
                triangles[(3 * i) + 2] = i + 2;
            }

            polyMesh.triangles = triangles;
            prevRadius = radius;

            gameObject.GetComponent<MeshFilter>().mesh = polyMesh;
        }
    }
}
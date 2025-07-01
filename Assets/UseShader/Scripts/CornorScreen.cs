using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

public class CornorScreen : MonoBehaviour
{
    [SerializeField] List<GameObject> quads;
    // Start is called before the first frame update
    void Start()
    {
        CreatNewQuad(quads[0]);
    }

    GameObject CreatNewQuad(GameObject oldQuad)
    {
        GameObject newQuad;
        Vector3[] vertices = GetNewVertices(oldQuad, out newQuad);
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        newQuad.AddComponent<MeshFilter>().mesh = mesh;
        return newQuad;
    }
    Vector3[] GetNewVertices(GameObject gameObject, out GameObject g)
    {
        Vector3[] originVertices = gameObject.GetComponent<MeshFilter>().mesh.vertices;
        Vector3[] newVertices = new Vector3[originVertices.Length];
        Color[] colors = new Color[] { Color.green, Color.blue, Color.red, Color.cyan };
        g = new GameObject();
        Transform t = g.transform;
        t.position = GetCorrespondedPosition(gameObject.transform.position, Vector3.forward, 10, Color.yellow);
        for (int i = 0; i < originVertices.Length; i++)
        {
            Vector3 worldPosition = gameObject.transform.TransformPoint(originVertices[i]);
            newVertices[i] =t.InverseTransformPoint(GetCorrespondedPosition(worldPosition, Vector3.forward, 10, colors[i]));
        }
        return newVertices;
    }
    Vector3 GetCorrespondedPosition(Vector3 origin, Vector3 direction, float distance, Color color)
    {
        RaycastHit hitInfo;
        Debug.DrawRay(origin, direction * distance, color, float.PositiveInfinity);
        if (Physics.Raycast(origin, direction, out hitInfo, distance))
        {
            return hitInfo.point;
        }
        else
        {
            return Vector3.zero;
        }
    }
    void movement(Vector3 direction, float speed)
    {
        transform.position += direction * speed * Time.deltaTime;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

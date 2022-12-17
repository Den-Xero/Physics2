using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physics_Plane : MonoBehaviour
{
    [SerializeField] SphereCollider[] Points = new SphereCollider[3];
    public Vector3 Normal;
    public Vector3 P1;

    Vector3 CrossProduct(Vector3 a, Vector3 b)
    {
        Vector3 V;
        V.x = (a.x * b.x);
        V.y = (a.y * b.y);
        V.z = (a.z * b.z);
        return V;
    }

    public Vector3 GetLocation()
    {
        return this.transform.position;
    }


    // Start is called before the first frame update
    void Start()
    {
       P1 = Points[0].transform.position;
       Vector3 P2 = Points[1].transform.position;
       Vector3 P3 = Points[2].transform.position;
       Normal = CrossProduct((P2 - P1), (P3 - P1));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

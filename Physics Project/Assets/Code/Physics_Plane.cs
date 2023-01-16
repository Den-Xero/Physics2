using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physics_Plane : MonoBehaviour
{
    [SerializeField] SphereCollider[] Points = new SphereCollider[3];


    public Vector3 GetLocation()
    {
        return transform.position;
    }

    public Vector3 GetNormal()
    {
        return transform.up;
    }

    public Vector3 GetP1Pos()
    {
        return Points[0].transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

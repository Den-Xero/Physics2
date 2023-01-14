using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physics_Sphere : MonoBehaviour
{
    //Variables and structs
    public Vector3 Velocity;
    [SerializeField] Vector3 Acceleration;
    [SerializeField]public float Radius = 0.5f;
    public Vector3 Pos;

    public Vector3 GetLocation()
    {
        return transform.position;
    }

    public void OneMoarTime()
    {
        Update();
    }

    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        float Delta = Time.deltaTime;
        Velocity += Acceleration * Delta;

        transform.position += Velocity * Delta;
    }
}

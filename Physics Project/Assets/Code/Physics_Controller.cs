using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physics_Controller : MonoBehaviour
{
    [SerializeField]Physics_Sphere[] Spheres = new Physics_Sphere[10];
    [SerializeField]Physics_Plane[] Planes = new Physics_Plane[10];
    [SerializeField] bool SphereToStationarySphere;
    [SerializeField] bool SphereToPlane;
    [SerializeField] bool MovingSphereToMovingSphere;
    float DotProduct(Vector3 a, Vector3 b)
    {
        return (a.x * b.x) + (a.y * b.y) + (a.z * b.z);
    }
    float TheLengthOfVector(Vector3 a)
    {
        return Mathf.Sqrt(Mathf.Pow(a.x, 2) + Mathf.Pow(a.y, 2) + Mathf.Pow(a.z, 2));
    }

    Vector3 AddFloat(Vector3 a, float b)
    {
        Vector3 c;
        c.x = a.x + b;
        c.y = a.y + b;
        c.z = a.z + b;
        return c;
    }

    bool EqualToOrLessThanNegative(Vector3 a, Vector3 b)
    {
        float x = a.x - b.x;
        float y = a.y - b.y;
        float z = a.z - b.z;
        if (x > 0) return false;
        if (y > 0) return false;
        if (z > 0) return false;
        return true;
    }

    bool EqualToOrLessThanPositve(Vector3 a, Vector3 b)
    {
        float x = a.x + b.x;
        float y = a.y + b.y;
        float z = a.z + b.z;
        if (x > 0) return false;
        if (y > 0) return false;
        if (z > 0) return false;
        return true;
    }

    bool TestNegative(Vector3 a)
    {
        if (a.x < 0) return true;
        if (a.y < 0) return true;
        if (a.z < 0) return true;
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(SphereToStationarySphere) Collision(Spheres[0], Spheres[1]);

    }


    void Collision(Physics_Sphere ball1, Physics_Sphere ball2)
    {
        Vector3 Ball1Location = ball1.GetLocation();
        Vector3 Ball2Location = ball2.GetLocation();
        Vector3 LengthA = Ball1Location - Ball2Location;
        Debug.Log(" Hello: " + LengthA);

        float RadiiBall1 = ball1.Radius;//ball1->GetRadius();
        float RadiiBall2 = ball2.Radius;// ball2->GetRadius();
        float SumOfRadii = RadiiBall1 + RadiiBall2;

        Vector3 LengthV = ball1.Velocity;

        Vector3 TestA = LengthA;

        bool TestB;

        bool IsNegative = TestNegative(LengthV);

        //if (IsNegative)
        //{
        //    float NegitiveRadii = SumOfRadii * -1;

        //    Vector3 TestV = AddFloat(LengthV, NegitiveRadii);

        //    TestB = EqualToOrLessThanPositve(TestA, TestV);
        //}
        //else
        //{
        //    TestA = TestA * -1;

        //    Vector3 TestV = AddFloat(LengthV, SumOfRadii);

        //    TestB = EqualToOrLessThanNegative(TestA, TestV);
        //}


        TestB = true;
        if (TestB)
        {

            Debug.Log("No collision1");

            float AngleQ = Mathf.Acos(DotProduct(LengthV, LengthA) / (TheLengthOfVector(LengthV) * TheLengthOfVector(LengthA)));

            //float AngleQ = DotProduct(LengthV, LengthA) / (TheLengthOfVector(LengthV) * TheLengthOfVector(LengthA));
            //AngleQ = Mathf.Cos(AngleQ);

            AngleQ = (AngleQ / 180 * Mathf.PI);

            float LengthD = Mathf.Sin(AngleQ) * TheLengthOfVector(LengthA);


            if (LengthD > SumOfRadii)
            {
                    //Ball1Location = Ball1Location + LengthV;
                Debug.Log("No collision2");
            }
            else
            {
                Debug.Log("No collision3");
                float LengthE = Mathf.Sqrt(Mathf.Pow((RadiiBall1 + RadiiBall2), 2) - Mathf.Pow(LengthD, 2));

                float LengthVc = Mathf.Cos(AngleQ) * TheLengthOfVector(LengthA) - LengthE;

                Vector3 LengthOfVc = LengthV / TheLengthOfVector(LengthV) * LengthVc;

                if(Mathf.Abs(LengthVc) < Mathf.Abs(TheLengthOfVector(LengthV * Time.fixedDeltaTime)))
                {
                    ball1.Velocity = Vector3.zero;
                }
                
                //Ball1Location = Ball1Location + LengthOfVc;

                //ball1.Velocity = LengthOfVc;

            }
        }
        }
}

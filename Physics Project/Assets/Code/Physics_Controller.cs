using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physics_Controller : MonoBehaviour
{
    [SerializeField]Physics_Sphere[] Spheres = new Physics_Sphere[10];
    [SerializeField]Physics_Plane[] Planes = new Physics_Plane[10];
    [Range(0f,3f)]
    [SerializeField] int CollisionType;
    bool Stop = false;
    float DotProduct(Vector3 a, Vector3 b)
    {
        return (a.x * b.x) + (a.y * b.y) + (a.z * b.z);
    }
    float TheLengthOfVector(Vector3 a)
    {
        return Mathf.Sqrt(Mathf.Pow(a.x, 2) + Mathf.Pow(a.y, 2) + Mathf.Pow(a.z, 2));
    }

    Vector3 FloatXVector(Vector3 a, float b)
    {
        Vector3 c = new Vector3(a.x * b, a.y * b, a.z * b);
        return c;
    }

    Vector3 VectorPlusVector(Vector3 a, Vector3 b)
    {
        Vector3 c = new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        return c;
    }

    Vector3 VectorDivideVector(Vector3 a, Vector3 b)
    {
        Vector3 c = new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
        Debug.Log("c: " + c);
        if(c.x != c.x)
        {
            c.x = 0;
            Debug.Log("x changed");
        }
        if(c.y != c.y)
        {
            c.y = 0;
            Debug.Log("y changed");
        }
        if(c.z != c.z)
        {
            c.z = 0;
            Debug.Log("z changed");
        }
        return c;
    }

    Vector3 GetDistances(Physics_Sphere Ball1, Physics_Sphere Ball2)
    {
        Vector3 Ball1Location = Ball1.GetLocation();
        Vector3 Ball2Location = Ball2.GetLocation();
        return Ball1Location - Ball2Location;  
    }

    float GetSumOFRadii(Physics_Sphere Ball1, Physics_Sphere Ball2)
    {
        float RadiiBall1 = Ball1.Radius;
        float RadiiBall2 = Ball2.Radius;
        return RadiiBall1 + RadiiBall2;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Stop) return;

        switch (CollisionType)
        {
            case 1:
                CollisionStationarySphere(Spheres[0], Spheres[1]);
                break;

            case 2:
                CollisionPlane(Spheres[0], Planes[0]);
                break;

            case 3:
                CollisionMovingSphere(Spheres[0], Spheres[1]);
                break;

            default:
                Debug.LogError("You forgot to put a number for what type of collision is going on baka");
                break;
        }

    }
    void CollisionMovingSphere(Physics_Sphere Ball1, Physics_Sphere Ball2)
    {
        //getting the two Balls locations.
        Vector3 Ball1Location = Ball1.GetLocation();
        Vector3 Ball2Location = Ball2.GetLocation();
        //gets the Balls velocity.
        Vector3 Ball1Velocity = Ball1.Velocity;
        Vector3 Ball2Velocity = Ball2.Velocity;
        //getting Balls radius and then adding them together to get min distance for collision.
        float SumOfRadii = GetSumOFRadii(Ball1, Ball2);

        //|(Ball1Location + tBall1Velocity) - (Ball2Location + tBall2Veloity)| = SumOfRadii
        //Ball1Location = a1i + b1j + c1k, Ball2Location = a2i + b2j + c2k, Ball1Velocity = d1i + e1j + f1k, Ball2Velocity = d2i + e2j + f2k
        //|(a1i + b1j + c1k + td1i + te1j + tf1k) - (a2i + b2j + c2k + td2i + te2j + tf2k)|
        //|(a1 - a2 + td1 - td2)i + (b1 - b2 + te1 - te2)j + (c1 - c2 + tf1 - tf2)k|
        //Deltaxp = a1 - a2, Deltayp = b1 - b2, Deltazp = c1 - c2, Deltaxv = d1 - d2, Deltayv = e1 - e2, Deltazv = f1 - f2
        //Mathf.sqrt(Mathf.Pow(Deltaxp + Deltaxv, 2) + Mathf.Pow(Deltayp + Deltayv, 2) + Mathf.Pow(Deltazp + Deltazv, 2) = SumOfRadii
        //(Mathf.Pow(Deltaxp + tDeltaxv, 2) + Mathf.Pow(Deltayp + tDeltayv, 2) + Mathf.Pow(Deltazp + tDeltazv, 2) = Mathf.Pow(SumOfRadii, 2)
        //Mathf.Pow(t, 2)(Mathf.Pow(Deltaxv, 2) + Mathf.Pow(Deltayv, 2) + Mathf.Pow(Deltazv, 2) + t(2 * Deltaxp * Deltaxv + 2 * Deltayp * Deltayv + 2 * Deltazp * Deltazv) + Mathf.Pow(Deltaxp, 2) + Mathf.Pow(Deltayp, 2) Mathf.Pow(Deltazp, 2) - Mathf.Pow(SumOfRadii, 2) = 0
        //A = Mathf.Pow(Deltaxv, 2) + Mathf.Pow(Deltayv, 2) + Mathf.Pow(Deltazv, 2)
        //B = 2 * Deltaxp * Deltaxv + 2 * Deltayp * Deltayv + 2 * Deltazp * Deltazv
        //C = Mathf.Pow(Deltaxp, 2) + Mathf.Pow(Deltayp, 2) + Mathf.Pow(Deltazp, 2) - Mathf.Pow(SumOfRadii, 2)
        //t = -B +- Mathf.Sqrt(Mathf.Pow(B, 2) - 4AC) / 2A
        //need a if to check if 4AC > Mathf.Pow(B, 2)
        //if 1 or more of the t is outside of range (0-1) then no collision this frame.
        //if only one t value then spheres barely touch so no aftermath need

        float Deltaxp = Ball1Location.x - Ball2Location.x;
        float Deltayp = Ball1Location.y - Ball2Location.y;
        float Deltazp = Ball1Location.z - Ball2Location.z;
        float Deltaxv = Ball1Velocity.x - Ball2Velocity.x;
        float Deltayv = Ball1Velocity.y - Ball2Velocity.y;
        float Deltazv = Ball1Velocity.z - Ball2Velocity.z;


        float A = (Deltaxv * Deltaxv) + (Deltayv * Deltayv) + (Deltazv * Deltazv);
        float B = (2 * Deltaxp * Deltaxv) + (2 * Deltayp * Deltayv) + (2 * Deltazp * Deltazv);
        float C = (Deltaxp * Deltaxp) + (Deltayp * Deltayp) + (Deltazp * Deltazp) - (SumOfRadii * SumOfRadii);

        //Debug.Log("A: " + A);
        //Debug.Log("B: " + B);
        //Debug.Log("C: " + C);

        float SqrtPart = (B * B) - (4 * A * C);

        if (SqrtPart <= 0)
        {
            Debug.Log("No Real Values will be made");
            return;
        }

        float t1 = (-B + Mathf.Sqrt(SqrtPart)) / (2 * A);
        float t2 = (-B - Mathf.Sqrt(SqrtPart)) / (2 * A);

        //Debug.Log("t1: " + t1);
        //Debug.Log("t2: " + t2);

        float Min = Mathf.Min(t1, t2);

        if (t1 == t2)
        {
            Debug.Log("barely touch, no collision");
            return;
        }

        if (Min > 0 && Min <= Time.fixedDeltaTime)
        {
            Ball1.transform.position += Ball1Velocity * Min;
            Ball2.transform.position += Ball2Velocity * Min;
            //Ball1.Velocity = Vector3.zero;
            //Ball2.Velocity = Vector3.zero;
            //Stop = true;
            Debug.Log("Collision");
            TwoMovingSphereAfterMath(Ball1, Ball2);
        }

        
    }

    void CollisionPlane(Physics_Sphere Ball, Physics_Plane Plane)
    {
        Vector3 BallLocation = Ball.GetLocation();
        Vector3 ArbitraryPoint = Plane.GetLocation();
        Vector3 DistanceBetweenBallAndPlane = BallLocation  - ArbitraryPoint;

        Vector3 BallVelocity = Ball.Velocity;
        float Radius = Ball.Radius;
        Vector3 Normal = Plane.GetNormal();

        float AngleBetweenVelocityAndNegNormal = Mathf.Acos(DotProduct(-Normal, BallVelocity) / (TheLengthOfVector(-Normal) * TheLengthOfVector(BallVelocity)));

        if (AngleBetweenVelocityAndNegNormal > 90) return;


        float AngleBetweenNormalAndBall = Mathf.Acos(DotProduct(Normal, DistanceBetweenBallAndPlane) / (TheLengthOfVector(Normal) * TheLengthOfVector(DistanceBetweenBallAndPlane)));


        float AngleBetweenPlaneAndDistance = (90 * (Mathf.PI / 180)) - AngleBetweenNormalAndBall;


        Debug.Log("Angle1: " + AngleBetweenNormalAndBall);
        Debug.Log("Angle2: " + AngleBetweenPlaneAndDistance);


        float ClosestDistanceBetweenSphereAndPlane = Mathf.Sin(AngleBetweenPlaneAndDistance) * TheLengthOfVector(DistanceBetweenBallAndPlane);

        Debug.Log("Closest: " + ClosestDistanceBetweenSphereAndPlane);

        float VelocityNeeded = (ClosestDistanceBetweenSphereAndPlane - Radius ) / Mathf.Cos(AngleBetweenVelocityAndNegNormal);
        
        Debug.Log("Vel: " + TheLengthOfVector(BallVelocity));
        Debug.Log("Delta: " + Time.fixedDeltaTime);
        Debug.Log("Velocity Needed: " + VelocityNeeded);
        Debug.Log("Next move: " + Mathf.Abs(TheLengthOfVector(BallVelocity * Time.fixedDeltaTime)));

        if (Mathf.Abs(VelocityNeeded) <= Mathf.Abs(TheLengthOfVector(BallVelocity * Time.fixedDeltaTime)))
        {
            Ball.transform.position += BallVelocity.normalized * VelocityNeeded;
            //Stop = true;
            Debug.Log("Collision");
            SphereToPlaneAfterMath(Ball, Normal);
        }

    }

    void CollisionStationarySphere(Physics_Sphere Ball1, Physics_Sphere Ball2)
    {
        //getting the two Balls locations then seeing how far they are from each other.
        Vector3 DistanceBetweenBalls = GetDistances(Ball1, Ball2);
        //Debug.Log(" Hello: " + DistanceBetweenBalls);

        //getting Balls radius and then adding them together to get min distance for collision.
        float SumOfRadii = GetSumOFRadii(Ball1, Ball2);

        //gets the moving Balls velocity.
        Vector3 MovingBallVelocity = Ball1.Velocity;

        //Debug.Log("Code working");

        //calculating the angle between the distance between the two Balls and the moving Balls velocity.
        float AngleBetweenDistanceAndVelocity = Mathf.Acos(DotProduct(MovingBallVelocity, DistanceBetweenBalls) / (TheLengthOfVector(MovingBallVelocity) * TheLengthOfVector(DistanceBetweenBalls)));

        //Degrees to radians
        AngleBetweenDistanceAndVelocity *= (Mathf.PI / 180);

        //Getting the shortest distance between the Balls.
        float ShortestDistanceThisFrame = Mathf.Sin(AngleBetweenDistanceAndVelocity) * TheLengthOfVector(DistanceBetweenBalls);

        //If shortest distance is bigger then the max distance for collision then there is no collision possible. 
        if (ShortestDistanceThisFrame > SumOfRadii) 
        { 
            Debug.Log("No collision"); 
            return; 
        }

        //Debug.Log("Collision possible");
        //Excess velocity.
        float ExcessVelocity = Mathf.Sqrt(Mathf.Pow((Ball1.Radius + Ball2.Radius), 2) - Mathf.Pow(ShortestDistanceThisFrame, 2));

        //Velocity with out excess.
        float VelocityNeedToHitBall = Mathf.Cos(AngleBetweenDistanceAndVelocity) * TheLengthOfVector(DistanceBetweenBalls) - ExcessVelocity;


        //checks to see if Velocity need is less then the Velocity of the Ball, if soo Ball collides.
        if (Mathf.Abs(VelocityNeedToHitBall) < Mathf.Abs(TheLengthOfVector(MovingBallVelocity * Time.fixedDeltaTime))) 
        {
            //Does aftermath code from Balls colliding.
            //Ball1.transform.position += MovingBallVelocity * Time.fixedDeltaTime;
            //Ball1.Velocity = Vector3.zero;
            //Stop = true;
            Debug.Log("Collision");
            SphereToStationarySphereAfterMath(Ball1, Ball2);
        }
     
    }

    void SphereToPlaneAfterMath(Physics_Sphere Ball, Vector3 Normal)
    {
        Vector3 VelocityMinus = Ball.Velocity;
        Vector3 VelocityMinusHat = VelocityMinus / TheLengthOfVector(VelocityMinus);
        Vector3 VelocityPlusHat = 2 * Normal * DotProduct(Normal, -VelocityMinusHat) + VelocityMinusHat;
        Vector3 VelocityPlus = VelocityPlusHat * TheLengthOfVector(VelocityMinus);
        Ball.Velocity = VelocityPlus;
        Ball.transform.position += VelocityPlusHat * 0.1f;
    }

    void SphereToStationarySphereAfterMath(Physics_Sphere Ball1, Physics_Sphere Ball2)
    {
        Vector3 Velocity1 = Ball1.Velocity;
        Vector3 CentreOfSphere2 = Ball2.GetLocation();
        Vector3 VelocityDirection = VectorDivideVector(Velocity1, Velocity1);
        if (Velocity1.x < 0)
        {
            VelocityDirection.x = -VelocityDirection.x;
        }
        if(Velocity1.y < 0)
        {
            VelocityDirection.y = -VelocityDirection.y;
        }
        if(Velocity1.z < 0)
        {
            VelocityDirection.z = -VelocityDirection.z;
        }
        VelocityDirection *= Ball1.Radius;
        Vector3 CollisionPoint = Ball1.GetLocation() + VelocityDirection;
        Vector3 DirectionOfForce = CentreOfSphere2 - CollisionPoint;

        float AngleBetweenVelocityandForce = Mathf.Acos(DotProduct(Velocity1, DirectionOfForce) / (TheLengthOfVector(Velocity1) * TheLengthOfVector(DirectionOfForce)));
        //AngleBetweenVelocityandForce *= (Mathf.PI / 180);

        Vector3 Velocity2 = Mathf.Cos(AngleBetweenVelocityandForce) * DirectionOfForce;
        Velocity1 -= Velocity2;
        Ball1.Velocity = Velocity1;
        Ball2.Velocity = Velocity2;
        //Ball1.transform.position += Velocity1.normalized * 0.1f;
        //Ball2.transform.position += Velocity2.normalized * 0.1f;
    }

    void TwoMovingSphereAfterMath(Physics_Sphere Ball1, Physics_Sphere Ball2)
    {
        Vector3 Velocity1 = Ball1.Velocity;
        Vector3 Velocity2 = Ball2.Velocity;
        Vector3 CentreOfSphere1 = Ball1.GetLocation();
        Vector3 CentreOfSphere2 = Ball2.GetLocation();
        Vector3 VelocityDirection = VectorDivideVector(Velocity1, Velocity2);
        if (Velocity1.x < 0 && Velocity2.x < 0)
        {
            VelocityDirection.x = -VelocityDirection.x;
        }
        if (Velocity1.y < 0 && Velocity2.y < 0)
        {
            VelocityDirection.y = -VelocityDirection.y;
        }
        if (Velocity1.z < 0 && Velocity2.z < 0)
        {
            VelocityDirection.z = -VelocityDirection.z;
        }
        VelocityDirection *= Ball1.Radius;
        Vector3 CollisionPoint = Ball1.GetLocation() + VelocityDirection;
        Vector3 Impulse1 = (CentreOfSphere2 - CentreOfSphere1) / (TheLengthOfVector(CentreOfSphere2) - TheLengthOfVector(CentreOfSphere1));
        Vector3 Impulse2 = (CentreOfSphere1 - CentreOfSphere2) / (TheLengthOfVector(CentreOfSphere1) - TheLengthOfVector(CentreOfSphere2));

        Vector3 DirectionOfForce1 = CentreOfSphere1 - CollisionPoint;
        Vector3 DirectionOfForce2 = CentreOfSphere2 - CollisionPoint;

        float AngleBetweenVelocityandForce1 = Mathf.Acos(DotProduct(Velocity1, DirectionOfForce1) / (TheLengthOfVector(Velocity1) * TheLengthOfVector(DirectionOfForce1)));
        float AngleBetweenVelocityandForce2 = Mathf.Acos(DotProduct(Velocity2, DirectionOfForce2) / (TheLengthOfVector(Velocity2) * TheLengthOfVector(DirectionOfForce2)));
        //AngleBetweenVelocityandForce *= (Mathf.PI / 180);

        float StrengthOfForce1 = Mathf.Cos(AngleBetweenVelocityandForce1) * TheLengthOfVector(Velocity1);
        float StrengthOfForce2 = Mathf.Cos(AngleBetweenVelocityandForce2) * TheLengthOfVector(Velocity2);

        Velocity1 += StrengthOfForce1 * Impulse1 - StrengthOfForce2 * Impulse2;
        Velocity2 += StrengthOfForce2* Impulse2 -StrengthOfForce1 * Impulse1;


        Ball1.Velocity = Velocity1;
        Ball2.Velocity = Velocity2;
        //Ball1.transform.position += Velocity1.normalized * 0.1f;
        //Ball2.transform.position += Velocity2.normalized * 0.1f;
    }
}
 
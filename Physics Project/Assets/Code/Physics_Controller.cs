using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physics_Controller : MonoBehaviour
{
    [SerializeField]Physics_Sphere[] Spheres = new Physics_Sphere[10];
    [SerializeField]Physics_Plane[] Planes = new Physics_Plane[10];
    [Range(0f,3f)]
    [SerializeField] int CollisionType;
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

    Vector3 GetDistances(Physics_Sphere ball1, Physics_Sphere ball2)
    {
        Vector3 Ball1Location = ball1.GetLocation();
        Vector3 Ball2Location = ball2.GetLocation();
        return Ball1Location - Ball2Location;  
    }

    float GetSumOFRadii(Physics_Sphere ball1, Physics_Sphere ball2)
    {
        float RadiiBall1 = ball1.Radius;
        float RadiiBall2 = ball2.Radius;
        return RadiiBall1 + RadiiBall2;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
    void CollisionMovingSphere(Physics_Sphere ball1, Physics_Sphere ball2)
    {
        //getting the two balls locations.
        Vector3 Ball1Location = ball1.GetLocation();
        Vector3 Ball2Location = ball2.GetLocation();
        //gets the balls velocity.
        Vector3 Ball1Velocity = ball1.Velocity;
        Vector3 Ball2Velocity = ball2.Velocity;
        //getting balls radius and then adding them together to get min distance for collision.
        float SumOfRadii = GetSumOFRadii(ball1, ball2);

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


        float A = Deltaxv * Deltaxv + Deltayv * Deltayv + Deltazv * Deltazv;//100
        float B = (2 * Mathf.Abs(Deltaxp) * Mathf.Abs(Deltaxv)) + (2 * Mathf.Abs(Deltayp) * Mathf.Abs(Deltayv)) + (2 * Mathf.Abs(Deltazp) * Mathf.Abs(Deltazv));//1000
        float C = Deltaxp * Deltaxp + Deltayp * Deltayp + Deltazp * Deltazp - SumOfRadii * SumOfRadii;//2488

        Debug.Log("A: " + A);
        Debug.Log("B: " + B);
        Debug.Log("C: " + C);

        float SqrtPart = (B * B) - (4 * A * C);

        if (SqrtPart < 0)
        {
            Debug.Log("No Real Values will be made");
            return;
        }

        float t1 = (-B + Mathf.Sqrt(SqrtPart)) / (2 * A);//-4.6538 to -0.5024
        float t2 = (-B - Mathf.Sqrt(SqrtPart)) / (2 * A);//-5.3464 to -1.3165


        if (t1 > 1 || t1 < 0 && t2 > 1 || t2 < 0)
        {
            Debug.Log("Out of range, no collision");
            Debug.Log("t1: " + t1);
            Debug.Log("t2: " + t2);
            return;
        }
        
        if (t1 == t2)
        {
            Debug.Log("barely touch, no collision");
            return;
        }

        float CollisionNextFrame = 0;

        if (t1 < t2 && t1 > 0 && t1 < 1)
        {
            //|(Ball1Location + t1 * Ball1Velocity) - (Ball2Location + t1 * Ball2Velocity)| = SumOfRadii;
            CollisionNextFrame = (TheLengthOfVector(Ball1Location) + TheLengthOfVector(Ball1Velocity) * t1) - (TheLengthOfVector(Ball2Location) + TheLengthOfVector(Ball2Velocity) * t1);

        }
        else if(t2 > 0 && t2 < 1)
        {
            //|(Ball1Location + t2 * Ball1Velocity) - (Ball2Location + t2 * Ball2Velocity)| = SumOfRadii;
            CollisionNextFrame = (TheLengthOfVector(Ball1Location) + TheLengthOfVector(Ball1Velocity) * t2) - (TheLengthOfVector(Ball2Location) + TheLengthOfVector(Ball2Velocity) * t2);

        }
       

        if(CollisionNextFrame > SumOfRadii)
        {
            Debug.Log("No collision");
            return;
        }

        ball1.Velocity = Vector3.zero;
        ball2.Velocity = Vector3.zero;
        Debug.Log("Collision");

        //Debug.Log(" Hello: " + DistanceBetweenBalls);







        //Debug.Log("Code working");

        //calculating the angle between the distance between the two balls and the moving balls velocity.
        //float AngleBetweenDistanceAndVelocity = Mathf.Acos(DotProduct(MovingBallVelocity, DistanceBetweenBalls) / (TheLengthOfVector(MovingBallVelocity) * TheLengthOfVector(DistanceBetweenBalls)));

        //Degrees to radians
        //AngleBetweenDistanceAndVelocity *= (Mathf.PI / 180);

        //Getting the shortest distance between the balls.
        //float ShortestDistanceThisFrame = Mathf.Sin(AngleBetweenDistanceAndVelocity) * TheLengthOfVector(DistanceBetweenBalls);

        //If shortest distance is bigger then the max distance for collision then there is no collision possible. 
        //if (ShortestDistanceThisFrame > SumOfRadii)
        //{
        //    Debug.Log("No collision");
        //    return;
        //}

       //Debug.Log("Collision possible");
        //Excess velocity.
        //float ExcessVelocity = Mathf.Sqrt(Mathf.Pow((ball1.Radius + ball2.Radius), 2) - Mathf.Pow(ShortestDistanceThisFrame, 2));

        //Velocity with out excess.
        //float VelocityNeedToHitBall = Mathf.Cos(AngleBetweenDistanceAndVelocity) * TheLengthOfVector(DistanceBetweenBalls) - ExcessVelocity;


        //checks to see if Velocity need is less then the Velocity of the ball, if soo ball collides.
        //if (Mathf.Abs(VelocityNeedToHitBall) < Mathf.Abs(TheLengthOfVector(MovingBallVelocity * Time.fixedDeltaTime)))
        //{
        //    //Velocity needed to hit ball
        //    Vector3 VelocityToHitBallVector = MovingBallVelocity / TheLengthOfVector(MovingBallVelocity) * VelocityNeedToHitBall;

        //    //Does aftermath code from balls colliding.
        //    ball1.Velocity = Vector3.zero;
        //    Debug.Log("Collision");
        //}
    }

    void CollisionPlane(Physics_Sphere ball, Physics_Plane Plane)
    {
        Vector3 BallLocation = ball.GetLocation();
        Vector3 ArbitraryPoint = Plane.GetP1Pos();
        //Vector3 PlaneLocation = Plane.GetLocation();
        Vector3 DistanceBetweenBallAndPlane = BallLocation  - ArbitraryPoint;

        Vector3 BallVelocity = ball.Velocity;
        float Radius = ball.Radius;
        Vector3 Normal = Plane.GetNormal();

        float AngleBetweenVelocityAndNegNormal = Mathf.Acos(DotProduct(-Normal, BallVelocity) / (TheLengthOfVector(-Normal) * TheLengthOfVector(BallVelocity)));
        AngleBetweenVelocityAndNegNormal *= (180 / Mathf.PI);

        Debug.Log("Angle0: " + AngleBetweenVelocityAndNegNormal);

        if (Mathf.Abs(AngleBetweenVelocityAndNegNormal) > 90) return;

        Debug.Log("Heading to plane");


        float AngleBetweenNormalAndBall = Mathf.Acos(DotProduct(Normal, DistanceBetweenBallAndPlane) / (TheLengthOfVector(Normal) * TheLengthOfVector(DistanceBetweenBallAndPlane)));
        AngleBetweenNormalAndBall *= (180 / Mathf.PI);

        Debug.Log("Angle1: " + AngleBetweenNormalAndBall);

        //float AngleBetweenPlaneAndDistance = Mathf.Acos(DotProduct(PlaneLocation.normalized, DistanceBetweenBallAndPlane.normalized));
        //AngleBetweenPlaneAndDistance *= (180 / Mathf.PI);

        float AngleBetweenPlaneAndDistance = 90 - Mathf.Abs(AngleBetweenNormalAndBall);

        Debug.Log("Angle2: " + AngleBetweenPlaneAndDistance);

        if (Mathf.Abs(AngleBetweenNormalAndBall) + Mathf.Abs(AngleBetweenPlaneAndDistance) != 90) Debug.LogError("Failed to make 90");

        float ClosestDistanceBetweenSphereAndPlane = Mathf.Sin(AngleBetweenPlaneAndDistance) * TheLengthOfVector(DistanceBetweenBallAndPlane);

        Debug.Log("Closest: " + ClosestDistanceBetweenSphereAndPlane);

        //if (Mathf.Abs(ClosestDistanceBetweenSphereAndPlane) > Radius)
        //{
        //    Debug.Log("No collision");
        //    return;
        //}

        float VelocityNeeded = (ClosestDistanceBetweenSphereAndPlane - Radius ) / Mathf.Cos(AngleBetweenVelocityAndNegNormal);

        Debug.Log("Velocity Needed: " + VelocityNeeded);

        //Velocity with out excess.
        float VelocityNeedToHitBall = Mathf.Cos(AngleBetweenVelocityAndNegNormal) * TheLengthOfVector(DistanceBetweenBallAndPlane) - VelocityNeeded;

        Debug.Log("Velocity Needed to hit: " + VelocityNeedToHitBall);

        //checks to see if Velocity need is less then the Velocity of the ball, if soo ball collides.
        if (Mathf.Abs(VelocityNeedToHitBall) <= Mathf.Abs(TheLengthOfVector(BallVelocity * Time.fixedDeltaTime)))
        {
            //Velocity needed to hit plane
            Vector3 VelocityToHitBallVector = BallVelocity / TheLengthOfVector(BallVelocity) * VelocityNeeded;

            ball.Velocity = Vector3.zero;
            Debug.Log("Collision");
        }


    }


    void CollisionStationarySphere(Physics_Sphere ball1, Physics_Sphere ball2)
    {
        //getting the two balls locations then seeing how far they are from each other.
        Vector3 DistanceBetweenBalls = GetDistances(ball1, ball2);
        Debug.Log(" Hello: " + DistanceBetweenBalls);

        //getting balls radius and then adding them together to get min distance for collision.
        float SumOfRadii = GetSumOFRadii(ball1, ball2);

        //gets the moving balls velocity.
        Vector3 MovingBallVelocity = ball1.Velocity;

        Debug.Log("Code working");

        //calculating the angle between the distance between the two balls and the moving balls velocity.
        float AngleBetweenDistanceAndVelocity = Mathf.Acos(DotProduct(MovingBallVelocity, DistanceBetweenBalls) / (TheLengthOfVector(MovingBallVelocity) * TheLengthOfVector(DistanceBetweenBalls)));

        //Degrees to radians
        AngleBetweenDistanceAndVelocity *= (Mathf.PI / 180);

        //Getting the shortest distance between the balls.
        float ShortestDistanceThisFrame = Mathf.Sin(AngleBetweenDistanceAndVelocity) * TheLengthOfVector(DistanceBetweenBalls);

        //If shortest distance is bigger then the max distance for collision then there is no collision possible. 
        if (ShortestDistanceThisFrame > SumOfRadii) 
        { 
            Debug.Log("No collision"); 
            return; 
        }

        Debug.Log("Collision possible");
        //Excess velocity.
        float ExcessVelocity = Mathf.Sqrt(Mathf.Pow((ball1.Radius + ball2.Radius), 2) - Mathf.Pow(ShortestDistanceThisFrame, 2));

        //Velocity with out excess.
        float VelocityNeedToHitBall = Mathf.Cos(AngleBetweenDistanceAndVelocity) * TheLengthOfVector(DistanceBetweenBalls) - ExcessVelocity;


        //checks to see if Velocity need is less then the Velocity of the ball, if soo ball collides.
        if (Mathf.Abs(VelocityNeedToHitBall) < Mathf.Abs(TheLengthOfVector(MovingBallVelocity * Time.fixedDeltaTime))) 
        {
            //Velocity needed to hit ball
            Vector3 VelocityToHitBallVector = MovingBallVelocity / TheLengthOfVector(MovingBallVelocity) * VelocityNeedToHitBall;

            //Does aftermath code from balls colliding.
            ball1.Velocity = Vector3.zero; 
            Debug.Log("Collision");  
        }
     
    }
}

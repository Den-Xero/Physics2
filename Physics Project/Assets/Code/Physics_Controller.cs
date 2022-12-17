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

    }

    void CollisionPlane(Physics_Sphere ball, Physics_Plane Plane)
    {
        Vector3 BallLocation = ball.GetLocation();
        Vector3 ArbitraryPoint = Plane.GetP1Pos();
        Vector3 PlaneLocation = Plane.GetLocation();
        Vector3 DistanceBetweenBallAndPlane = ArbitraryPoint - BallLocation;

        Vector3 BallVelocity = ball.Velocity;
        float Radius = ball.Radius;
        Vector3 Normal = Plane.GetNormal();

        float AngleBetweenVelocityAndNegNormal = Mathf.Acos(DotProduct(-Normal, BallVelocity.normalized));
        AngleBetweenVelocityAndNegNormal = (AngleBetweenVelocityAndNegNormal * (180 / Mathf.PI));

        Debug.Log("Angle0: " + AngleBetweenVelocityAndNegNormal);

        if (AngleBetweenVelocityAndNegNormal < -180 && AngleBetweenVelocityAndNegNormal > 180) return;

        Debug.Log("Heading to plane");


        float AngleBetweenNormalAndBall = Mathf.Acos(DotProduct(Normal, DistanceBetweenBallAndPlane.normalized));
        AngleBetweenNormalAndBall *= (180 / Mathf.PI);

        Debug.Log("Angle1: " + AngleBetweenNormalAndBall);

        float AngleBetweenPlaneAndDistance = Mathf.Acos(DotProduct(PlaneLocation.normalized, DistanceBetweenBallAndPlane.normalized));
        AngleBetweenPlaneAndDistance *= (180 / Mathf.PI);

        Debug.Log("Angle2: " + AngleBetweenPlaneAndDistance);

        if (AngleBetweenNormalAndBall + AngleBetweenPlaneAndDistance != 90) Debug.LogError("Failed to make 90");

        float ClosestDistanceBetweenSphereAndPlane = Mathf.Sin(AngleBetweenPlaneAndDistance) * TheLengthOfVector(DistanceBetweenBallAndPlane);


        float VelocityNeeded = (ClosestDistanceBetweenSphereAndPlane - Radius ) / Mathf.Cos(AngleBetweenVelocityAndNegNormal);

        //Velocity with out excess.
        float VelocityNeedToHitBall = Mathf.Cos(AngleBetweenVelocityAndNegNormal) * TheLengthOfVector(DistanceBetweenBallAndPlane) - VelocityNeeded;

        //checks to see if Velocity need is less then the Velocity of the ball, if soo ball collides.
        if (Mathf.Abs(VelocityNeedToHitBall) < Mathf.Abs(TheLengthOfVector(BallVelocity * Time.fixedDeltaTime)))
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

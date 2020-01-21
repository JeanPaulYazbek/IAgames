using UnityEngine;

public class CollisionAvoidance 
{

    Kinetics character;
    float maxAccel;
    Kinetics[] targets;//lista de targets a evadir
    float radius;

    public CollisionAvoidance(Kinetics Character, float MaxAccel, Kinetics[] Targets, float Radius){
        character = Character;
        maxAccel = MaxAccel;
        targets = Targets;
        radius = Radius;
    }

    public SteeringOutput getSteering(SteeringOutput steeringCharacter){

        float shortestTime = float.MaxValue;

        Kinetics firstTarget = new Kinetics(0f, Vector3.zero, character.transform);

        float firstMinSeparation = -1f;;
        float firstDistance = -1f;
        Vector3 firstRelativePos = new Vector3(-1,-1,-1);
        Vector3 firstRelativeVel = new Vector3(-1,-1,-1);

        //----

        Vector3 relativePos;
        Vector3 relativeVel;
        float relativeSpeed;
        float timeToCollision;
        float distance = -1f;
        float minSeparation;
        Kinetics target;
        

        for (int i = 0; i<targets.Length; i++){

            target = targets[i];
            relativePos = target.transform.position - character.transform.position;
            relativeVel = target.velocity - character.velocity;
            relativeSpeed = relativeVel.magnitude;
            timeToCollision = (Vector3.Dot(relativePos,relativeVel))/(relativeSpeed*relativeSpeed);
            
            distance = relativePos.magnitude;
            minSeparation = distance-relativeSpeed*shortestTime;

            if (minSeparation > 2*radius){
                continue;
            }
            
            if (timeToCollision > 0 && timeToCollision < shortestTime){
                shortestTime = timeToCollision;
                firstTarget = target;
                firstMinSeparation = minSeparation;
                firstDistance = distance;
                firstRelativePos = relativePos;
                firstRelativeVel = relativeVel;
            }



        }

        if (firstDistance == -1f){
            return steeringCharacter;
        }

        if (firstMinSeparation <= 0 || distance < 2*radius){
            relativePos = firstTarget.transform.position - character.transform.position;
        }else{
            relativePos = firstRelativePos + firstRelativeVel * shortestTime;
        }

        //Evadimos
        relativePos.Normalize();
        
        SteeringOutput steering = new SteeringOutput(relativePos * maxAccel, 0f);
    
        return steering;


    }



}
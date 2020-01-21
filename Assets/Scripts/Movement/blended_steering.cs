using UnityEngine;
using System;
public class BlendedSteering
{
/**
    Behavior[] behaviors; 
    float maxAccel;
    float maxAngular;

    public BlendedSteering(Behavior[] Behaviors, float MaxAccel, float MaxAngular){
        behaviors = Behaviors;
        maxAccel = MaxAccel;
        maxAngular = MaxAngular;
    }

    public SteeringOutput getSteering(){

        SteeringOutput steering = new SteeringOutput(Vector3.zero, 0f);

        int n = behaviors.Length;
        Behavior behavior;
        SteeringOutput steering_beh;
        for (int i = 0; i < n; i ++){
            behavior = behaviors[i];

            steering_beh = behavior.getSteering();
            steering.linear += behavior.weigth * steering_beh.linear;
            steering.angular += behavior.weigth * steering_beh.angular;
            Debug.Log(steering_beh.angular);
            
        }

        if(steering.linear.magnitude > maxAccel){
            steering.linear.Normalize();
            steering.linear *= maxAccel;
        }

        if(steering.angular > maxAngular){
            steering.angular = maxAngular;
        }
        
        return steering;

        
    }
**/
}
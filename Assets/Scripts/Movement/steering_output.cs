using UnityEngine;
using System;

public class SteeringOutput
{

    public Vector3 linear;

    public float angular = 0f;
    public SteeringOutput(Vector3 Linear, float Angular)
    {
        linear = Linear;
        angular = Angular;
    }

    public void UpdateSteering(SteeringOutput newAccels){
        
        linear = newAccels.linear;
        angular = newAccels.angular;
    }

    
}


using UnityEngine;

public class VelocityMatch : Behavior
{
    public Kinetics character;
    public Kinetics target;
    public float maxAcceleration;
    public float timeToTarget;


    public VelocityMatch(Kinetics Character, Kinetics Target, float MaxAcceleration,  float TimeToTarget) {
        character = Character;
        target = Target;
        maxAcceleration = MaxAcceleration;
        timeToTarget = TimeToTarget;
    }

    //funcion que calcula la aceleracio necesaria para igualar la velocidad de un target
    public override SteeringOutput getSteering(){

       
        //velocidades de salida
        SteeringOutput steering = new SteeringOutput(Vector3.zero, 0f);

        //la aceleracion ira girando hacia la velocidad del target
        steering.linear = target.velocity - character.velocity;

        //la ajustamos
        steering.linear /= timeToTarget;

        if (steering.linear.magnitude > maxAcceleration){
            steering.linear.Normalize();
            steering.linear *= maxAcceleration;
        }

        steering.angular = 0;
        
        return steering;

    } 
        
}

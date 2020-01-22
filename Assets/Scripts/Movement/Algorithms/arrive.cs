
using UnityEngine;

public class Arrive : Behavior
{
    public Kinetics character;
    public Kinetics target;
    public float maxAcceleration;
    public float maxSpeed;

    public float targetRadius;//radio pequenno
    public float slowRadius;//radio grande

    public float timeToTarget;


    public Arrive(Kinetics Character, Kinetics Target, float MaxAcceleration, float MaxSpeed, float TargetRadius, float SlowRadius, float TimeToTarget) {
        character = Character;
        target = Target;
        maxAcceleration = MaxAcceleration;
        maxSpeed = MaxSpeed;
        targetRadius = TargetRadius;
        slowRadius = SlowRadius;
        timeToTarget = TimeToTarget;
    }

   
    public override SteeringOutput getSteering(){

       
        //velocidades de salida
        SteeringOutput steering = new SteeringOutput(Vector3.zero, 0f);

        Vector3 direction = target.transform.position - character.transform.position;
        float distance = direction.magnitude;

        //revisamos si estamos en el radio peque
        if(distance<targetRadius){
            character.velocity = Vector3.zero;
            character.rotation = 0f;
            return steering;//dejamos todo en 0
        }

        float targetSpeed;

        //si estamos fuera del rango grande vamos a toda velocidad
        if(distance > slowRadius){
            targetSpeed = maxSpeed;
        //si estamos dentro del radio grande ajustamos la velocidad
        }else{
            targetSpeed = maxSpeed * distance / slowRadius;
            //fijate que la distancia tiene que ser mas pequenna 
            // que el radio grande aqui
        }

        Vector3 targetVelocity = direction;
        targetVelocity.Normalize();
        targetVelocity *= targetSpeed;

        //aceleramos hacia el objetivo pero poco a poco
        steering.linear = targetVelocity - character.velocity;

        //ajustamos esa aceleracion
        steering.linear /= timeToTarget;

        //si nos pasamos de la aceleracion maxima ponemos la maxima
        if(steering.linear.magnitude > maxAcceleration){
            steering.linear.Normalize();
            steering.linear *= maxAcceleration;
        }

        steering.angular = 0;
        return steering;

    } 
        
}

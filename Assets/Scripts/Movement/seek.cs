using UnityEngine;

public class Seek 
{
    public Kinetics character;
    public Kinetics target;
    public float maxAcceleration;


    public Seek(Kinetics Character, Kinetics Target, float MaxAcceleration) {
        character = Character;
        target = Target;
        maxAcceleration = MaxAcceleration;
    }

    //funcion que realiza seeking de un punto si le pasas 1 y flee si le pasas
    //0
    public SteeringOutput getSteering(int seek_or_flee){

       
        //velocidades de salida
        SteeringOutput steering = new SteeringOutput(Vector3.zero, 0f);

        if (seek_or_flee == 1){
            steering.linear = target.transform.position - character.transform.position;
        }else{
            steering.linear = character.transform.position - target.transform.position;
        }
             
        steering.linear.Normalize();
        //Debug.Log(newVelocity);
        steering.linear *= maxAcceleration;

        steering.angular = 0f;
        //Debug.Log(newVelocity);
        return steering;

    }

    //funcion que realiza seeking de un punto que le pases
    public SteeringOutput getSteering2(Vector3 seekedPos){

       
        //velocidades de salida
        SteeringOutput steering = new SteeringOutput(Vector3.zero, 0f);

        steering.linear = seekedPos - character.transform.position;

             
        steering.linear.Normalize();
        //Debug.Log(newVelocity);
        steering.linear *= maxAcceleration;

        steering.angular = 0f;
        //Debug.Log(newVelocity);
        return steering;

    }
}

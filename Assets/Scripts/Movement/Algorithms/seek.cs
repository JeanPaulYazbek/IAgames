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
    public virtual SteeringOutput getSteering(int seek_or_flee){

       
        return getSteering2(target.transform.position, seek_or_flee);

    }

    //funcion que realiza seeking(si le pasas 1) o flee(si le pasas 0)
    // de la posicion seekedPos
    public SteeringOutput getSteering2(Vector3 seekedPos, int seek_or_flee){

       
        //velocidades de salida
        SteeringOutput steering = new SteeringOutput(Vector3.zero, 0f);

        if (seek_or_flee == 1){
            steering.linear = seekedPos - character.transform.position;
        }else{
            steering.linear = character.transform.position - seekedPos;
        }

             
        steering.linear.Normalize();
        //Debug.Log(newVelocity);
        steering.linear *= maxAcceleration;

        steering.angular = 0f;
        //Debug.Log(newVelocity);
        return steering;

    }
}

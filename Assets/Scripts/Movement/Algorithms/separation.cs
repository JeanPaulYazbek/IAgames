using UnityEngine;

public class Separation {

    Kinetics character;
    Kinetics[] targets;

    float threshold; //que tan cerca empezar a huir

    float maxAccel;

    public Separation(Kinetics Character, Kinetics[] Targets, float Threshold, float MaxAccel){
        character = Character;
        targets = Targets;
        threshold = Threshold;
        maxAccel = MaxAccel;
    }

    public SteeringOutput getSteering(){

        SteeringOutput steering = new SteeringOutput(Vector3.zero, 0f);

        Vector3 direction;//direccion de huida
        float distance;//distancia del target
        float strength;//cuanto aceleraremos para huir
        for (int i = 0; i<targets.Length; i++){

            //revisamos si entraron en el threshold
            strength = 0;
            direction =  character.transform.position - targets[i].transform.position;
            distance = direction.magnitude;

            if (distance < threshold){//estan cerca huimos

                strength = maxAccel * (threshold - distance) /threshold;
            }

            direction.Normalize();
            steering.linear += strength * direction;//aceleramos en huida
        }

        return steering;
    }


}
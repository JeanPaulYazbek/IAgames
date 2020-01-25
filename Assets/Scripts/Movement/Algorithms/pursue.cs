using UnityEngine;

public class Pursue : Seek {

    public Kinetics pTarget;//target del pursue
    float maxPrediction;//cuanto tiempo maximo queremos predecir

    public Pursue(Kinetics Character ,Kinetics PTarget, float MaxPrediction) : base(Character, PTarget,20f){
        pTarget = PTarget;
        maxPrediction = MaxPrediction;
    }

    //funcion que intenta predecir a donde se movera el target e ir
    //antes que el ahi
    public override SteeringOutput getSteering(int seek_or_flee){


        Vector3 direction = pTarget.transform.position - character.transform.position;

        float distance = direction.magnitude;

        float speed= character.velocity.magnitude;

        float prediction;
        // si la velocidad que tengo ahora no me permite recorrer esa distancia
        // en el tiempo de prediccion, (fijate que d / mP es una velocidad hipotetica)
        // pongo maxPrediction para que seek vaya acelere mas
        if (speed <= distance / maxPrediction){
            prediction = maxPrediction;
        // si mi velocidad es ya muy grande simplemente predecimos cuanto tardariamos
        // en recorrer esa distancia con nuestra velocidad actual
        }else{
            prediction = distance / speed;
        }

        //la posicion que estamos pasando es como ver prediction segs en el futuro
        return getSteering2(pTarget.transform.position + pTarget.velocity * prediction, seek_or_flee);
    }



}
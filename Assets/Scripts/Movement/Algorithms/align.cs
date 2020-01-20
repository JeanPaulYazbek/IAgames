using UnityEngine;
using System;
public class Align : Behavior
{
    public Kinetics character;
    public Kinetics target;

    public float maxAngularAccel;
    public float maxRotation;

    //radio de angulo aceptable
    public float targetRadius;
    public float slowRadius;

    public float timeToTarget ;

    public Align(Kinetics Character, Kinetics Target, float MaxAngularAccel, float MaxRotation, float TargetRadius, float SlowRadius, float TimeToTarget){

        character = Character;
        target = Target;
        maxAngularAccel = MaxAngularAccel;
        maxRotation = MaxRotation;
        targetRadius = TargetRadius;
        slowRadius = SlowRadius;
        timeToTarget = TimeToTarget;

    }

    public float mapToRange(float angle){

        float angle2 = angle%360;
         
        if(Math.Abs(angle2) < 180){
            return angle2;
        }

        if(angle2 > 0){
            return angle2 -360;
        }
        return angle2+360;
       
    }

    public virtual SteeringOutput getSteering(){

        return getSteering2(target.transform.eulerAngles.z);

    }

    //funcion que toma una orientacion objetivo y trata de que 
    //el character tenga la misma orientacion cambiando su 
    //acelarion angular
    public SteeringOutput getSteering2(float targetOrientation){

        SteeringOutput steering = new SteeringOutput(Vector3.zero, 0f);

        //sacamos la resta de los angulos para intentar conseguir 
        //lo que nos tenemos que rodar
        float rotation = targetOrientation - character.transform.eulerAngles.z;

        rotation = mapToRange(rotation);
        float rotationSize = Math.Abs(rotation);

        //si ya estamos en el rango de rotacion aceptable salimos
        if (rotationSize < targetRadius){
            
            character.rotation = 0f;//deja de girar
            return steering;//quita acelaraciones
        }

        float targetRotation;
        //si estamos fuera del radio grande giramos rapido
        if(rotationSize > slowRadius){
            targetRotation = maxRotation;
        }else{//si estamos en el radio grande
            targetRotation = maxRotation * rotationSize / slowRadius;
        }

        //ponemos la direccion correcta
        targetRotation *= rotation / rotationSize;

        //en caso de que el target este girando 
        //tratamos de igualar su rotacion con nuestra aceleracion
        steering.angular = targetRotation - character.rotation;

        //ajustamos la aceleracion
        steering.angular /= timeToTarget;

        float angularAccel = Math.Abs(steering.angular);

        //si nos pasamos de la aceleracion maxima ponemos la maxima
        if (angularAccel > maxAngularAccel){
            steering.angular /= angularAccel;
            steering.angular *= maxAngularAccel;
        }

        steering.linear = Vector3.zero;
        return steering;


    }

    
}

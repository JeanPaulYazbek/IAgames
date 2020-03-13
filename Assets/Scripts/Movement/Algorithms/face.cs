using UnityEngine;
using System;

public class Face : Align {

    public Kinetics fTarget;//target del face

    public Face(Kinetics Character ,Kinetics FTarget) : base(Character, FTarget, 20f, 80f, 2f, 30f, 0.1f){
        fTarget = FTarget;
    }

    
    //funcion que te hace girar hacia un target
    override public SteeringOutput getSteering(){

        return getSteeringF2(fTarget.transform.position);
  
    }

    public SteeringOutput getSteeringF2(Vector3 targetPosition){

        //calculamos la direccion a donde tenemos que voltear
        Vector3 direction = targetPosition - character.transform.position;

        if(direction.magnitude == 0){
            // si estamos parados encima no hay forma de voltear hacia el
            return new SteeringOutput(Vector3.zero, 0f);
        }

        float targetOrientation = (float)Math.Atan2(-direction.x, direction.y)*57;

        return getSteering2(targetOrientation);

        
    }



}
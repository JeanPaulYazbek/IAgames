using UnityEngine;
using System;
public class LookWhereYouAreGoing : Align
{

    public LookWhereYouAreGoing(Kinetics Character) : base(Character, Character, 10f, 50f, 2f, 30f, 0.1f){

    }

    //funcion que te hace girar hacia donde vas
    new public SteeringOutput getSteering(){

        if (character.velocity.magnitude==0){
            return new SteeringOutput(Vector3.zero, 0f);
        }

        return getSteering2((float)Math.Atan2(-character.velocity.x,character.velocity.y)*57);
    }
}
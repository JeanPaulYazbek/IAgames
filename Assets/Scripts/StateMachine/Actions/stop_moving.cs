using UnityEngine;
//Accion que permite a un personaje detenerse totalmente
public class StopMoving : Action {

    //Datos del personaje que debe dejar de moverse
    Kinetics kineticsAgent;
    SteeringOutput steeringAgent;

    public StopMoving(Kinetics KineticsAgent, SteeringOutput SteeringAgent){
        kineticsAgent = KineticsAgent;
        steeringAgent = SteeringAgent;
    }

    public override void DoAction(){

        //Dejamos de movernos
        kineticsAgent.velocity = Vector3.zero;
        steeringAgent.linear = Vector3.zero;
        steeringAgent.angular = 0f;

        
    }

}
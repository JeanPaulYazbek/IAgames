using UnityEngine;

// Esta condicion ayudara a saber si cierto pokemon fue atrapado
public class ArrivedToPosition : Condition{

    public Vector3 targetPos;
    Transform agent;
    float arrivalRadius;

    public ArrivedToPosition(Vector3 TargetPos, Transform Agent, float ArrivalRadius){
        targetPos = TargetPos;
        agent = Agent;
        arrivalRadius = ArrivalRadius;
    }

    //Funcion que devuelve true si nos acercamos a la posicion target a cierto radio de distancia
    public override bool Test(){

        Vector3 agentPos = agent.position;
        agentPos.z = 0f;
        return Vector3.Distance(targetPos, agentPos) < arrivalRadius;

    }
}
using UnityEngine;
using System.Collections.Generic;

// Esta condicion ayudara a saber si cierto pokemon fue atrapado
public class ArrivedToStone : Condition{

    Stack<GameObject> items;
    Transform agent;
    float arrivalRadius;

    public ArrivedToStone(Stack<GameObject> Items, Transform Agent, float ArrivalRadius){
        items = Items;
        agent = Agent;
        arrivalRadius = ArrivalRadius;
    }

    //Funcion que devuelve true si nos acercamos a la posicion target a cierto radio de distancia
    public override bool Test(){

        Vector3 agentPos = agent.position;
        agentPos.z = 0f;
        return Vector3.Distance(items.Peek().transform.position, agentPos) < arrivalRadius;

    }
}
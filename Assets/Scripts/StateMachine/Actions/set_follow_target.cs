using UnityEngine;
public class SetArriveTarget : Action {
    Kinetics[] targets;
    ArriveToTarget arrive;

    public SetArriveTarget(Kinetics[] Targets, ArriveToTarget Arrive){
        targets = Targets;
        arrive = Arrive;

    }

    //Accion que de una lista de targets busca el que esta mas cerca del personaje
    //que usa el Arrive y hace que lo sigan a el
    public override void DoAction(){

        float minDistance = float.PositiveInfinity;
        float distance;
        Kinetics closestTarget = null;
        Vector3 characterPos = arrive.arrive.character.transform.position;
        for(int i = 0; i<targets.Length; i++){
            distance = Vector3.Distance(targets[i].transform.position, characterPos) ;
            if(distance< minDistance){
                closestTarget = targets[i];
                minDistance = distance;
            }

        }

        arrive.arrive.target = closestTarget;
    }
}
using UnityEngine;

// Esta condicion ayudara a saber si dos agentes estan muy cerca
public class TooClose : Condition{

    Kinetics character;
    Kinetics target;

    float range;//este numero nos dice que tan cerca es muy cerca

    public TooClose(Kinetics Character, Kinetics Target, float Range){
        character = Character;
        target = Target;
        range = Range;
    }

    //Funcion que devuelve true si el character y el target estan muy cerca
    public override bool Test(){

        Vector3 agentPos = character.transform.position;
        agentPos.z = 0f;
        return Vector3.Distance(agentPos, target.transform.position) < range;

    }
}
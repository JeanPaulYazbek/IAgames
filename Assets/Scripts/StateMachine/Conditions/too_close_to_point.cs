using UnityEngine;

// Esta condicion ayudara a saber si dos agentes estan muy cerca
public class TooCloseToPoint : Condition{

    Vector3 point;
    Kinetics target;

    float range;//este numero nos dice que tan cerca es muy cerca

    public TooCloseToPoint(Vector3 Point, Kinetics Target, float Range){
        point = Point;
        target = Target;
        range = Range;
    }

    //Funcion que devuelve true si el character y el target estan muy cerca
    public override bool Test(){

        
        return Vector3.Distance(point, target.transform.position) < range;

    }
}
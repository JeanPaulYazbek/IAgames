using UnityEngine;
//Condicion para saber si el proximo condo al que nos movemos 
//con follow esta en el aire
public class TargetPointEarth : Condition {

    Transform transform;
    bool wasOnEarth = false;

    public TargetPointEarth(Transform Tranform){
        transform = Tranform;
    }

    public override bool Test(){

        if(!wasOnEarth && transform.position.z >= -0.7){
            
            wasOnEarth = true;
            return true;
        }

        

        wasOnEarth = false;
        return false;

    }
}
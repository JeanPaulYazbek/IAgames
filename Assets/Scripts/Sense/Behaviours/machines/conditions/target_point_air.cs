using UnityEngine;
//Condicion para saber si el proximo condo al que nos movemos 
//con follow esta en el aire
public class TargetPointAir : Condition {

    Transform transform;
    bool wasOnAir = false;

    public TargetPointAir(Transform Transform){
        transform = Transform;
    }

    public override bool Test(){

        
        //Si no estaba en aire y ahora esta volando
        if(!wasOnAir && transform.position.z < -0.5){
            //Ahora esta en aire
            wasOnAir = true;
            return true;
        }

        wasOnAir = false;
        return false;
    }
}
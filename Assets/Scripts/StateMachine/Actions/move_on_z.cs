using UnityEngine;
//Accion que permite ocultar un icono (un corazon por ejemplo)
//que sea un objeto hijo del objeto agent
public class MoveOnZ : Action {

    Transform transform;
    float zCoord;

    public MoveOnZ(Transform Transform, float ZCoord){
        transform = Transform;
        zCoord = ZCoord;
        
    }

    //Funcion que mete al dueño del transform bajo tierra
    public override void DoAction(){
        Vector3 targetPos = transform.position;
        targetPos.z = zCoord;

        transform.position = targetPos;
    }

}
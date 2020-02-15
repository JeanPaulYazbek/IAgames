using UnityEngine;
//Accion que permite seguir un camino de puntos
public class FollowPathOfPoints : Action {


    SteeringOutput steeringAgent;//steering de quien vaya a seguir el camino
    Kinetics agentKin;
    Seek seek;//seek que usaremos para seguir el punto, debe venir inicializado con los datos del dueño del
                //steeringAgent
    Vector3[] path;//arreglo de puntos a seguir, al menos un punto es necesario
    int currentIndexPoint;//indice del punto que estamos siguiendo en este momento
    Vector3 currentTargetPoint;//hacia donde vamos

    public FollowPathOfPoints(SteeringOutput SteeringAgent, Seek Seek, Vector3[] Path){
        steeringAgent = SteeringAgent;
        seek = Seek;
        agentKin = seek.character;
        path = Path;



    }

    public void UpdatePath(Vector3[] newPath){
        path = newPath;
        currentIndexPoint = 0;
        currentTargetPoint = path[0];
    }

    //en este caso la accion es actualizar la acelaracion del usuario
    public override void DoAction(){

        //si nos acercamos mucho al punto actual pasamos al siguiente
        if(Vector3.Distance(currentTargetPoint, agentKin.transform.position)<5f){
            currentIndexPoint++;
            int n = path.Length;
            if(currentIndexPoint == n){//si nos pasamos del largo del path 
                currentIndexPoint = n -1;//nos quedamos en el ultimo
            }
            currentTargetPoint = path[currentIndexPoint];//siguiente triangulo a seguir
        }

        //seguimos el punto actual
        steeringAgent.UpdateSteering(seek.getSteering2(currentTargetPoint,1));
    }
}
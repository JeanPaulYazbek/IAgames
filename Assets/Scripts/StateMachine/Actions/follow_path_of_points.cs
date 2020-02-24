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

    Vector3 oldTargetPoint;

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

        Vector3 agent = agentKin.transform.position;
        Vector3 target = currentTargetPoint;
        target.z = agent.z;
        float modifier = 1f;//este numero sera util para que el seek no se pase mucho de los puntos        

        //si nos acercamos mucho al punto actual pasamos al siguiente
        if(Vector3.Distance(target, agent)<5f){

            currentIndexPoint++;
            int n = path.Length;
            oldTargetPoint = target;
            if(currentIndexPoint == n){//si nos pasamos del largo del path 
                currentIndexPoint = n -1;//nos quedamos en el ultimo
            }
            currentTargetPoint = path[currentIndexPoint];//siguiente triangulo a seguir
        }

        if(Vector3.Distance(oldTargetPoint, agent)<5f){//si estamos muy cerca del punto anterior
            modifier = 5f;//aceleraremos mas para cambiar de direccion al siguiente punto bien
        }

        //seguimos el punto actual
        steeringAgent.UpdateSteering(seek.getSteering2(target,1));
        steeringAgent.linear *= modifier;//ajustamos la aceleracion
    }
}
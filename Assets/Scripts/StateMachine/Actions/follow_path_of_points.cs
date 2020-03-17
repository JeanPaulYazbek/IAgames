using UnityEngine;
//Accion que permite seguir un camino de puntos
public class FollowPathOfPoints : Action {


    SteeringOutput steeringAgent;//steering de quien vaya a seguir el camino
    Kinetics agentKin;
    Seek seek;//seek que usaremos para seguir el punto, debe venir inicializado con los datos del dueño del
                //steeringAgent
    Vector3[] path;//arreglo de puntos a seguir, al menos un punto es necesario
    int currentIndexPoint;//indice del punto que estamos siguiendo en este momento
    public Vector3 currentTargetPoint;//hacia donde vamos

    bool underground;//esto solo debe ser true si el personaje que usea esta accion se va a mover bajo tierra como dugtrio

    public Vector3 oldTargetPoint;

    public FollowPathOfPoints(SteeringOutput SteeringAgent, Seek Seek, Vector3[] Path, bool Underground){
        steeringAgent = SteeringAgent;
        seek = Seek;
        agentKin = seek.character;
        path = Path;
        underground = Underground;


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
        if(underground){
            target.z = agent.z;//esta linea hace que sigamos los puntos a la misma altura
        }

        float modifier = 1f;//este numero sera util para que el seek no se pase mucho de los puntos        
        int n = path.Length;

        //ACTUALIZAR PUNTO
        if(Vector3.Distance(target, agent)<4f){

            currentIndexPoint++;
            oldTargetPoint = target;
            if(currentIndexPoint == n){//si nos pasamos del largo del path 
                currentIndexPoint = n -1;//nos quedamos en el ultimo
            }
            currentTargetPoint = path[currentIndexPoint];//siguiente triangulo a seguir
        }

        //ACELERAR MUCHO SI ESTAMOS CAMBIANDO DE PUNTO
        if(Vector3.Distance(oldTargetPoint, agent)<5f){//si estamos muy cerca del punto anterior
            modifier = 5f;//aceleraremos mas para cambiar de direccion al siguiente punto bien
        }

        //SI ESTAMOS EN EL ULTIMO PUNTO NO MODIFICAR ACELERACION
        if(currentIndexPoint == n){
            modifier = 1f;
        }

        
        

        //seguimos el punto actual
        steeringAgent.UpdateSteering(seek.getSteering2(target,1));
        steeringAgent.linear *= modifier;//ajustamos la aceleracion
       
    }

    //esta funcion retorna el ultimo punto del camino de follow
    public Vector3 LastPoint(){

        return (path[path.Length -1]);
    }
}
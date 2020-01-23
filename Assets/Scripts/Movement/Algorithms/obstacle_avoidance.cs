using UnityEngine;

public class ObstacleAvoidance : Seek {

    float avoidDistance;//que tan lejos me voy a alejar de la colision

    float lookAhead;//que tanto en el futuro vere si ocurre una colision

    CollisionDetector collisionDetector;

    SteeringOutput characterSteering;//necesitamos esto para devolver no cambios

    public ObstacleAvoidance(Kinetics Character ,Transform[] Obstacles,float AvoidDistance, float LookAhead, SteeringOutput CharacterSteering) : base(Character, Character,10f){
        avoidDistance = AvoidDistance;
        lookAhead = LookAhead;
        collisionDetector = new CollisionDetector(Obstacles);
        characterSteering = CharacterSteering;

    }

    //funcion que ayuda a obtener una aceleracion que evitara colisiones con obstaculos
    override public SteeringOutput getSteering(){

        
        //GENERAMOS EL VECTOR DETECTOR DE COLISIONES
        //este es el que va al frente del character
        Vector3 rayVector = character.velocity;
        rayVector.Normalize();
        rayVector *= lookAhead;//ahora el vector que detecta colisiones tiene el tamanno que queremos

        
        Collision collision = collisionDetector.GetCollision(character.transform.position,rayVector);

        //si no ocurrio una collision no hay cambios
        if (!collision.collision){
            return characterSteering;
        }
        
        //en otro caso hay que crear el target ficticio
        //hacemos que el character deje de moverse inmediatamente

        character.velocity = Vector3.zero;//disminuimos mucho la velocidad a la que vamos para 
        //que el seek tenga tiempo de acelerar bien

        //calculamos la posicion del target ficticio
        Vector3 targetPos = collision.position+collision.normal*avoidDistance;

        //si alguna cuenta se danna por falta de precision 
        // cableamos la respuesta 
        //vamos en direccion contraria a por donde vinimos
        if ( float.IsNaN(targetPos.x) || float.IsNaN(targetPos.y)){
            targetPos =  character.transform.position + character.velocity * (-1);
        }

        // revisamos si quedamos atrapados en una esquina
        // para eso vemos si la posicion a la que nos movemos 
        //esta dentro de otra elispe
        if( collisionDetector.CheckAllElipse(targetPos)){
            //en este caso volvemos por donde vinimos con cierta aleatoriedad
            targetPos = character.transform.position + rayVector * (-1) ;
            float range = 1f;
            targetPos.x += UnityEngine.Random.Range(-range, range); 
            targetPos.y += UnityEngine.Random.Range(-range, range);
            
        }

        

        

    
        //Debug.Log("Posicion seek");
        //Debug.Log(targetPos);
        Debug.DrawLine(collision.position,targetPos,  Color.blue, 10.0f,true);
        //Debug.DrawLine(Vector3.zero,collision.position,  Color.black, 10.0f,true);
        return getSteering2(targetPos,1);
        

        

    }



}
using UnityEngine;
using System;

public class KineticsShoot
{

    //DATOS INTERNOS DE LA POKEBALL
    public float speed = 0f;//velocidad con la que se lanza la pokeball
    public Vector3 direction;//direccion hacia donde se lanzo la pokeball
    public Vector3 gravity;//vector gravedad 
    public Vector3 velocity;//guardamos la velocidad completa
    public Transform transform;//aqui esta la posicion de la pokeBall

    const float capture_rate = 2f;//radio de la esfera que representa el pokemon

    //DATOS EXTERNOS
    Kinetics[] targets;//lista de targets que queremos golpear
    Transform[] obstacles;//lista de obstaculos con los que podriamos chocar

    //UTILIDADES
    Vector3 zVector = new Vector3(0f,0f,1f);//vector unitario en z de utilidad
    float lastHeight= 0f;//ultima altura que tuvo la ball, es para saber si vamos bajando o subiendo


    //CONSTRUCTOR
    public KineticsShoot(float Speed, Vector3 Direction, Vector3 Gravity, Transform Transform, Kinetics[] Targets, Transform[] Obstacles)
    {
        
        speed = Speed;
        direction = Direction;
        gravity = Gravity;
        transform = Transform;
        velocity = direction*speed;
        targets = Targets;
        obstacles = Obstacles;
       
    }

    //funcion que calcula la nueva posicion de un proyectil
    //si devuelve [0,-1] es que no hay nada especial pasando y puede seguir
    //si devuelve [1,-1] es que el proyectil choco con el suelo y debe desaparecer
    //si devuelve [1,-2] es que el proyectil choco con un obstaculo y debe desaparecer
    //si devuelve [2,n] es que la bola atrapo un pokemon y n es el indice del pokemon en el arreglo
    public int[] UpdateKinetics(float time){

        int[] answer = new int[2];
        //--Actualizamos posicion 
        transform.position +=  velocity*time + gravity*(time*time/2);
        velocity.z += gravity.z * time;//aplicamos la gravedad solo al componente z 


        //--Ajustamos tamanno de la ball a medida que sube
        float targetSize = Math.Abs(transform.position.z)*(0.4f);
        if (targetSize < 0.5f){
            targetSize = 0.5f;
        }
        transform.localScale = new Vector3(targetSize , targetSize , 1f);


        //--Necesitamos hacer que la ball desaparezca si toca el suelo
        float newZ = transform.position.z;

        //OJO por alguna razon en mi unity -3 es arriba en z y 3 es abajo osea 
        //esta invertido el eje z, por eso hago las cosas al reves
        if(newZ >= -0.1f && newZ > lastHeight){//si bajamos y estamos muy cerca del suelo
            
            answer[0]=1;
            answer[1]=-1;
            return answer;
        }

        //--necesitamos revisar si chocamos con algo
        if(checkCollision()){
            answer[0]=1;
            answer[1]=-2;
            return answer;
        }

        //--si atrapamos un pokemon
        if(CheckCatch(answer)){
            answer[0]=2;
            return answer;
        }

        lastHeight = newZ;

        answer[0]=0;
        return answer;
        
    }

    //funcion que revisa si la pokeball esta tocando un pokemon
    //devuelve un bool si es asi, ademas guarda el indice del pokemon
    // en un arreglo que le pasen
    public bool CheckCatch(int[] arr){
        Vector3 pokemonPos;
        for (int i = 0; i<targets.Length; i++){
            pokemonPos = targets[i].transform.position;
            //revisamos si la poke ball esta dentro del radio de captura del pokemon y si 
            //el pokemon no fue atrapado ya
            if (InsideSphere(pokemonPos, capture_rate) && targets[i].transform.localScale.x > 0f){
                arr[1] = i;
                return true;
            }
        }
        return false;
    }

    //funcion que revisa si la ball dentro de una esfera de centro p, radio r
    public bool InsideSphere(Vector3 p, float r){
        Vector3 point  = transform.position;

        float x = (point.x - p.x);
        float y = (point.y - p.y);
        float z = (point.z - p.z);

        if( x*x + y*y + z*z <= r*r){
            return true;
        }

        return false;

    }

    public bool checkCollision(){
        Vector3 point = transform.position;
        Vector3 obsPos;
        float x;
        float y;
        float z;
        Vector3 obsSize;
        float offsetX;
        float offsetY;
        float offsetZ;

        for(int i = 0; i<obstacles.Length; i++){
            //centro 
            
            obsPos = obstacles[i].position;
            x = obsPos.x;
            y = obsPos.y;
            z = obsPos.z;
            //medidas del cubo que representa el obstaculo
            obsSize = obstacles[i].localScale;
            offsetX = obsSize.x/2;
            offsetY = obsSize.y/2;
            offsetZ = -obsSize.z;
            //si estamos dentro del cubo
            
            if( ( x - offsetX < point.x && point.x < x + offsetX ) &&
            ( y - offsetY < point.y && point.y < y + offsetY ) &&
            ( 0 > point.z && point.z > z + offsetZ )){
                return true;
            }
        }

        return false;
    }



    
}

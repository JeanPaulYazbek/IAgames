using UnityEngine;
using System;

public class CollisionDetector {

    //la idea en general es generar un ovalo alrededor del obstaculo
    //luego revisaremos si el vector que nos dan se intersecta con el
    // ovalo (osea que hay una colision) , en verdad no veremos si intersecta
    //solo revisaremos si el vector esta dentro del ovalo (para eso solo necesitamos)
    //la punta del vector que nos den
    //por ultimo solo evadimos al que este mas cerca en un momento dado

    //NOTA: para calcular el ovalo (o la funcion que representa el ovalo)
    // necesitamos la escala del obstaculo para ajustarlo y la posicion del
    // obstaculo que sera el centro del ovalo


    Transform[] obstacles;

    public CollisionDetector(Transform[] Obstacles){
        obstacles = Obstacles;
    }

    //funcion que recibe la posicion del character y el rayo detector de colisiones 
    // y en base a eso devuelve la normal a la elipse y el punto donde ocurre
    // la interseccion
    public Collision GetCollision(Vector3 position, Vector3 rayVector){

        Collision collision = new Collision(Vector3.zero, Vector3.zero, false);

        //PRIMERO vemoas si hay una colision

        //este vector representa al punto del rayo que pudiera caer dentro de la elipse
        Vector3 rayPosition = position + rayVector;
        
        float m = -1;
        if(rayVector.x != 0){//si no es 0 x podemos seguir normal , sino mas adelante cableamos la respuesta
            m = rayVector.y / rayVector.x;//pendiente del rayo
        }


        //algunos datos que necesitaremos de cada obstaculo para 
        //hacer los calculos de elipse
        //OJO: los obstaculos no pueden estar girados o va a fallar

        Vector3 obstacleCenter;
        float obstacleWidth;
        float obstacleHeight;
        Transform obstacle;

        bool inside;//nos dira si colisionaremos con el obstaculos

        Vector3 intersection = Vector3.zero;//punto de interseccion del rayo y el ovalo

        for (int i = 0; i< obstacles.Length; i++ ){

            obstacle = obstacles[i];
            obstacleWidth = obstacle.localScale.x;
            obstacleHeight = obstacle.localScale.y;
            obstacleCenter = obstacle.position;

            

            inside = CheckElipse(rayPosition, obstacleWidth, obstacleHeight, obstacleCenter);

            
            //si no estamos a punto de chocar con este obstaculo intentamos con 
            // el siguiente
            if(!inside){
                continue;
            }

            //Si vamos a chocar hacemos lo siquiente   

            collision.collision = true;//decimos que si hubo colision

            //Para poder saber la normal necesitamos el punto de interseccion
            // btw tambien necesitamos devolver el punto de interseccion

            if(m==-1){//si x era 0 cableamos la respuestas
                collision.position = obstacleCenter + new Vector3(0f, -rayVector.y, 0f);
                collision.normal = new Vector3(0f, -rayVector.y/rayVector.y, 0f);
                return collision;
            
            }

            collision.position = Intersection(position, m, obstacleWidth, obstacleHeight, obstacleCenter);
            //el punto de interseccion menos el centro del ovalo nos da la normal
           
            collision.normal = collision.position - obstacleCenter;
            collision.normal.z = 0f;
            collision.normal.Normalize();

            return collision;

        }

        //en este caso no se encontro colision asi que avisamos con false en el objeto
        return collision;


    }


    //funcion que recibe un punto y los datos de una elipse y revisa si el punto
    //cae dentro de la elipse
    public bool CheckElipse(Vector3 point,float radius1, float radius2, Vector3 center){

        //(y-y0)^2/a^2 + (x-x0)^2/b^2 = 1
        float deltaY = point.y - center.y;
        float deltaX = point.x - center.x;

        return ((deltaY*deltaY)/(radius2*radius2) + (deltaX*deltaX)/(radius1*radius1) )<=1;
    }

    // funcion que toma 
    // UNA RECTA
    // point : punto a traves del cual pasa la recta
    // m : pendiente de la regla
    // UNA ELIPSE
    // radius1 : radio 1 de la elipse
    // radius2 : radio 2 de la elipse
    // center : centro de la elipse
    public Vector3 Intersection(Vector3 point, float m, float radius1, float radius2, Vector3 center){

        //NOTA: toda las cuentas las saque calculando la formula interseccion
        // entre la formula de la elipse y de recta

        //Debug.Log(point);
        //Debug.Log(m);
        //Debug.Log(radius1);
        //Debug.Log(radius2);
        //Debug.Log(center);
        float y0 = point.y;
        float x0 = point.x;
        float y1 = center.y;
        float x1 = center.x;
        float k = y0 - m*x0;

        float k1 = radius1*radius1;
        float k2 = radius2*radius2;

        float k3 = k - y1;

        float A = k2*m*m + k1;
        float B = 2*k2*k3*m - 2*k1*x1;
        float C = k2*k3*k3 + k1*x1*x1 - k1*k2;

        //discriminante
        float disc = (float)Math.Sqrt(B*B - 4*A*C);
        //Debug.Log(B*B - 4*A*C);

        float xA = (-B + disc)/(2*A);
        float xB = (-B - disc)/(2*A);

        //posibles intersecciones
        Vector3 intersectionA = new Vector3(xA, y1+m*xA-m*x1 ,0f);
        Vector3 intersectionB = new Vector3(xB, y1+m*xB-m*x1 ,0f);

        //buscamos la interseccion mas cercana la cual es el objetivo
        //el que este mas cerca del centro de character (point) es nuestra respuesta
        float distanceA = (float)Math.Sqrt((x0-intersectionA.x)+(y0-intersectionA.y));
        float distanceB = (float)Math.Sqrt((x0-intersectionB.x)+(y0-intersectionB.y));

        if(distanceA < distanceB){
            return intersectionA;
        }

        return intersectionB;

    }

}
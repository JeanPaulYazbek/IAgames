using UnityEngine;
using System.Collections.Generic;
using System;

public class Utilities 
{

    //funcion que convierte orientacion en vector
    public Vector3 OrientationToVector(float orientation){

        //hay que dividir entre 53 para convertir los angulos a radianes
        return new Vector3(-(float)Math.Sin(orientation/57), (float)Math.Cos(orientation/57),0f);

    }

    //funcion que toma un vector y un angulo y devuelve el vector
    //rotado esos angulos
    public Vector3 RotateVector(Vector3 vector, float angle){
        float newX = (float) (Math.Cos(angle/57)*vector.x - Math.Sin(angle/57)*vector.y);
        float newY = (float) (Math.Sin(angle/57)*vector.x + Math.Cos(angle/57)*vector.y);
        return new Vector3(newX, newY, 0f);


    }   

    //Esta funcion toma dos point1 y large y orientation y width,  genera un triangulo
    //cuyo primer punto es point1 y los otros dos puntos estan en direccion de la orientacion
    //separados por dos veces width y a una distancia mas o menos large del point1
    //La funcion devolvera true si point2 esta dentro de ese triangulo
    public bool CheckSightCone(Vector3 point1, float large, float width, float orientation, Vector3 point2){

        //conseguimos la direccion de la orientacion
        Vector3 middle = OrientationToVector(orientation);
        //middle.Normalize();
        //ahora tenemos el punto del medio de los dos que buscamos
        middle = middle*large;

        //rotamos el vector del medio obteniendo los que forman el triangulo
        Vector3 point1A = RotateVector(middle, width);
        Vector3 point1B = RotateVector(middle, -width);

        //los hacemos relativos al punto1 ya que antes eran relativos al centro
        point1A += point1;
        point1B += point1;

        //DrawTriangle(point1, point1A, point1B, 2f);
        bool answer = PointInTriangle(point2, point1, point1A, point1B);
        return answer;

    }

    public void DrawTriangle(Vector3 vertexA, Vector3 vertexB, Vector3 vertexC, float duration){

        Debug.DrawLine(vertexA, vertexB, Color.white, duration, true);
        Debug.DrawLine(vertexA, vertexC, Color.white, duration, true);
        Debug.DrawLine(vertexB, vertexC, Color.white, duration, true);

    }

    //Funcion que toma una lista de puntos y un tiempo
    // y dibuja el camino formado por esos puntos durante ese tiempo
    public void DrawPath(Vector3[] path, float duration){

        if(path is null){
            Debug.Log("Oh no, no hay camino");
            return;
        }

        if(path.Length < 2){
            Debug.Log("No puedo dibujarte un camino de un solo punto");
            return;
        }

        for(int i = 1; i<path.Length; i++){
            Debug.DrawLine(path[i], path[i-1], Color.black, duration, true);
        }
    }

    //Funcion que toma un punto p y revisa si esta dentro del triangulo
    //formado por p0 p1 y p2
    public bool PointInTriangle(Vector3 p, Vector3 p0, Vector3 p1, Vector3 p2){
        var s = p0.y * p2.x - p0.x * p2.y + (p2.y - p0.y) * p.x + (p0.x - p2.x) * p.y;
        var t = p0.x * p1.y - p0.y * p1.x + (p0.y - p1.y) * p.x + (p1.x - p0.x) * p.y;

        if ((s < 0) != (t < 0))
            return false;

        var A = -p1.y * p2.x + p0.y * (p2.x - p1.x) + p0.x * (p1.y - p2.y) + p1.x * p2.y;

        return A < 0 ?
                (s <= 0 && s + t >= A) :
                (s >= 0 && s + t <= A);
    }

    // Funcion que dice si ocurre una interseccion entre dos segmentos
    // de rectas: (p1, p2) y (p3, p4)
    public bool LineSegmentIntersection(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    {
        // Get the segments' parameters.
        float dx12 = p2.x - p1.x;
        float dy12 = p2.y - p1.y;
        float dx34 = p4.x - p3.x;
        float dy34 = p4.y - p3.y;

        // Solve for t1 and t2
        float denominator = (dy12 * dx34 - dx12 * dy34);

        float t1 = ((p1.x - p3.x) * dy34 + (p3.y - p1.y) * dx34) / denominator;

        bool segments_intersect = false;

        if (float.IsInfinity(t1))
        {
            return segments_intersect;
        }


        float t2 = ((p3.x - p1.x) * dy12 + (p1.y - p3.y) * dx12) / -denominator;

        // The segments intersect if t1 and t2 are between 0 and 1.
        segments_intersect = ((t1 >= 0) && (t1 <= 1) && (t2 >= 0) && (t2 <= 1));

        return segments_intersect;

      
    }

    //Funcion que revisa si un segmento de linea choca con algun obstaculo
    //en la lista dada, el segmento de linea viene dado por point1 y point2
    public bool LineSegmentIntersectionObstacle(Vector3 point1, Vector3 point2, obstacle_data[] obstacles){

        obstacle_data obstacle;
        bool intersectsSide;

        //estos modificadores son un truco para hacer que smooth path piense 
        //que los obstaculos son mas grandes de lo que son, esto ayuda a quitarnos
        //casos borde que hacian que los personajes pasaran encima de obstaculos
        Vector3 modifierX = new Vector3(4f,0f,0f);
        Vector3 modifierY = new Vector3(0f,4f,0f);


        //Vemos si algun obstaculo colisiona con el segmento de recta
        for(int i = 0; i < obstacles.Length; i++){
            obstacle = obstacles[i];
            
            //vemos si choca con algun lado del obstaculo
            intersectsSide = LineSegmentIntersection(point1, point2, obstacle.upLeft - modifierX, obstacle.upRight + modifierX)
            || LineSegmentIntersection(point1, point2, obstacle.upLeft + modifierY, obstacle.downLeft - modifierY)
            || LineSegmentIntersection(point1, point2, obstacle.upRight + modifierY, obstacle.downRight - modifierY)
            || LineSegmentIntersection(point1, point2, obstacle.downLeft - modifierX, obstacle.downRight + modifierX);

            if(intersectsSide){//si choco con alguno dejamos de revisar 
                return true;
            }

        }

        //no hubo colisiones
        return false;

    }

    //Funcion que revisa si un segmento de linea choca con algun obstaculo
    //en la lista dada, el segmento de linea viene dado por point1 y point2
    //ademas solo considerara que intersecta si el obstaculo tiene alguno de
    //los tipos en valid type
    public bool LineSegmentIntersectionObstacleType(Vector3 point1, Vector3 point2, obstacle_data[] obstacles, string[] validObstacles){

        obstacle_data obstacle;
        bool intersectsSide;

       
        //Vemos si algun obstaculo colisiona con el segmento de recta
        for(int i = 0; i < obstacles.Length; i++){
            obstacle = obstacles[i];

            //revisamos que el tipo del obstaculo sea valido, por ejemplo para octillery el agua no es un obstaculo
            bool valid = false;
            for(int j = 0; j<validObstacles.Length; j++){
                //si alguno es valido lo marcamos y salimos
                if(validObstacles[j] == obstacle.type){
                    valid = true;
                    break;
                }
            }

            //si ninguno era valido estudiamos el siguiente obstaculo
            if(!valid){
                continue;
            }
            
            //vemos si choca con algun lado del obstaculo
            intersectsSide = LineSegmentIntersection(point1, point2, obstacle.upLeft , obstacle.upRight)
            || LineSegmentIntersection(point1, point2, obstacle.upLeft, obstacle.downLeft )
            || LineSegmentIntersection(point1, point2, obstacle.upRight, obstacle.downRight)
            || LineSegmentIntersection(point1, point2, obstacle.downLeft, obstacle.downRight);

            if(intersectsSide){//si choco con alguno dejamos de revisar 
                return true;
            }

        }

        //no hubo colisiones
        return false;

    }

}

using System.Collections.Generic;
using UnityEngine;
using System;

public class test : MonoBehaviour
{

    GameObject[] meshes;

    Utilities utilities;
    List<Node> nodes = new List<Node>();
    List<Connection> connections = new List<Connection>();

    // Start is called before the first frame update
    void Start()
    {
        meshes = GameObject.FindGameObjectsWithTag("MyMesh");
        static_mesh target;
        int id = 0;
        for(int i = 0; i< meshes.Length; i++){
            target = meshes[i].GetComponent<static_mesh>();
            // Generamos los dos triangulos a partir de un rectangulo
            nodes.Add(new Node(id, target.upLeft, target.downLeft, target.downRight));
            id++;
            nodes.Add(new Node(id, target.upLeft, target.downRight, target.upRight));
            id++;

        }

        //Ahora vamos a hacer una matriz que representara las coordenadas X y Y
        //para ellos necesitaremos saber cuanto offset tienen los numeros
        //para solo trabajar momentaneamente con coordenadas positivas

        float min = float.PositiveInfinity;//si calculamos la coordenada mas pequenna podremos saber cuantos es el offser
        float max = float.NegativeInfinity;//si calculamos la coordenada mas grande junto al offser sabremos el largo de la matriz
        Vector3 vertexB;
        
        foreach(var node in nodes){
            node.DrawTriangle(40);

            //solo necesitamos un vertice para hacer las comparaciones
            //ya que sabemos que nuestros rectangulos de mayor largo
            //tienen maximo 15 en un largo entonces al final basta con
            //rodar el minimo que consigamos digamos 30 unidades
            vertexB = node.vertexB;

            if(min >  vertexB.x){
                min =  vertexB.x;
            }

            if(min >  vertexB.y){
                min =  vertexB.y;
            }

            if(max <  vertexB.x){
                max =  vertexB.x;
            }

            if(max <  vertexB.y){
                max =  vertexB.y;
            }

        }

        int offset = (int)Math.Abs(min);

        //solo para ser precavidos agreguemosle 30 unidades
        offset += 30;

        //creamos la matriz que representara el plano
        //en cada punto del plano hay una lista
        //en esa lista pondremos los triangulos que tengan un punto
        //medio de alguno de sus lados ahi
        //la tercera coordenada (de largo 2) es para poner los nodos
        //ya que maximo hay 2 nodos en un punto ( un punto es el centro de un lado)
        //Nota: esto es eficiente porque crean la lista no toma O(n^2)
        //c# lo hace rapido porque sabe que no estas usando todos los nodos
        int n = offset + (int)Math.Abs(max);

        Debug.Log(max);
        Debug.Log(offset);
        Debug.Log(n);

        Node[,,] plane = new Node[n,n,2];

        //ahora que la matriz esta creada deberiamos bajar el offset para 
        //que los puntos de verdad no salgan de la amtriz

        offset -= 15;

     
        //RELLENAMOS LA MATRIZ CON LAS LISTAS DE TRIANGULOS
     
        foreach(var node in nodes){

            AllocateNode(plane, node, offset);
        }

        int currentId;
        Node targetNode;
        Connection currentConnection;
        //AHORA PROCEDEMOS A CREAR LAS ARISTAS
        foreach(var node in nodes){

            currentId = node.id;

            targetNode = FindConnection(plane, node.centerAB, currentId, offset);
            if(!(targetNode is null)){
                currentConnection = new Connection(node, targetNode);
                connections.Add(currentConnection);
            }

            targetNode = FindConnection(plane, node.centerAC, currentId, offset);
            if(!(targetNode is null)){
                currentConnection = new Connection(node, targetNode);
                connections.Add(currentConnection);
            }

            targetNode = FindConnection(plane, node.centerBC, currentId, offset);
            if(!(targetNode is null)){
                currentConnection = new Connection(node, targetNode);
                connections.Add(currentConnection);
            }


        }

        //IMPRIMIMOS NUESTRAS CONECCIONES PARA VER QUE SIRVAN
        foreach(var connection in connections){
            connection.DrawConnection(40);
        }

        

        Debug.Log(offset);
        Debug.Log(n);
        
    }

    Node FindConnection(Node[,,] plane, Vector3 center, int id, int offset){

        int n = plane.GetLength(0);//necesitamos el largo de la primera coordenada

        //puntos x y y centrales
        int x = (int)center.x+offset;
        int y = (int)center.y+offset;

        

        //primero estudiamos el centro porque ahi es mas probable que encontremos un vecino
        Node targetA = plane[x,y,0];
        Node targetB = plane[x,y,1];
        if(!(targetA is null) && !(targetB is null)){//si las dos casillas estan ocupada ENCONTRAMOS VECINO
        //pero hay que ver quien es el vecino y quien soy yo

            if(id != targetB.id){//si los id son diferentes entonces el B es el vecino
                return targetB;
            }
            return targetA;

        }

        //SI NO ESTABA EN EL CENTRO BUSCAMOS ALREDEDOR PARA ESTAR SEGUROS

        //variables que ayudaran a revisar alrededor de los centrales
        int currentX = 0;
        int currentY = 0;
        Node current;

        //probamos los ocho puntos alrededor del punto que buscamos para estar seguros
        for(int i = -1; i<2; i++){
            for(int j = -1; j<2; j++){
                currentX = x + i;
                currentY = y + j;

                if(currentX < n && currentY < n && (currentX != 0 && currentY !=0)) {//si no nos salimos de la matriz 
                //OJO: no revisamos el centro osea cuando los dos current son 0 porque ya lo revisamos
                //ademas podemos tomar ventaja de que si no estaba en el centro esta en la primera casilla de otra
                    current = plane[currentX,currentY,0];
                    if(!(current is null)){//si la casilla tiene un nodo ENCONTRAMOS POR FIN UN VECINO
                        return current;
                    }
                }
            }
        }

        //si no hay vecino por el lado que nos dieron devolvemos null
        return null;
    }

    //funcion que toma la matriz de nodos, un nodo y el offset del plano
    //y pone el nodo en el sitio apropiado
    void AllocateNode(Node[,,] plane, Node node, int offset){
        Vector3[] centers = new Vector3[3];
        centers[0] = node.centerAB;
        centers[1] = node.centerAC;
        centers[2] = node.centerBC;

        int x;
        int y;

        for(int i = 0; i< 3; i++){//para cada lado alocamos el nodo
            
            x = (int)centers[i].x + offset;
            y = (int)centers[i].y + offset;

            // Debug.Log("x:");
            
            // Debug.Log(x);
            // Debug.Log("y:");
            // Debug.Log(y);
            if( !(plane[x,y,0] is null)){//si la primera posicion esta ocupado lo ponemos en la segunda
                plane[x,y,1] = node;
            }else{//si no lo ponemos en la primera casilla
                plane[x,y,0] = node;
            }
        }
        

    }


}

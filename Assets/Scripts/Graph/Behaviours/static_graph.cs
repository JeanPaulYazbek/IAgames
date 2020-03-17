using System.Collections.Generic;
using UnityEngine;
using System;

public class static_graph : MonoBehaviour
{

    GameObject[] meshes;//aqui guardaremos todos los rectangulos que se hicieron a mano

    Utilities utilities =  new Utilities();//funciones de utilidad
    

    public bool drawTriangles = false;
    public bool drawConnections = false;

    public Graph graph;//aqui guardaremos el grafo cuando este listo


    // Start is called before the first frame update
    void Awake()
    {
        meshes = GameObject.FindGameObjectsWithTag("MyMesh");
        graph = GenerateGraph(meshes);
        
        //ESTO SIRVE EN CASO DE QUE SOSPECHES QUE ALGUIEN PUSO DOS TRIANGULOS EN EL MISMO LUGAR
        // Vector3 centerC;
        // for(int i = 0; i< amountTriangles; i++){
        //     centerC = graph.GetNode(i).center;
        //     for(int j = 0; j<amountTriangles; j++){
        //         if((Vector3.Distance(graph.GetNode(j).center, centerC )< 0.1f ) && j!=i){
        //             Debug.Log("ARREGLA LOS TRIANGULOS");
        //             
        //             graph.GetNode(i).DrawTriangle(40f);
        //             graph.GetNode(j).DrawTriangle(40f);
        //             return;

        //         }
        //     }
        // }

        //IMPRIMIMOS NUESTRAS CONECCIONES PARA VER QUE SIRVAN
        if(drawConnections){//solo si queremos imprimirlas
            for(int i =0; i<graph.numberNodes; i++){
                foreach(var connection in graph.connections[i]){
                    connection.DrawConnection(40);
                }
            }
        }

        
    }


    //Funcion que toma una lista de rectangulos y genera el grafo de triangulos 
    //acorde a ellos
    Graph GenerateGraph(GameObject[] meshes){
        
        static_mesh target;
        int id = 0;

        //DATOS PARA CREAR EL GRAFO
        int amountTriangles = meshes.Length*2;//tendresmos el dobles de triangulos
        Node[] nodes = new Node[amountTriangles];


        Vector3 center;//centro del rectangulo
        float offsetX;//su ancho
        float offsetY;//su alto
        float zCoord;//altura(util para triangulos voladores)
        //LLENAMOS EL ARREGLO DE TRIANGULOS
        for(int i = 0; i< meshes.Length; i++){
            target = meshes[i].GetComponent<static_mesh>();

            //Buscamos los datos necesarios para los calculos
            center = target.transform.position;
            zCoord = center.z;
            offsetX = Math.Abs(target.transform.localScale.x);
            offsetY = Math.Abs(target.transform.localScale.y);

            // Conseguimos las esquinas
            target.upLeft = target.corner(center, -offsetX, offsetY);
            target.upRight = target.corner(center, offsetX, offsetY);
            target.downLeft = target.corner(center, -offsetX, -offsetY);
            target.downRight = target.corner(center, offsetX, -offsetY);
            
            // Generamos los dos triangulos a partir de un rectangulo
            nodes[id]=new Node(id, target.upLeft, target.downLeft, target.downRight,zCoord, target.type);
            target.id1 = id;
            id++;
            nodes[id]=new Node(id, target.upLeft, target.downRight, target.upRight,zCoord, target.type);
            target.id2 = id;
            id++;

        }


        //GRAFO
        graph = new Graph(nodes);


        //Ahora vamos a hacer una matriz que representara las coordenadas X y Y
        //para ellos necesitaremos saber cuanto offset tienen los numeros
        //para solo trabajar momentaneamente con coordenadas positivas

        float min = float.PositiveInfinity;//si calculamos la coordenada mas pequenna podremos saber cuantos es el offser
        float max = float.NegativeInfinity;//si calculamos la coordenada mas grande junto al offser sabremos el largo de la matriz
        Vector3 vertexB;
        Node node;
        
        for(int i =0; i<amountTriangles; i++){
            node = nodes[i];
            if(drawTriangles){//si queremos dibujar los triangulos 
                node.DrawTriangle(40);
            }
            

            //solo necesitamos un vertice para hacer las comparaciones
            //ya que sabemos que nuestros rectangulos de mayor largo
            //tienen maximo 15 en un largo entonces al final basta con
            //rodar el minimo que consigamos digamos 30 unidades
            vertexB = node.vertexB;

            if(min >  vertexB.x){ min =  vertexB.x;}

            if(min >  vertexB.y){min =  vertexB.y;}

            if(max <  vertexB.x){max =  vertexB.x;}

            if(max <  vertexB.y){max =  vertexB.y;}

        }

        int offset = (int)Math.Round(Math.Abs(min));//esto  es cuanto hay que desplazar todas las coordenadas para que queden positivas

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
        int n = offset + (int)Math.Round(Math.Abs(max));
        List<Node>[,] plane = new List<Node>[n,n];

        //ahora que la matriz esta creada deberiamos bajar el offset para 
        //que los puntos de verdad no salgan de la amtriz
        offset -= 15;

     
        //RELLENAMOS LA MATRIZ CON LAS LISTAS DE TRIANGULOS
     
        for( int i =0; i<amountTriangles; i++){
            node = nodes[i];
            AllocateNode(plane, node, offset);
        }


        int currentId;
        Node targetNode;
        Connection currentConnection;
        //AHORA PROCEDEMOS A CREAR LAS ARISTAS
        for(int i = 0; i<amountTriangles; i++){

            node = nodes[i];
            currentId = node.id;

            targetNode = FindConnection(plane, node.centerAB, currentId, offset);
            if(!(targetNode is null)){
                currentConnection = new Connection(node, targetNode);
                graph.AddConnection(node.id, targetNode.id);
            }

            targetNode = FindConnection(plane, node.centerAC, currentId, offset);
            if(!(targetNode is null)){
                currentConnection = new Connection(node, targetNode);
                graph.AddConnection(node.id, targetNode.id);
            }

            targetNode = FindConnection(plane, node.centerBC, currentId, offset);
            if(!(targetNode is null)){
                currentConnection = new Connection(node, targetNode);
                graph.AddConnection(node.id, targetNode.id);
            }


        }

        //descomentar para dibujar el grafo
        //graph.DrawGraph(40f);
        return graph;

    }


    // Funcion que toma un plano el centro del lado de un nodo, el id del nodo
    // y el offset y devuelve el triangulo vecino si lo hay, sino devuelve null
    Node FindConnection(List<Node>[,] plane, Vector3 center, int id, int offset){

        int n = plane.Length;//necesitamos el largo de la primera coordenada

        //puntos x y y centrales
        int x = (int)Math.Round(center.x)+offset;
        int y = (int)Math.Round(center.y)+offset;

        

        //primero estudiamos el centro porque ahi es mas probable que encontremos un vecino
        Node targetA = plane[x,y][0];
        Node targetB = null;

        if(plane[x,y].Count == 2){//si hay dos triangulos en la lista
            targetB = plane[x,y][1];
        }

        if(!(targetA is null) && !(targetB is null)){//si las dos casillas estan ocupada ENCONTRAMOS VECINO
        //pero hay que ver quien es el vecino y quien soy yo

            if(id != targetB.id){//si los id son diferentes entonces el B es el vecino
                return targetB;
            }
            return targetA;

        }

        //SI NO ESTABA EN EL CENTRO BUSCAMOS ALREDEDOR PARA ESTAR SEGUROS

        //variables que ayudaran a revisar alrededor del punto central
        int currentX = 0;
        int currentY = 0;
        Node current;


        //probamos los ocho puntos alrededor del punto que buscamos para estar seguros
        for(int i = -1; i<2; i++){
            for(int j = -1; j<2; j++){

                
                currentX = x + i;
                currentY = y + j;

  
                if(currentX < n && currentY < n && (i != 0 || j != 0) && !(plane[currentX, currentY] is null)) {//si no nos salimos de la matriz y hay una lista en la casilla
                //OJO: no revisamos el centro osea cuando los dos current son 0 porque ya lo revisamos
                //ademas podemos tomar ventaja de que si no estaba en el centro esta en la primera casilla de otra
                    
                    current = plane[currentX,currentY][0];
                    return current;

                }
            }
        }

        //si no hay vecino por el lado que nos dieron devolvemos null
        return null;
    }

    //funcion que toma la matriz de nodos, un nodo y el offset del plano
    //y pone el nodo en el sitio apropiado
    void AllocateNode(List<Node>[,] plane, Node node, int offset){
        Vector3[] centers = new Vector3[3];
        centers[0] = node.centerAB;
        centers[1] = node.centerAC;
        centers[2] = node.centerBC;

        int x;
        int y;

        for(int i = 0; i< 3; i++){//para cada lado alocamos el nodo
            
            x = (int)Math.Round(centers[i].x) + offset;
            y = (int)Math.Round(centers[i].y) + offset;

       
            if(plane[x,y] is null){//si no hay una lista ahi la creamos
                plane[x,y] = new List<Node>();
            }
            plane[x,y].Add(node);//agregamos el nodo a su lista
        }
        

    }


}

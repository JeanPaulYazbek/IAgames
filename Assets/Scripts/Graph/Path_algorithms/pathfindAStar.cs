using UnityEngine;
using System.Collections.Generic;
using System.Linq; 

public class PathFindAStar{

    Graph graph;//el grafo a analizar
    public Node start;//nodo desde donce queremos e path
    public Node goal;//nodo de destino
    public Heuristic heuristic;//herustica a utilizar

    public PathFindAStar(Graph Graph, Node Start, Node Goal, Heuristic Heuristic){
        graph = Graph;
        start = Start;
        goal = Goal;
        heuristic = Heuristic;
    }

    //Estructura que sera util para analizar los nodos
    public class NodeRecord{
        public Node node;
        public float costSoFar;//costo real que llevamos desde el nodo de comienzo
        public float estimatedTotalCost;//estimado de cuanto cuesta llegar hasta el destino (suma de costSoFar y heuristic)
        public NodeRecord prev;//nodo anterior para recontruir el camino

        public NodeRecord(Node Node, float CostSoFar, float EstimatedTotalCost){
            node = Node;
            costSoFar = CostSoFar;
            estimatedTotalCost = EstimatedTotalCost;
        }
    }

    //Funcion que calcula el minimo de una lista
    public NodeRecord GetSmallestElement(List<NodeRecord> records){
        float min = float.PositiveInfinity;
        float currentCost;
        NodeRecord smallest = null;
        foreach(var record in records){
            currentCost = record.estimatedTotalCost;
            if( currentCost < min){
                min = currentCost;
                smallest = record;
            }
        }

        return smallest;

    }

    public Vector3[] GetPath(){


        //Inicializamos datos necesarios
        int n = graph.numberNodes;
        char[] status = new char[n];//arreglo que tendra 'U' para no visitado, 'O' para abierto, 'C' cara cerrado
        for(int i = 0; i<n; i++){//al principio todos los nodos son no visitados
            status[i] = 'U';
        }
        List<NodeRecord> open = new List<NodeRecord>();

        //Creamos el primer record y abrimos el nodo
        NodeRecord startRecord = new NodeRecord(start, 0f, heuristic.Estimate(start));
        open.Add(startRecord);
        status[start.id] = 'O';


        //--Variables necesarias durante el while:

        //Datos del nodo abierto
        NodeRecord current;
        Node currentNode = null;
        List<Connection> connections;
        //Datos de los nodos vecinos 
        Node endNode;
        float endNodeCost;
        NodeRecord endNodeRecord = null;
        float endNodeHeuristic =0f;
        //Datos utiles para iterar
        int k = 0;
        float newTotal;
        //Datos del goal
        NodeRecord goalRecord = null;

 
        while(open.Count > 0){
            
            //Agarramos el nodo con minimos costo estimado total
            current = GetSmallestElement(open);
            currentNode = current.node;

            //Si alcanzamos el objetivo terminamos el while
            if(currentNode == goal){
                goalRecord = current;
                break;
            }

            //Lados del nodo abierto
            connections = graph.GetConnections(currentNode.id);

            //Para cada vecino
            foreach(var connection in connections){

                //Buscamos el vecino
                endNode = connection.GetToNode();
                endNodeCost = current.costSoFar + connection.cost;

                if(status[endNode.id]=='C'){

                    continue;

                //Si ya estaba abierto
                }else if(status[endNode.id]=='O'){
                    endNodeRecord = open.Find(x => x.node == endNode);
                    if(endNodeRecord.costSoFar <= endNodeCost){//Si el costo que lleva ahora es mas barato 
                        continue;//Estudiamos el siguiente vecino
                    }  
                    //No es necesario recalcular la heuristica, podemos restar para conseguirla
                    endNodeHeuristic = endNodeRecord.estimatedTotalCost - endNodeRecord.costSoFar;
                }else if (status[endNode.id]=='U'){//Si no ha sido visitado
                    endNodeRecord = new NodeRecord(endNode, 0f, 0f);
                    endNodeHeuristic = heuristic.Estimate(endNode);
                }

                //Actualizamos record del vecino
                endNodeRecord.costSoFar = endNodeCost;
                endNodeRecord.estimatedTotalCost = endNodeCost + endNodeHeuristic;
                newTotal = endNodeRecord.estimatedTotalCost;
                endNodeRecord.prev = current;

                
                //Si no estaba abierto hay que insertarlo
                if( status[endNode.id] == 'U'){
                    open.Add(endNodeRecord);
                }


            

            }

            //Cerramos el nodo abierto

            k = 0;
            foreach(var record in open){
                if(record == current){
                    break;
                }
                k++;
            }
            open.RemoveAt(k);
            status[currentNode.id] = 'C';


            
            
            //Descomentar esta linea si quieres debuggear 
            //currentNode.DrawTriangle(40f);
           

        }

        //Si no hay solucion
        if (currentNode != goal){
            return null;
        }

        //Necesitamos saber que tan largo es el camino
        k = 1;
        current = goalRecord;
        while(current != startRecord){
            k++;
            current = current.prev;
        }

        //Si hay solucion reconstruimos el camino
        Vector3[] path = new Vector3[k];

        k =  k - 1;

        current = goalRecord;
        while(current != startRecord){
            path[k] = current.node.center;
            current = current.prev;
            k--;
        }
        path[0] = start.center;

        
        // start.DrawTriangle(40f);
        // goal.DrawTriangle(40f);
        return path;


    }

}
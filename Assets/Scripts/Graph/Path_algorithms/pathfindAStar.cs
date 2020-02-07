using UnityEngine;
using System.Collections.Generic;
using System.Linq; 

public class PathFindAStar{

    Graph graph;//el grafo a analizar
    Node start;//nodo desde donce queremos e path
    Node goal;//nodo de destino
    Heuristic heuristic;//herustica a utilizar

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

    public List<Vector3> GetPath(){


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


    //Variables necesarias durante el while:
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

        int z = 0;
        while(open.Count > 0){
            
            //Agarramos el nodo con minimos costo estimado total
            current = open[0];
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
                    k = 0;
                    //Los insertamos en su lugar ordenado
                    foreach(var record in open){
                        if(record.estimatedTotalCost > newTotal){
                            break;
                        }
                        k++;
                    }

                    

                    //Si debemos insertarlo de ultimo
                    if(k == open.Count){
                        open.Add(endNodeRecord);
                    }else{
                        open.Insert(k, endNodeRecord);
                        status[endNode.id] = 'O';
                    }
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


            //DEBUGGEO
            if(z == 10000){
                break;
            }
            z++;
            
            foreach(var record in open){
                // Debug.Log("Iteration:");
                // Debug.Log(z);
                // Debug.Log("OPENED: ");
                // Debug.Log("id:");
                // Debug.Log(record.node.id);
                // Debug.Log("cost so far:");
                // Debug.Log(record.costSoFar);
                // Debug.Log("total cost");
                // Debug.Log(record.estimatedTotalCost);
                // Debug.Log("prev id:");
                // Debug.Log(record.prev.node.id);
                ;

            }
            currentNode.DrawTriangle(40f);
           

        }

        //Si no hay solucion
        if (currentNode != goal){
            Debug.Log("ID");
            return null;
        }

        //Si hay solucion reconstruimos
        List<Vector3> path = new List<Vector3>();

        current = goalRecord;
        while(current != startRecord){
            path.Add(current.node.center);
            current = current.prev;
        }

        path.Reverse();
        return path;


    }

}
using UnityEngine;
using System.Collections.Generic;
using System.Linq; 

public class PathFindAStar{

    Graph graph;
    Node start;
    Node goal;
    Heuristic heuristic;

    public PathFindAStar(Graph Graph, Node Start, Node Goal, Heuristic Heuristic){
        graph = Graph;
        start = Start;
        goal = Goal;
        heuristic = Heuristic;
    }

    public class NodeRecord{
        public Node node;
        public float costSoFar;
        public float estimatedTotalCost;

        public NodeRecord(Node Node, float CostSoFar, float EstimatedTotalCost){
            node = Node;
            costSoFar = CostSoFar;
            estimatedTotalCost = EstimatedTotalCost;
        }
    }

    public Vector3[] getPath(){


        NodeRecord startRecord = new NodeRecord(start, 0f, heuristic.Estimate(start));
        List<NodeRecord> open = new List<NodeRecord>();
        open.Add(startRecord);
        int n = graph.numberNodes;
        char[] status = new char[](n);//arreglo que tendra 'U' para no visitado, 'O' para abierto, 'C' cara cerrado

        for(int i = 0; i<n; i++){
            status[i] = 'U';
        }

        status[start.id] = 'O';

        NodeRecord current;
        Node currentNode = null;
        List<Connection> connections;

        Node endNode;
        float endNodeCost;
        NodeRecord endNodeRecord = null;
        float endNodeHeuristic;

        int k = 0;
        float newTotal;

    
        while(open.Count > 0){
            
            current = open[0];
            currentNode = current.node;

            if(currentNode == goal){
                break;
            }

            connections = graph.GetConnections(currentNode.id);

            foreach(var connection in connections){

                endNode = connection.GetToNode();
                endNodeCost = current.costSoFar + connection.cost;

                if(status[endNode.id]=='O'){
                    endNodeRecord = open.Find(x => x.node.id == endNode.id);
                    if(endNodeRecord.costSoFar <= endNodeCost){
                        continue;
                    }
                    endNodeHeuristic = endNodeRecord.estimatedTotalCost - endNodeRecord.costSoFar;
                }else{
                    endNodeRecord = new NodeRecord(endNode, 0f, 0f);

                    endNodeHeuristic = heuristic.Estimate(endNode);
                }

                endNodeRecord.costSoFar = endNodeCost;
                endNodeRecord.estimatedTotalCost = endNodeCost + endNodeHeuristic;
                newTotal = endNodeRecord.estimatedTotalCost;
                
                if( status[endNode.id] != 'O'){
                    k = 0;
                    foreach(var record in open){
                        if(record.estimatedTotalCost > newTotal){
                            open.Insert(k, endNodeRecord);
                            status[endNode.id] = 'O';
                        }
                    }


                }


            

            }

            var itemToRemove = open.Single(x => x.node == currentNode);
            open.Remove(endNodeRecord);
            status[currentNode.id] = 'C';

        }

        if (currentNode != goal){
            return null;
        }

        //falta recontruir



    }

}
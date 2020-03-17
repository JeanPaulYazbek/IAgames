using System.Collections.Generic;
using System;
using UnityEngine;

//Esta accion calcula el mejor punto de cobertura en el momento que se llama
//y actualiza Astar para que el target sea ese nodo
public class UpdateAstarBestCoverPoint : Action {

    List<CoverNode> coverPoints;//mejores puntos de cobertura
    Transform character;//personaje que se oculta
    Transform[] enemies;//enemigo del personaje
    obstacle_data[] obstacles;//obstaculos
    string[] validObstacles = new string[]{"Tree1", "Tree2"};
    Utilities utilities = new Utilities();
    Graph graph;//grafo
    PathFindAStar aStar;
    public string[] walkable;//nodos sobre los que puede caminar el personaje

    public UpdateAstarBestCoverPoint(List<CoverNode> CoverPoints, Transform Character, Transform[] Enemies, obstacle_data[] Obstacles, Graph Graph, PathFindAStar Astar, string[] Walkable){
        coverPoints = CoverPoints;
        character = Character;
        enemies = Enemies;
        obstacles = Obstacles;
        graph = Graph;
        aStar = Astar;
        walkable = Walkable;
    }

    public override void DoAction(){

        
        //Aqui guardaremos los mejores candidatos
        List<CoverNode> bestNodes = new List<CoverNode>();

        //CALCULAMOS LOS PUNTOS QUE NO SON VISIBLES PARA ALGUN ENEMIGO
        Vector3 enemyPosition;
        Vector3 nodePosition;
        bool validPoint;
        foreach(var point in coverPoints){
            validPoint = true;
            //Revisamos si el punto es visible para algun enemigo
            foreach(var enemy in enemies){
                enemyPosition = enemy.position;
                nodePosition = point.node.center;
                //Si no hay obstaculos entre el enemigo y el punto entonces no nos sirve
                if(!utilities.LineSegmentIntersectionObstacleType(enemyPosition,nodePosition, obstacles, validObstacles)){
                    validPoint = false;
                    break;
                }
            }
            //Si el punto es valido lo agregamos
            if(validPoint){
                bestNodes.Add(point);
            }
        }

        //AHORA BUSCAMOS EL MAS CERCANO DE LOS MEJORES NODOS
        List<CoverNode> bestNodes2 = new List<CoverNode>();
        float minDistance = float.PositiveInfinity;
        float currentDistance;
        Node closestNode = null;
        Vector3 characterPos = character.position;
        foreach(var node in bestNodes){

            currentDistance = Vector3.Distance(characterPos, node.node.center);

            if(currentDistance < minDistance){
                bestNodes2.Add(node);//guardamos a aqui los candidatos mas cercanos
                closestNode = node.node;
                minDistance = currentDistance;
            }
        }

        //ESCOGEMOS UNO AL AZAR
        System.Random rnd = new System.Random();
        int randomIndex  = rnd.Next(0, bestNodes2.Count);  // creates a number between 1 and 12
        closestNode = bestNodes2[randomIndex].node;
        
        //ACTUALIZAMOS Astar
        aStar.goal = closestNode;
        aStar.start = graph.FindNode(characterPos, walkable);
        aStar.heuristic = new Euclidean(closestNode);
    }

}
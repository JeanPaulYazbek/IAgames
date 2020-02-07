using UnityEngine;
public class Euclidean : Heuristic {

    public Euclidean(Node GoalNode){
        goalNode = GoalNode;
    } 

    public override float Estimate(Node start){

        return Vector3.Distance(start.center, goalNode.center);
    }
}
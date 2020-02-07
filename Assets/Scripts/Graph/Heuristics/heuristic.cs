using UnityEngine;

//esta clase ayudara a implementar heuristicas diferentes en caso de
// que usemos mas de una
public abstract class Heuristic {

    public Node goalNode;//nodo target de la heuristica

    public abstract float Estimate(Node start);//funcion de estimacion


}
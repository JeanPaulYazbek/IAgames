using UnityEngine;

//Action que utiliza A star para calcular una nueva ruta a seguir
//y modfica el camino que esta siguiendo el follow ahora al nuevo
public class UpdateFollowPathWithAstar : Action{

    FollowPathOfPoints follow;
    PathFindAStar aStar;

    SmoothPath smoothPath;

    public UpdateFollowPathWithAstar(FollowPathOfPoints Follow, PathFindAStar AStar, obstacle_data[] obstacles){
        follow = Follow;
        aStar = AStar;
        smoothPath = new SmoothPath(obstacles);
    }

    public override void DoAction(){

        //Conseguimos el camino de A* y ademas lo hacemos mejor con SmoothPath
        Vector3[] newPath = smoothPath.Smooth(aStar.GetPath());

        //Le damos el nuevo camino a follow
        follow.UpdatePath(newPath);
    }


}
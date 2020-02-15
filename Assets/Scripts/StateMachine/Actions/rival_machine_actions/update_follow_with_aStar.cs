//Action que utiliza A star para calcular una nueva ruta a seguir
//y modfica el camino que esta siguiendo el follow ahora al nuevo
public class UpdateFollowPathWithAstar : Action{

    FollowPathOfPoints follow;
    PathFindAStar aStar;

    public UpdateFollowPathWithAstar(FollowPathOfPoints Follow, PathFindAStar AStar){
        follow = Follow;
        aStar = AStar;
    }

    public override void DoAction(){
        follow.UpdatePath(aStar.GetPath());
    }


}
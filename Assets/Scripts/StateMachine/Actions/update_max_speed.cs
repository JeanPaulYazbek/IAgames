//Accion que permite modificar la maxima velocidad de un personaje
public class UpdateMaxSpeed : Action {


    static_data characterData;
    float newSpeed;

    public UpdateMaxSpeed(static_data CharacterData, float NewSpeed){
        characterData = CharacterData;
        newSpeed = NewSpeed;
    }

    //en este caso la accion es actualizar la maxima velocidad del usuario
    public override void DoAction(){
        characterData.maxspeed = newSpeed;
    }
}
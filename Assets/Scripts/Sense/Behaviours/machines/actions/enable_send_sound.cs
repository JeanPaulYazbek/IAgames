public class EnableSendSound : Action {

    send_sound sendSoundComp;

    public EnableSendSound(send_sound SendSoundComp){
        sendSoundComp = SendSoundComp;
    }
    public override void DoAction(){
        
        sendSoundComp.enabled = true;
    }
}
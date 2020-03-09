public class DisableSendSound : Action {

    send_sound sendSoundComp;

    public DisableSendSound(send_sound SendSoundComp){
        sendSoundComp = SendSoundComp;
    }
    public override void DoAction(){
        sendSoundComp.audioToSend.volume = 0f;
        sendSoundComp.enabled = false;
    }
}
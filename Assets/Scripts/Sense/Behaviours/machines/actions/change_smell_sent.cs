public class ChangeSmellSent : Action {

    send_smell smellSender;
    string newSmell;

    public ChangeSmellSent(send_smell SmellSender, string NewSmell){
        smellSender = SmellSender;
        newSmell = NewSmell;
    }

    public override void DoAction(){
        smellSender.UpdateSignalDescription(newSmell);
    }

}
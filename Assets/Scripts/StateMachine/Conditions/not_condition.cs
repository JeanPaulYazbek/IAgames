public class NotCondition : Condition{

    Condition condition;

    public NotCondition(Condition Condition){
        condition = Condition;
    }

    public override bool Test(){

        return !(condition.Test());

    }
}
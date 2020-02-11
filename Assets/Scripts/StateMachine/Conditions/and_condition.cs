public class AndCondition : Condition{

    Condition conditionA;
    Condition conditionB;

    public AndCondition(Condition ConditionA, Condition ConditionB){
        conditionA = ConditionA;
        conditionB = ConditionB;
    }

    public override bool Test(){

        return conditionB.Test() && conditionA.Test();

    }
}
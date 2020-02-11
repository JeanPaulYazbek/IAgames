public class OrCondition : Condition{

    Condition conditionA;
    Condition conditionB;

    public OrCondition(Condition ConditionA, Condition ConditionB){
        conditionA = ConditionA;
        conditionB = ConditionB;
    }

    public override bool Test(){

        return conditionB.Test() || conditionA.Test();

    }
}
using UnityEngine;

public class AttackPlayer : BTNode
{
    private EnemyAttack enemyAttack;
    
    public AttackPlayer(EnemyAttack enemyAttack)
    {
        this.enemyAttack = enemyAttack;
    }

    public override NodeState Evaluate()
    {
        //enemyAttack.Attack();

        state = NodeState.SUCCESS;
        return state;
    }
}

using System.Collections.Generic;

public class Sequence : BTNode
{
    private List<BTNode> children;
    
    public Sequence(List<BTNode> children)
    {
        this.children = children;
    }

    public override NodeState Evaluate()
    {
        foreach (var child in children)
        {
            NodeState childState = child.Evaluate();

            if (childState == NodeState.FAILURE)
            {
                state = NodeState.FAILURE;
                return state;
            }

            if (childState == NodeState.RUNNING)
            {
                state = NodeState.RUNNING;
                return state;
            }
        }

        state = NodeState.SUCCESS;
        return state;
    }
}

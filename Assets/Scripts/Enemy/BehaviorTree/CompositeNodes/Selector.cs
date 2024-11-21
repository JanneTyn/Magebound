using System.Collections.Generic;

public class Selector : BTNode
{
    private List<BTNode> children;
    
    public Selector(List<BTNode> children)
    {
        this.children = children;
    }

    public override NodeState Evaluate()
    {
        foreach (var child in children)
        {
            NodeState childState = child.Evaluate();

            if (childState == NodeState.SUCCESS)
            {
                state = NodeState.SUCCESS;
                return state;
            }

            if (childState == NodeState.RUNNING)
            {
                state = NodeState.RUNNING;
                return state;
            }
        }

        state = NodeState.FAILURE;
        return state;
    }
}

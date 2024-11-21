public abstract class BTNode
{
    public enum NodeState
    {
        SUCCESS,
        FAILURE,
        RUNNING
    }

    protected NodeState state;

    public abstract NodeState Evaluate();
}

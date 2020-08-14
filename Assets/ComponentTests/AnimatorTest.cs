using UnityEngine;

public class AnimatorTest : MonoBehaviourExtended
{
    [Component]
    private Animator animator;

    [GlobalComponent]
    private Animator animatorGlobal;

    [OwnComponent]
    private Animator animatorOwn;

    private void Start()
    {
        Debug.Log("Animator injected " + animator);
    }
}

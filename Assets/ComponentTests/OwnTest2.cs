using UnityEngine;

public class OwnTest2 : MonoBehaviourExtended, ITest
{
    [OwnComponent(Of = "Dead")]
    private Gravedigger gravedigger;

    private void Start()
    {
        if (!Test())
        {
            Debug.LogError("Test failed: " + this);
        }
    }

    public bool Test()
    {
        if (gravedigger != null)
        {
            var name = gravedigger.transform.parent.gameObject.name;
            Debug.Log("Found component Gravedigger in Dead of " + name + " for " + this);
            if (name.Equals("Blue team"))
            {
                return true;
            }
        }
        return false;
    }
}
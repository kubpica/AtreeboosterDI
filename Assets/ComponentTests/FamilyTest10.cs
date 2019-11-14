using UnityEngine;

public class FamilyTest10 : MonoBehaviourExtended, ITest
{
    [FamilyComponent(Of = "Black team", SkipItself = true)]
    private Monster2 monster;

    private void Start()
    {
        if (!Test())
        {
            Debug.LogError("Test failed: " + this);
        }
    }

    public bool Test()
    {
        if (monster != null)
        {
            var name = monster.gameObject.name;
            Debug.Log("Found component Monster2 in " + name + " for " + this);
            if (name.Equals("__app") && monster.transform.parent == null)
            {
                return true;
            }
        }
        return false;
    }
}
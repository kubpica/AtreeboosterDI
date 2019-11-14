using UnityEngine;

public class RootObjectTest11 : MonoBehaviourExtended, ITest
{
    [Root("Raptor")]
    private GameObject go;
    private void Start()
    {
        if (!Test())
        {
            Debug.LogError("Test failed: " + this);
        }
    }

    public bool Test()
    {
        if (go != null)
        {
            var name = go.gameObject.name;
            var sceneName = go.scene.name;
            Debug.Log("Found [Root(\"Raptor\")] GameObject " + name + " of " + this);
            if (name.Equals("Raptor") && sceneName.Equals("_additivly"))
            {
                return true;
            }
        }
        return false;
    }
}
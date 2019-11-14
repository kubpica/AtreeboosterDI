using UnityEngine;

namespace AtreeboosterDemo
{
    public class ParentDemo1 : MonoBehaviourExtended
    {
        [ParentComponent]
        private InventoryDemo spawner;
        private void Start()
        {
            if (!Test())
            {
                Debug.LogError("Test failed: " + this);
            }
        }

        public bool Test()
        {
            if (spawner != null)
            {
                var name = spawner.gameObject.name;
                Debug.Log("Found component InventoryDemo in " + name + " for " + this);
                if (name.Equals("Team Blue"))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
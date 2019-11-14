using UnityEngine;
namespace AtreeboosterDemo
{
    public class ChildDemo1 : MonoBehaviourExtended
    {
        [ChildComponent]
        private InventoryDemo inventory;
        private void Start()
        {
            if (!Test())
            {
                Debug.LogError("Test failed: " + this);
            }
        }

        public bool Test()
        {
            if (inventory != null)
            {
                var name = inventory.gameObject.name;
                Debug.Log("Found component InventoryDemo in " + name + " for " + this);
                if (name.Equals("Inventory holder C"))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
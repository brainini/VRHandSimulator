using UnityEngine;

public class Item : MonoBehaviour
{
    public bool inSlot;
    public Vector3 slotRotation = Vector3.zero;
    public Slot currentSlot;

    public void GrabInventory()
    {
        if (inSlot)
        {
            currentSlot.ItemInSlot = null;
            gameObject.transform.parent = null;
            inSlot = false;
            currentSlot.ResetColor();
            currentSlot = null;
        }
    }
}

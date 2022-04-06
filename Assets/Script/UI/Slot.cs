using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class Slot : MonoBehaviour
{
    public InputDeviceCharacteristics controllerCharacteristics;
    private InputDevice targetDevice;

    public GameObject ItemInSlot;
    public Image slotImage;
    Color originalColor;

    void Start()
    {
        slotImage = GetComponentInChildren<Image>();
        originalColor = slotImage.color;
        TryInitialize();

    }
    void TryInitialize()
    {
        List<InputDevice> devices = new List<InputDevice>();

        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);
        if (devices.Count > 0)
        {
            targetDevice = devices[0];
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Slot / Trigger Stay");
        GameObject obj = other.gameObject;
        if (!IsItem(obj)) return;
        Debug.Log("Slot / It is item.");
        if (ItemInSlot != null) return;
        Debug.Log("Slot / Slot is not empty.");
        
        targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue);
        if (gripValue > 0)
        {
            Debug.Log("Slot / You gripped something");
            InsertItem(obj);
        }
    }

    bool IsItem(GameObject obj)
    {
        return obj.GetComponent<Item>();
    }

    void InsertItem(GameObject obj)
    {
        obj.GetComponent<Rigidbody>().isKinematic = true;
        obj.transform.SetParent(gameObject.transform, true);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localEulerAngles = obj.GetComponent<Item>().slotRotation;
        obj.GetComponent<Item>().inSlot = true;
        obj.GetComponent<Item>().currentSlot = this;
        ItemInSlot = obj;
        slotImage.color = Color.gray;
    }

    public void ResetColor()
    {
        slotImage.color = originalColor;
    }
}

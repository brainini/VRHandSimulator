using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class InventoryVR : MonoBehaviour
{
    public InputDeviceCharacteristics controllerCharacteristics;
    private InputDevice targetDevice;

    public GameObject Inventory;
    public GameObject Anchor;
    bool UIActive;

    private void Start()
    {
        Inventory.SetActive(false);
        UIActive = false;
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

    public void TurnInventory()
    {
        UIActive = !UIActive;
        Inventory.SetActive(UIActive);
    }

    public void GrabInventory()
    {
        if (gameObject.GetComponent<Item>() == null) return;
        if (gameObject.GetComponent<Item>().inSlot)
        {
            gameObject.GetComponentInParent<Slot>().ItemInSlot = null;
            gameObject.transform.parent = null;
            gameObject.GetComponent<Item>().inSlot = false;
            gameObject.GetComponent<Item>().currentSlot.ResetColor();
            gameObject.GetComponent<Item>().currentSlot = null;
        }
    }
    void UpdateHandAnimation()
    {
        targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primarySelect);
        if (primarySelect)
        {
            Debug.Log("primaryButton Selected");
            TurnInventory();
        }
    }
    private void Update()
    {
        if (UIActive)
        {
            Inventory.transform.position = Anchor.transform.position;
            Inventory.transform.eulerAngles = new Vector3(Anchor.transform.eulerAngles.x + 15, Anchor.transform.eulerAngles.y, 0);
        }

        if (!targetDevice.isValid)
        {
            TryInitialize();
        }
        else
        {
            UpdateHandAnimation();
        }
    }
}

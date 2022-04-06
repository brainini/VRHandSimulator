using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class InventoryVR : MonoBehaviour
{
    public InputDeviceCharacteristics controllerCharacteristics;
    public InputDevice targetDevice;
    [SerializeField]
    private GameObject Inventory;
    [SerializeField]
    private GameObject Anchor;
    bool UIActive;
    bool buttonstate = false;

    private void Start()
    {
        Inventory.SetActive(false);
        UIActive = false;
        buttonstate = false;
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

    private void TurnInventory()
    {
        UIActive = !UIActive;
        Inventory.SetActive(UIActive);
    }

    void InventoryCheck()
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
            targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool buttonValue);
            {
                if (buttonValue != buttonstate)
                {
                    InventoryCheck();
                    //do stuff
                    buttonstate = buttonValue;
                }
            }
        }
    }
}

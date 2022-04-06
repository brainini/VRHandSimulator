using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class DebugDisplay : InventoryVR
{
    Dictionary<string, string> debugLogs = new Dictionary<string, string>();
    public GameObject Console;
    public Text display;
    public ScrollRect m_bottomScrollRect;
    bool ConsoleActive;
    private void Start()
    {
        gameObject.SetActive(true);
        ConsoleActive = true;
    }

    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
        StartCoroutine(PushToBottom());
    }
    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }
    IEnumerator PushToBottom()
    {
        yield return new WaitForEndOfFrame();
        m_bottomScrollRect.verticalNormalizedPosition = 0;
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)m_bottomScrollRect.transform);
    }

    private IEnumerator TurnConsole()
    {
        ConsoleActive = !ConsoleActive;
        gameObject.SetActive(ConsoleActive);
        yield return new WaitForSeconds(1f);
    }

    private void UpdateDebugConsole()
    {
        targetDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondarySelect);
        if (secondarySelect)
        {
            Debug.Log("secondaryButton Selected");
            StartCoroutine(TurnConsole());
        }
    }
    private void Update()
    {
        Debug.Log("time: " + Time.time);
        Debug.Log(gameObject.name);

        UpdateDebugConsole();
    }
    void HandleLog(string logString, string stackTree, LogType type)
    {
        if (type == LogType.Log)
        {
            string[] splitString = logString.Split(char.Parse(":"));
            string debugKey = splitString[0];
            string debugValue = splitString.Length > 1 ? splitString[1] : "";

            if (debugLogs.ContainsKey(debugKey))
                debugLogs[debugKey] = debugValue;
            else
                debugLogs.Add(debugKey, debugValue);
        }

        string displayText = "";
        foreach (KeyValuePair<string, string> log in debugLogs)
        {
            if (log.Value == "")
                displayText += log.Key + "\n";
            else
                displayText += log.Key + ": " + log.Value + "\n";
        }
        display.text = displayText;
    }
}

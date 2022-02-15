using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using UnityEngine;
using UnityEngine.Events;

//singleton pattern for panel manager
public class PanelManager : Singleton<PanelManager>
{
    public UnityEvent<PopupType> onPanelShowed = new UnityEvent<PopupType>();
    public UnityEvent<PopupType> onPanelHidden = new UnityEvent<PopupType>();

    public Transform PanelCanvas
    {
        get
        {
            if (panelsParent == null)
            {
                Canvas mainCanvas = FindObjectOfType<MainCanvas>().GetComponent<Canvas>();
                panelsParent = mainCanvas.transform;
            }
            
            return panelsParent;
        }
    }

    private const string prefabPathPrefix = "Panels/";
    private Transform panelsParent;
    private Dictionary<PopupType, BasePanel> panels = new Dictionary<PopupType, BasePanel>();
    private Dictionary<PopupType, BasePanel> loadedPrefabs = new Dictionary<PopupType, BasePanel>();


    //Show whit panel type and panel data
    public void Show(PopupType type, PanelData data)
    {
        var instance = GetPopup(type);

        if (!panels.ContainsKey(type))
            panels.Add(type, instance);

        instance.ShowPanel(data);
        onPanelShowed.Invoke(type);
    }

    public BasePanel GetPopup(PopupType type)
    {
        if (loadedPrefabs.ContainsKey(type))
            return loadedPrefabs[type];

        string panelPath = prefabPathPrefix + type.GetPopupPrefabName();
        BasePanel panelPrefab = Resources.Load<BasePanel>(panelPath);
        BasePanel panel = Instantiate(panelPrefab, PanelCanvas);
        panel.gameObject.SetActive(false);
        loadedPrefabs.Add(type, panel);
        return panel;
    }

    public void Hide(PopupType poolType)
    {
        if (panels.ContainsKey(poolType))
        {
            panels[poolType].HidePanel();
            panels.Remove(poolType);
        }
    }
    
    public void Hide(BasePanel panel)
    {
        if (panels.ContainsValue(panel))
        {
            var pair = panels.First(pair => pair.Value == panel);
            panels[pair.Key].HidePanel();
            panels.Remove(pair.Key);
        }
    }

    public void HideAllPanel()
    {
        foreach (var panel in panels)
        {
            panel.Value.HidePanel();
        }
    }

    public bool IsPanelShowed(PopupType popupType, bool checkStillShowing = true)
    {
        foreach (var panel in panels)
        {
            if (panel.Key == popupType)
            {
                return !checkStillShowing || panel.Value.gameObject.activeSelf;
            }
        }

        return false;
    }

}


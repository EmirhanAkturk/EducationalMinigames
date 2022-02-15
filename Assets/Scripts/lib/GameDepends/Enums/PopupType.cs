using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PopupType 
{
    GamePlayPanel = 1,
    HRBuyStaffPanel = 2,
    HRUpgradeStaffPanel = 3,
    ATMPanel = 4,
    VendingPanel = 5,
    SupplyPanel = 6,
    SwitchScenePanel = 7,
    OffScreenIndicatorPanel = 8,
    TipBoxPanel = 9,
}

public static class PopupTypeExtantions
{
    private static Dictionary<PopupType, string> popupPrefabs = new Dictionary<PopupType, string>()
    {
        {PopupType.GamePlayPanel, "GamePlayPanel"},
        {PopupType.HRBuyStaffPanel, "HRStaffPanel"},
        {PopupType.HRUpgradeStaffPanel, "HRUpgradePanel"},
        {PopupType.ATMPanel, "ATMPanel"},
        {PopupType.VendingPanel, "VendingPanel"},
        {PopupType.SupplyPanel, "SupplyPanel"},
        {PopupType.SwitchScenePanel, "SwitchScenePanel"},
        {PopupType.OffScreenIndicatorPanel, "OffScreenIndicatorPanel"},
        {PopupType.TipBoxPanel, "TipBoxPanel"},
    };
    
    public static string GetPopupPrefabName(this PopupType popupType)
    {
        return popupPrefabs[popupType];
    }
}

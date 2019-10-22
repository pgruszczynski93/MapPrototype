using System;
using System.Collections;
using System.Collections.Generic;
using HGTV.MapsOfOrders;
using UnityEngine;

public class MOMapOrderHouse : MonoBehaviour, ISelectableHouse {
    
    [SerializeField] MOOrderHouseInfo info;
    [SerializeField] Material highlightMat;
    [SerializeField] Material selectionMat;
    [SerializeField] Material normalMaterial;
    [SerializeField] MeshRenderer renderer;
    public void ShowSelectedHouseInfo() {
        Debug.Log("DETAILS: " + info.details);
    }

    public void ChangeHighlightMaterial(HouseAction action) {
        switch (action) {
            case HouseAction.Selected:
                SwitchMaterials(selectionMat);
                break;
            case HouseAction.Undefined:
                SwitchMaterials(normalMaterial);
                break;
            case HouseAction.Highlighted:
                SwitchMaterials(highlightMat);
                break;
            default:
                break;
        }
    }

    void SwitchMaterials(Material toChange) {
        var newMats = renderer.materials;
        for(var i=0; i<newMats.Length; i++) {
            newMats[i] = toChange;
        }

        renderer.materials = newMats;
    }
    
}

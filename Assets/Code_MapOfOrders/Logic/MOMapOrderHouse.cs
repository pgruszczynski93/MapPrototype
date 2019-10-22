using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using HGTV.MapsOfOrders;
using UnityEngine;

public class MOMapOrderHouse : MonoBehaviour {
    [SerializeField] MOOrderHouseInfo info;
    [SerializeField] Material highlightMat;
    [SerializeField] Material selectionMat;
    [SerializeField] Material normalMaterial;
    [SerializeField] MeshRenderer renderer;

    [SerializeField] bool isSelected;
    [SerializeField] MapSelectionType mapSelectionType;

    public bool IsSelcted
    {
        set => isSelected = value;
    }
    void Initialise() {
        mapSelectionType = MapSelectionType.Undefined;
    }

    void Start() {
        Initialise();
    }
    public void ManageSelectedHouse(MapSelectionType state) {
        mapSelectionType = state;

        switch (mapSelectionType) {
            case MapSelectionType.Undefined:
                SwitchMaterials(normalMaterial);
                break;
            case MapSelectionType.Highlight:
                SwitchMaterials(highlightMat);
                break;
            case MapSelectionType.Selection:
                TryToSelectHouse();
                break;
            default:
                break;
        }
    }

    void TryToSelectHouse() {
        if (!isSelected)
            isSelected = true;
        
        SwitchMaterials(selectionMat);
        ShowSelectedHouseInfo();
    }

    void ShowSelectedHouseInfo() {
        Debug.Log("DETAILS: " + info.details);
    }

    void SwitchMaterials(Material toChange) {
        var newMats = renderer.materials;
        for (var i = 0; i < newMats.Length; i++) {
            newMats[i] = toChange;
        }
        renderer.materials = newMats;
    }
}
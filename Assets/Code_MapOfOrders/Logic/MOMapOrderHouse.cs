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
    [SerializeField] SelectionType selectionType;

    public bool IsSelcted
    {
        set => isSelected = value;
    }
    void Initialise() {
        selectionType = SelectionType.Undefined;
    }

    void Start() {
        Initialise();
    }
    public void ManageSelectedHouse(SelectionType state) {
        selectionType = state;

        switch (selectionType) {
            case SelectionType.Undefined:
                SwitchMaterials(normalMaterial);
                break;
            case SelectionType.Highlight:
                SwitchMaterials(highlightMat);
                break;
            case SelectionType.Selection:
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
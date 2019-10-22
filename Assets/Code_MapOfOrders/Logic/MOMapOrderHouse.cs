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

    public bool isSelected;
    public HouseAction houseState;

    void Initialise() {
        houseState = HouseAction.NotSelected;
    }

    void OnEnable() {
        AssignEvents();
    }

    void OnDisable() {
        RemoveEvents();
    }

    void AssignEvents() {
        MOEvents.OnUpdate += UpdateMaterials;
    }

    void RemoveEvents() {
        MOEvents.OnUpdate -= UpdateMaterials;
    }

    void Start() {
        Initialise();
    }

    void UpdateMaterials() {
        if (!isSelected)
            return;

        SwitchMaterials(selectionMat);
    }

    public void ManageSelectedHouse(HouseAction state) {
        houseState = state;

        switch (houseState) {
            case HouseAction.NotSelected:
                SwitchMaterials(normalMaterial);
                break;
            case HouseAction.Highlighted:
                SwitchMaterials(highlightMat);
                break;
            case HouseAction.Selected:
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
using System;
using System.Collections;
using System.Collections.Generic;
using HGTV.MapsOfOrders;
using UnityEngine;

public class MOMapOrderHouse : MonoBehaviour {
    [SerializeField] MOOrderHouseInfo info;
    [SerializeField] Material highlightMat;
    [SerializeField] Material selectionMat;
    [SerializeField] Material normalMaterial;
    [SerializeField] MeshRenderer renderer;

    bool isHouseActive;
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
        
    }

    void RemoveEvents() {
        
    }

    void Start() {
        Initialise();
    }

    public void ManageSelectedHouse(HouseAction state) {
        houseState = state;

        switch (houseState) {
            case HouseAction.NotSelected:
                SwitchMaterials(normalMaterial);
                break;
            case HouseAction.Highlighted:
                SwitchMaterials(highlightMat);
                isHouseActive = false;
                break;
            case HouseAction.Selected:
                SwitchMaterials(selectionMat);
                ShowSelectedHouseInfo();
                isHouseActive = true;
                break;
            default:
                break;
        }
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
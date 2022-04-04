/*
Copyright 2022 TheWindCode

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using UnityEngine;
using TMPro;

using System.Collections.Generic;

public class BuildingsPanel : MonoBehaviour
{
    
    public static BuildingsPanel instance;
    
    [SerializeField] int money;
    [SerializeField] TextMeshProUGUI moneyText;

    [SerializeField] int moneyEarnedEveryTime = 10;
    [SerializeField] float secondsBetweenMoneyEarns = 5f;
    
    [SerializeField] GameObject selectionIconPrefab;
    [SerializeField] BuildingType[] buildingTypes;

    List<BuildingSelectionIcon> buildingSelectionIcons = new List<BuildingSelectionIcon>();

    int lastObjectPrice;
    float nextTimeToEarnMoney;


    void Awake()
    {

        instance = this;

        for (int i = 0; i < buildingTypes.Length; i++)
        {

            GameObject newSelectionIcon = Instantiate(selectionIconPrefab);
            newSelectionIcon.transform.SetParent(transform);

            buildingSelectionIcons.Add(newSelectionIcon.GetComponent<BuildingSelectionIcon>());
            buildingSelectionIcons[buildingSelectionIcons.Count - 1].DisplayInformation(buildingTypes[i]);

        }

        SetMoney(money);

        nextTimeToEarnMoney = Time.time + secondsBetweenMoneyEarns;

    }

    void Update()
    {

        if (Time.time >= nextTimeToEarnMoney)
        {
            SetMoney(money += moneyEarnedEveryTime);
            nextTimeToEarnMoney = Time.time + secondsBetweenMoneyEarns;
        }

    }

    public void PlaceObject(BuildingType type)
    {

        ObjectPlacer.instance.SetObjectPrefab(type.prefab);
        ObjectPlacer.instance.PlacePreviewObject();

        lastObjectPrice = type.price;

    }

    public void PayForObject()
    {

        SetMoney(this.money - lastObjectPrice);

    }
    
    void SetMoney(int newMoney)
    {

        money = newMoney;

        moneyText.text = $"$ {money}";
        UpdateSelectionIcons();

    }

    void UpdateSelectionIcons()
    {

        for (int i = 0; i < buildingSelectionIcons.Count; i++)
        {

            bool setIconGrey = (buildingTypes[i].price > money);
            buildingSelectionIcons[i].SetGrey(setIconGrey);

        }

    }

}

[System.Serializable]
public class BuildingType
{

    public string title;
    public int price;
    public Sprite icon;
    public GameObject prefab;

}
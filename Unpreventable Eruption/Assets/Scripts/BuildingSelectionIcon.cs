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
using UnityEngine.UI;
using TMPro;

public class BuildingSelectionIcon : MonoBehaviour
{
    
    [SerializeField] Image background;
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] Image icon;

    BuildingType type;

    bool grey;


    public void DisplayInformation(BuildingType type)
    {

        this.type = type;

        titleText.text = type.title;
        priceText.text = $"$ {type.price}";
        icon.sprite = type.icon;

    }

    public void SetGrey(bool setGrey)
    {
        
        grey = setGrey;
        
        if (setGrey)
        {
            background.color = new Color(0.4f, 0.4f, 0.4f);
        }
        else
        {
            background.color = new Color(1f, 1f, 1f);
        }

    }

    public void OnSelected()
    {

        if (!grey)
        {
            BuildingsPanel.instance.PlaceObject(type);
        }

    }

}
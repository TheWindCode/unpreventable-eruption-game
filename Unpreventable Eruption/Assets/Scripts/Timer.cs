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

using System.Collections;

public class Timer : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI timeText;

    int minutes;
    int seconds;


    void Awake()
    {

        minutes = 0;
        seconds = 0;

        UpdateTimerUI();

        StartCoroutine(CounterCoroutine());

    }

    public (int minutes, int seconds) StopAndGetTime()
    {

        StopCoroutine(CounterCoroutine());

        return (minutes, seconds);

    }

    IEnumerator CounterCoroutine()
    {

        while (true)
        {

            yield return new WaitForSeconds(1f);

            seconds++;
            if (seconds >= 60)
            {
                minutes++;
                seconds = 0;
            }

            UpdateTimerUI();

        }

    }

    void UpdateTimerUI()
    {

        if (minutes == 0)
            timeText.text = $"{seconds} s";
        else
            timeText.text = $"{minutes} min {seconds} s";

    }

}
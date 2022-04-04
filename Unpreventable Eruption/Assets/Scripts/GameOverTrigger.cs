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

public class GameOverTrigger : MonoBehaviour
{

    [SerializeField] string triggerTag;

    [SerializeField] GameObject gameOverScreen;
    [SerializeField] TextMeshProUGUI timeText;


    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag(triggerTag))
            EndGame();

    }

    void EndGame()
    {

        GetComponent<Collider>().enabled = false;  
        
        (int minutes, int seconds) time = FindObjectOfType<Timer>().StopAndGetTime();
        
        gameOverScreen.SetActive(true);

        if (time.minutes == 0)
        {

            if (time.seconds == 1)
                timeText.text = $"{time.seconds} second";
            else
                timeText.text = $"{time.seconds} seconds";

        }
        else
        {

            string minutePart;
            string secondPart;
            
            if (time.minutes == 1)
                minutePart = $"{time.minutes} minute";
            else
                minutePart = $"{time.minutes} minutes";

            if (time.seconds == 1)
                secondPart = $"{time.seconds} second";
            else
                secondPart = $"{time.seconds} seconds";
            
            timeText.text = $"{minutePart}, {secondPart}";

        }

    }

}
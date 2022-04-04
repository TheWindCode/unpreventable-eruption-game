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
using UnityEngine.InputSystem;

[SelectionBase]
public class CameraController : MonoBehaviour
{

    [SerializeField] float movementSpeed = 1f;
    [SerializeField] Vector3 movementBounds;

    Vector3 movement = Vector3.zero;


    void Update()
    {

        transform.Translate(movement * Time.deltaTime, Space.World);

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -movementBounds.x, movementBounds.x),
            Mathf.Clamp(transform.position.y, -movementBounds.y, movementBounds.y),
            Mathf.Clamp(transform.position.z, -movementBounds.z, movementBounds.z)
        );

    }
    
    public void Move(InputAction.CallbackContext context)
    {

        if (context.performed)
        {

            Vector2 input = context.ReadValue<Vector2>();
            movement = new Vector3(input.x, 0, input.y) * movementSpeed;

        }
        else if (context.canceled)
        {

            movement = Vector3.zero;

        }

    }

}
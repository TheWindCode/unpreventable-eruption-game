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
using UnityEngine.EventSystems;

public class ObjectPlacer : MonoBehaviour
{

    public static ObjectPlacer instance;
    
    [SerializeField] new Camera camera;
    [SerializeField] float rayLenght = 100f;
    [SerializeField] float objectRotationSpeed = 1f;

    [SerializeField] Material buildingAllowedMaterial;
    [SerializeField] Material buildingForbiddenMaterial;

    [SerializeField] int defaultLayer;
    [SerializeField] int previewLayer;

    [SerializeField] LayerMask groundLayer;

    GameObject objectPrefab;
    GameObject previewObject = null;
    PreviewObject previewObjectComponent = null;
    float previewObjectYRotation = 0f;
    
    Vector2 mousePosition;


    void Awake()
    {

        instance = this;

    }
    
    void FixedUpdate()
    {

        if (previewObject != null)
        {
            MovePreviewObjectWithMouse();
            UpdatePreviewObjectMaterial();
        }

    }
    
    public void MousePosition(InputAction.CallbackContext context)
    {

        mousePosition = context.ReadValue<Vector2>();

    }

    public void Scroll(InputAction.CallbackContext context)
    {

        float scrollInput = context.ReadValue<float>();

        if (previewObject != null)
        {

            previewObjectYRotation += scrollInput * objectRotationSpeed;

        }

    }
    
    public void Click(InputAction.CallbackContext context)
    {

        if (context.performed)
        {

            if (previewObject != null)
            {

                if (EventSystem.current.IsPointerOverGameObject())
                {

                    Destroy(previewObject);
                    ResetPreviewObjectInfo();

                }
                else if (!previewObjectComponent.IsCollidingWithOtherObjects())
                {

                    ReleasePreviewObject();

                }

            }

        }

    }

    public void Back(InputAction.CallbackContext context)
    {

        if (context.performed)
        {

            if (previewObject != null)
            {

                Destroy(previewObject);
                ResetPreviewObjectInfo();

            }

        }

    }

    public void SetObjectPrefab(GameObject prefab)
    {

        objectPrefab = prefab;

    }

    public void PlacePreviewObject()
    {

        previewObject = PlaceObject();
        previewObject.layer = previewLayer;
        previewObject.AddComponent<Rigidbody>().isKinematic = true;
        previewObjectComponent = previewObject.AddComponent<PreviewObject>();
        previewObjectComponent.Initialise();

        MovePreviewObjectWithMouse();
        UpdatePreviewObjectMaterial();

    }

    GameObject PlaceObject()
    {

        GameObject objectPlaced = Instantiate(objectPrefab, Vector3.zero, Quaternion.identity);
        return objectPlaced;

    }

    void MovePreviewObjectWithMouse()
    {

        PositionRotation objectPositionRotation = MouseRayPositionRotation();

        if (objectPositionRotation != null)
        {

            previewObjectComponent.rigidbody.position = objectPositionRotation.position;
            previewObjectComponent.rigidbody.rotation = Quaternion.Euler(
                objectPositionRotation.rotation + Vector3.up * previewObjectYRotation
            );

        }

    }

    void UpdatePreviewObjectMaterial()
    {

        if (previewObjectComponent.IsCollidingWithOtherObjects())
        {
            previewObjectComponent.renderer.material = buildingForbiddenMaterial;
        }
        else
        {
            previewObjectComponent.renderer.material = buildingAllowedMaterial;
        }

    }

    void ReleasePreviewObject()
    {

        previewObject.layer = defaultLayer;
        previewObjectComponent.ReleasePreviewObject();
        ResetPreviewObjectInfo();

        BuildingsPanel.instance.PayForObject();

    }

    void ResetPreviewObjectInfo()
    {

        previewObject = null;
        previewObjectComponent = null;
        previewObjectYRotation = 0f;

    }

    PositionRotation MouseRayPositionRotation()
    {
        
        Ray mouseRay = camera.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(mouseRay, out hit, rayLenght, groundLayer.value))
        {

            return new PositionRotation(
                hit.point,
                Quaternion.FromToRotation(Vector3.up, hit.normal).eulerAngles
            );

        }
        else
        {

            return null;

        }

    }

}

public class PositionRotation
{

    public Vector3 position;
    public Vector3 rotation;

    public PositionRotation(Vector3 position, Vector3 rotation)
    {

        this.position = position;
        this.rotation = rotation;

    }

}
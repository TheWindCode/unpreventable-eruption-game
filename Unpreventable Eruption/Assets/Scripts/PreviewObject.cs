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

using System.Collections.Generic;

public class PreviewObject : MonoBehaviour
{

    [HideInInspector] public new Renderer renderer;
    [HideInInspector] public new Rigidbody rigidbody;

    Material originalMaterial;

    List<Collider> structureCollidersCollidingWithThis = new List<Collider>();

    
    public void Initialise()
    {

        renderer = GetComponent<Renderer>();
        rigidbody = GetComponent<Rigidbody>();

        originalMaterial = renderer.material;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

    }

    public void ReleasePreviewObject()
    {

        renderer.material = originalMaterial;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
        
        Destroy(rigidbody);
        Destroy(this);

    }
    
    public bool IsCollidingWithOtherObjects()
    {

        return (structureCollidersCollidingWithThis.Count != 0);

    }
    
    void OnCollisionEnter(Collision collision)
    {
        
        if (collision.collider.CompareTag("Structure"))
            structureCollidersCollidingWithThis.Add(collision.collider);

    }

    void OnCollisionExit(Collision collision)
    {
        
        if (collision.collider.CompareTag("Structure"))
            structureCollidersCollidingWithThis.Remove(collision.collider);

    }

}
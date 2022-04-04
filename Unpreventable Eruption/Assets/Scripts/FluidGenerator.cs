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

public class FluidGenerator : MonoBehaviour
{

    [SerializeField] int preallocatedParticles = 100;
    [SerializeField] float paricleSpawnsPerSecond = 1;
    [SerializeField] GameObject particlePrefab;

    [SerializeField] float spawnSphereDiameter = 1f;
    [SerializeField] float forceRandomisationScalar = 1f;
    [SerializeField] TimeForce[] initialForces;

    Vector3 initialForce;
    float nextTimeToChangeForce;
    int currentForceIndex;
    
    Queue<GameObject> particlesPool = new Queue<GameObject>();

    float nextTimeToSpawn = 0f;


    void Awake()
    {

        currentForceIndex = 0;

        UpdateInitialForce();

        for (int i = 0; i < preallocatedParticles; i++)
        {
            
            GameObject newParticle = Instantiate(particlePrefab);
            newParticle.transform.SetParent(transform);

            ReleaseParticle(newParticle);

        }

    }

    void FixedUpdate()
    {

        if (Time.time >= nextTimeToChangeForce)
            UpdateInitialForce();

        if (Time.time >= nextTimeToSpawn)
        {

            SpawnParticle();

            nextTimeToSpawn = Time.time + 1f/paricleSpawnsPerSecond;

        }

    }

    public void ReleaseParticle(GameObject particle)
    {

        particlesPool.Enqueue(particle);
        particle.GetComponent<Rigidbody>().position = transform.position;
        particle.transform.position = transform.position + Random.insideUnitSphere * spawnSphereDiameter;
        particle.SetActive(false);

        particle.transform.SetParent(null);
        particle.transform.SetParent(transform);

    }

    GameObject SpawnParticle()
    {

        GameObject newParticle;
        
        if (particlesPool.Count != 0)
        {

            newParticle = particlesPool.Dequeue();
            newParticle.SetActive(true);
            newParticle.GetComponent<Rigidbody>().AddForce(
                initialForce + Random.insideUnitSphere * forceRandomisationScalar, ForceMode.Impulse
            );

        }
        else
        {

            newParticle = null;

        }

        return newParticle;

    }

    void UpdateInitialForce()
    {

        initialForce = initialForces[currentForceIndex].force;

        if (initialForces.Length-1 > currentForceIndex)
        {

            currentForceIndex++;
            nextTimeToChangeForce = Time.time + initialForces[currentForceIndex].waitSeconds;

        }
        else
        {

            nextTimeToChangeForce = float.MaxValue;

        }
        
    }

}

[System.Serializable]
public class TimeForce
{

    public float waitSeconds;
    public Vector3 force;

}
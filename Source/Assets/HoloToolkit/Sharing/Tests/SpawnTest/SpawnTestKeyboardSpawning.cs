//
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
//

using HoloToolkit.Sharing.SyncModel;
using HoloToolkit.Sharing.Spawning;
using UnityEngine;
using UnityEngine.VR.WSA.Input;

/// <summary>
/// Class that handles spawning objects on keyboard presses, for the spawning test scene of
/// the HoloToolkit's Sharing component.
/// </summary>
public class SpawnTestKeyboardSpawning : MonoBehaviour
{
    public GameObject SpawnParent;
    public GameObject Prefab;

    public PrefabSpawnManager SpawnManager;

    private int counter = 0;


    private void onStart()
    {
        Vector3 position = new Vector3(1, 1, 1);
        Quaternion rotation = new Quaternion(0, 0, 0, 0);

        SyncSpawnedObject spawnedObject = new SyncSpawnedObject();

        SpawnManager.Spawn(spawnedObject, position, rotation, SpawnParent, "SpawnedObject", false);
    }

    private void Update()
    {
        /*if (counter % 100 == 0)
        {
            Vector3 position = Random.onUnitSphere * 2;
            Quaternion rotation = Random.rotation;

            SyncSpawnedObject spawnedObject = new SyncSpawnedObject();

            SpawnManager.Spawn(spawnedObject, position, rotation, SpawnParent, "SpawnedObject", false);
        }

        counter++;*/
    }
}

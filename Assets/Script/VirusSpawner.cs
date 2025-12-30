
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class VirusSpawner : MonoBehaviour
// {
//     [Header("Virus Settings")]
//     public GameObject virusPrefab;   // Virus prefab
//     public int maxVirus = 10;        // Total viruses to spawn
//     public float spawnRange = 5f;    // Range for X and Z coordinates
//     public float minDistance = 2f;   // Minimum distance between viruses
//     public float spawnHeight = 1f;   // Y coordinate (height) of virus

//     void Start()
//     {
//         SpawnInitialViruses();
//     }

//     void SpawnInitialViruses()
//     {
//         List<Vector3> spawnedPositions = new List<Vector3>();

//         int spawned = 0;
//         int attempts = 0;
//         int maxAttempts = 1000; // Safety limit to avoid infinite loops

//         while (spawned < maxVirus && attempts < maxAttempts)
//         {
//             attempts++;

//             // Generate a random position within the range
//             Vector3 pos = new Vector3(
//                 Random.Range(-spawnRange, spawnRange),
//                 spawnHeight,
//                 Random.Range(-spawnRange, spawnRange)
//             );

//             // Check if this position is far enough from existing viruses
//             bool tooClose = false;
//             foreach (Vector3 otherPos in spawnedPositions)
//             {
//                 if (Vector3.Distance(pos, otherPos) < minDistance)
//                 {
//                     tooClose = true;
//                     break;
//                 }
//             }

//             // If not too close, spawn the virus
//             if (!tooClose)
//             {
//                 Instantiate(virusPrefab, pos, Quaternion.identity);
//                 spawnedPositions.Add(pos);
//                 spawned++;
//             }
//         }

//         if (spawned < maxVirus)
//         {
//             Debug.LogWarning("Could not spawn all viruses due to spacing constraints.");
//         }
//     }
// }
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class VirusSpawner : MonoBehaviour
{
    [Header("Virus Settings")]
    public GameObject virusPrefab;
    public int maxVirus = 10;
    public float spawnRange = 10f;      // How far from spawner to spawn
    public float minDistance = 2f;      // Minimum distance between viruses
    public float minHeight = 1f;        // Min Y position
    public float maxHeight = 3f;        // Max Y position

    private List<GameObject> spawnedViruses = new List<GameObject>();

    void Start()
    {
        if (!Application.isPlaying)
        {
            SpawnViruses();
        }
    }

    public void SpawnViruses()
    {
        // Clear previously spawned viruses in editor
        foreach (GameObject v in spawnedViruses)
        {
            if (v != null)
                DestroyImmediate(v);
        }
        spawnedViruses.Clear();

        List<Vector3> positions = new List<Vector3>();
        int spawned = 0;
        int attempts = 0;
        int maxAttempts = 1000;

        while (spawned < maxVirus && attempts < maxAttempts)
        {
            attempts++;

            // Random position around the spawner
            Vector3 pos = new Vector3(
                Random.Range(-spawnRange, spawnRange),
                Random.Range(minHeight, maxHeight),
                Random.Range(-spawnRange, spawnRange)
            ) + transform.position;

            // Check spacing
            bool tooClose = false;
            foreach (Vector3 otherPos in positions)
            {
                if (Vector3.Distance(pos, otherPos) < minDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose)
            {
#if UNITY_EDITOR
                GameObject virus = (GameObject)PrefabUtility.InstantiatePrefab(virusPrefab, this.transform);
                virus.transform.position = pos;
#else
                GameObject virus = Instantiate(virusPrefab, pos, Quaternion.identity);
#endif
                spawnedViruses.Add(virus);
                positions.Add(pos);
                spawned++;
            }
        }

        if (spawned < maxVirus)
        {
            //Debug.LogWarning("Could not spawn all viruses due to spacing constraints.");
        }
    }
}

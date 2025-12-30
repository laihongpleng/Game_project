// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI; 
// using TMPro;

// public class PlayerShoot : MonoBehaviour
// {
//     [Header("Player & Camera")]
//     public Camera cam;
//     public KeyCode shootKey = KeyCode.Space;
//     public float shootRange = 100f;

//     [Header("Aim Dot")]
//     public Image aimDot;           // Drag your AimDot UI Image here
//     public Color normalColor = Color.white;
//     public Color hoverColor = Color.red;

//     [Header("Score")]
//     public Score scoreManager;     // Drag your Score script here

//     void Update()
//     {
//         // Step 1: Detect closest virus
//         GameObject[] viruses = GameObject.FindGameObjectsWithTag("Virus");
//         Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
//         GameObject closestVirus = null;
//         float minDistance = Mathf.Infinity;

//         foreach (GameObject virus in viruses)
//         {
//             Vector3 screenPos = cam.WorldToScreenPoint(virus.transform.position);

//             // Only consider viruses in front of camera
//             if (screenPos.z > 0)
//             {
//                 float distance = Vector2.Distance(new Vector2(screenPos.x, screenPos.y), new Vector2(screenCenter.x, screenCenter.y));
//                 if (distance < minDistance)
//                 {
//                     minDistance = distance;
//                     closestVirus = virus;
//                 }
//             }
//         }

//         // Step 2: Move aim dot
//         if (closestVirus != null)
//         {
//             Vector3 virusScreenPos = cam.WorldToScreenPoint(closestVirus.transform.position);
//             aimDot.transform.position = virusScreenPos;
//             aimDot.color = hoverColor;
//         }
//         else
//         {
//             aimDot.transform.position = screenCenter;
//             aimDot.color = normalColor;
//         }

//         // Step 3: Shoot when key pressed
//         if (Input.GetKeyDown(shootKey) && closestVirus != null)
//         {
//             Virus virusScript = closestVirus.GetComponent<Virus>();
//             if (virusScript != null)
//             {
//                 virusScript.OnHit();

//                 // Increase score
//                 Score.score += 1;
//                 Debug.Log("Virus hit! Score: " + Score.score);

//                 // Win condition
//                 if (GameObject.FindGameObjectsWithTag("Virus").Length == 0)
//                 {
//                     Debug.Log("You Win!");
//                 }
//             }
//         }
//     }
// }
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerShoot : MonoBehaviour
{
    [Header("Player & Camera")]
    public Camera cam;
    public KeyCode shootKey = KeyCode.Space;
    public float shootRange = 100f;   // (not used yet, kept for future raycast)

    [Header("Detection")]
    public float detectDistance = 10f;   // Player must be near virus

    [Header("Aim Dot")]
    public Image aimDot;           
    public Color normalColor = Color.white;
    public Color hoverColor = Color.red;

    [Header("Score")]
    public Score scoreManager;     

    void Update()
    {
        // Screen center
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);

        // Find viruses
        GameObject[] viruses = GameObject.FindGameObjectsWithTag("Virus");

        GameObject closestVirus = null;
        float minScreenDistance = Mathf.Infinity;

        // 🔍 Detect nearest virus (ONLY if close enough)
        foreach (GameObject virus in viruses)
        {
            // World distance check (player → virus)
            float worldDistance = Vector3.Distance(cam.transform.position, virus.transform.position);

            if (worldDistance > detectDistance)
                continue;

            // Convert to screen space
            Vector3 screenPos = cam.WorldToScreenPoint(virus.transform.position);

            // Must be in front of camera
            if (screenPos.z <= 0)
                continue;

            float screenDistance = Vector2.Distance(
                new Vector2(screenPos.x, screenPos.y),
                new Vector2(screenCenter.x, screenCenter.y)
            );

            if (screenDistance < minScreenDistance)
            {
                minScreenDistance = screenDistance;
                closestVirus = virus;
            }
        }

        // 🎯 Aim Dot behavior
        if (closestVirus != null)
        {
            Vector3 virusScreenPos = cam.WorldToScreenPoint(closestVirus.transform.position);
            aimDot.transform.position = virusScreenPos;
            aimDot.color = hoverColor;
        }
        else
        {
            aimDot.transform.position = screenCenter;
            aimDot.color = normalColor;
        }

        // 🔫 Shoot
        if (Input.GetKeyDown(shootKey) && closestVirus != null)
        {
            Virus virusScript = closestVirus.GetComponent<Virus>();
            if (virusScript != null)
            {
                virusScript.OnHit();

                // Increase score
                Score.score += 1;
                Debug.Log("Virus hit! Score: " + Score.score);

                // 🏆 Win condition
                if (GameObject.FindGameObjectsWithTag("Virus").Length == 0)
                {
                    Debug.Log("YOU WIN!");
                }
            }
        }
    }
}


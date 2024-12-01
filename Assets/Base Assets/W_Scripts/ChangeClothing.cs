using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeClothing : MonoBehaviour
{
    // Array to store the different clothing meshes
    public Mesh[] clothingMeshes;

    // Reference to the SkinnedMeshRenderer component
    private SkinnedMeshRenderer skinnedMeshRenderer;

    // Current clothing index
    private int currentClothingIndex = 0;

    void Start()
    {
        // Get the SkinnedMeshRenderer component attached to the player
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

        // Check if clothing meshes are assigned
        if (clothingMeshes.Length > 0)
        {
            // Set the initial clothing
            skinnedMeshRenderer.sharedMesh = clothingMeshes[currentClothingIndex];
        }
        else
        {
            Debug.LogError("No clothing meshes assigned.");
        }
    }

    void Update()
    {
        // Check for input to change clothing
        if (Input.GetKeyDown(KeyCode.C))
        {
            ChangeClothingMesh();
        }
    }

    void ChangeClothingMesh()
    {
        // Increment the clothing index
        currentClothingIndex++;

        // Loop back to the first clothing mesh if index exceeds the array length
        if (currentClothingIndex >= clothingMeshes.Length)
        {
            currentClothingIndex = 0;
        }

        // Change the mesh to the new clothing
        skinnedMeshRenderer.sharedMesh = clothingMeshes[currentClothingIndex];
    }
}

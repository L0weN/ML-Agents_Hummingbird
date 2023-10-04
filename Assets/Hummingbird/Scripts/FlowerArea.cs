using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerArea : MonoBehaviour
{
    // The diameter of the are where the agent and flowers can be
    // used for observing relative distance from agent to flower
    public const float AreaDiameter = 20f;

    // The list of all flowers in this flower area (flower plants have multiple flowers)
    private List<GameObject> flowerPlants;

    // A lookup dictionary for looking up a flower from a nectar collider
    private Dictionary<Collider, Flower> nectarFlowerDictionary;

    /// <summary>
    /// The list of all flowers in this flower area
    /// </summary>
    public List<Flower> Flowers { get; private set; }

    /// <summary>
    /// Reset the flowers and flower plants
    /// </summary>
    public void ResetFlowers()
    {
        //Rotate each flower plant around the Y axis and subtly around X and Z
        foreach (GameObject flowerPlant in flowerPlants)
        {
            float xRotation = UnityEngine.Random.Range(-5f, 5f);
            float yRotation = UnityEngine.Random.Range(-5f, 5f);
            float zRotation = UnityEngine.Random.Range(-5f, 5f);
            flowerPlant.transform.rotation = Quaternion.Euler(xRotation, yRotation, zRotation);
        }

        // Reset each flower
        foreach (Flower flower in Flowers)
        {
            flower.ResetFlower();
        }
    }
    /// <summary>
    /// Gets the <see cref="Flower"/> that a nectar collider belongs to"/>
    /// </summary>
    /// <param name="nectarCollider">The nectar collider</param>
    /// <returns>The matching flower</returns>
    public Flower GetFlowerFromNectar(Collider nectarCollider)
    {
        return nectarFlowerDictionary[nectarCollider];
    }

    /// <summary>
    /// Called when the area wakes up
    /// </summary>
    private void Awake()
    {
        // Initialize variables
        flowerPlants = new List<GameObject>();
        nectarFlowerDictionary = new Dictionary<Collider, Flower>();
        Flowers = new List<Flower>();
    }

    /// <summary>
    /// Called when the game starts
    /// </summary>
    private void Start()
    {
        // Find all flowers that are children of this GameObject/Transform
        FindChildFlowers(transform);
    }

    /// <summary>
    /// Recursively finds all flowers that are children of a parent transform
    /// </summary>
    /// <param name="parent">The parent of the children to check</param>
    private void FindChildFlowers(Transform parent)
    {
        for (int i = 0; i< parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);

            if(child.CompareTag("flower_plant"))
            {
                // found a flower plant, add all children flowers to the list
                flowerPlants.Add(child.gameObject);

                // Look for flowers within the flower plant
                FindChildFlowers(child);
            }
            else
            {
                // Not a flower plant, look for a flower component
                Flower flower = child.GetComponent<Flower>();
                if (flower != null)
                {
                    // Found a flower, add it to the list and dictionary
                    Flowers.Add(flower);

                    // Add to the nectar collider dictionary to the lookup dictionary
                    nectarFlowerDictionary.Add(flower.nectarCollider, flower);
                }
                else
                {
                    // Flower component not found, so check children
                    FindChildFlowers(child);
                }
            }
        }
    }
}

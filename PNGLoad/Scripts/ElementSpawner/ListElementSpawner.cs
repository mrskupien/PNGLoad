using System;
using System.Collections.Generic;
using UnityEngine;

public class ListElementSpawner : MonoBehaviour
{
    public static Action<int> OnElementsInitialized;

    [SerializeField] private RectTransform scrollContent;
    [SerializeField] private GameObject listElementPrefab;

    private List<ListElement> spawnedListElements = new List<ListElement>();

    private bool HasAllRefs => scrollContent != null && listElementPrefab != null;

    private void Awake()
    {
        if(!HasAllRefs)
        {
            Debug.LogError($"{this} has wrong references!");
            return;
        }

        DirectoryFinder.OnFilesFound += SpawnListElements;
    }
    private void OnDestroy()
    {
        DirectoryFinder.OnFilesFound -= SpawnListElements;
    }


    private void SpawnListElements(List<ElementInfo> elementsToSpawn)
    {
        UpdateElementsToSpawn(ref elementsToSpawn);

        if(ImageLoader.IsRequestingActive)
            ImageLoader.Reset();

        int elementsNumber = 0;
        foreach(ElementInfo elementInfo in elementsToSpawn)
        {
            ListElement element = Instantiate(listElementPrefab, scrollContent).GetComponent<ListElement>();
            spawnedListElements.Add(element);
            element.Initialize(elementInfo);
            elementsNumber++;
        }
        OnElementsInitialized?.Invoke(elementsNumber);
    }

    private void UpdateElementsToSpawn(ref List<ElementInfo> elementsToSpawn)
    {
        List<ElementInfo> properElementsToSpawn = new List<ElementInfo>();
        List<ListElement> elementsToDestroy = new List<ListElement>(spawnedListElements);

        foreach(var newElementInfo in elementsToSpawn)
        {
            bool isNewElementExisting = false;

            foreach(var spawnedElement in spawnedListElements)
            {
                if(spawnedElement.ElementInfo == newElementInfo)
                {
                    //new element is existing, skip spawning/destroying old one
                    isNewElementExisting = true;
                    elementsToDestroy.Remove(spawnedElement);
                    break;
                }
            }

            if(!isNewElementExisting)
                properElementsToSpawn.Add(newElementInfo);
        }

        elementsToSpawn = properElementsToSpawn;

        //destroy elements that are no longer existing
        foreach(var element in elementsToDestroy)
        {
            spawnedListElements.Remove(element);
            Destroy(element.gameObject);
        }
    }

}

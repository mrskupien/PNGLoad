using System;
using System.Collections.Generic;
using UnityEngine;

public class ListElementSpawner : MonoBehaviour
{
    public static Action<int> OnElementsInitialized;

    [SerializeField] private RectTransform scrollContent;
    [SerializeField] private GameObject listElementPrefab;
    private ImageLoader imageLoader;

    private bool HasAllRefs => scrollContent != null && listElementPrefab != null;

    private void Awake()
    {
        if(!HasAllRefs)
        {
            Debug.LogError($"{this} has wrong references!");
            return;
        }

        imageLoader = new ImageLoader();
        DirectoryFinder.OnFilesFound += SpawnListElements;
    }
    private void OnDestroy()
    {
        DirectoryFinder.OnFilesFound -= SpawnListElements;
    }


    private void SpawnListElements(List<ElementInfo> elementsToSpawn)
    {
        ClearList();
        if(imageLoader.IsRequestingActive)
            imageLoader.Reset();
        int id = 0;
        foreach(ElementInfo elementInfo in elementsToSpawn)
        {
            ListElement element = Instantiate(listElementPrefab, scrollContent).GetComponent<ListElement>();
            element.Initialize(elementInfo, id, imageLoader);
            id++;
        }
        OnElementsInitialized?.Invoke(id);
    }

    private void ClearList()
    {
        foreach(Transform element in scrollContent)
        {
            Destroy(element.gameObject);
        }
    }

}

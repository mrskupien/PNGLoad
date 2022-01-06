using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    [SerializeField] private Image image;
    private Material material;
    private int elementsToLoad;
    private int elementsLoaded;

    private float Percentage => elementsToLoad == 0 ? 1 : elementsLoaded / (float)elementsToLoad;

    void Awake()
    {
        material = new Material(image.material);
        image.material = material;

        material.SetFloat("_Percentage", 0);

        ListElementSpawner.OnElementsInitialized += Initialize;
        ListElement.OnSuccessfulyLoaded += UpdateProgress;
    }
    private void OnDestroy()
    {
        ListElementSpawner.OnElementsInitialized -= Initialize;
        ListElement.OnSuccessfulyLoaded -= UpdateProgress;
    }
    public void Initialize(int elements)
    {
        elementsToLoad = elements;
        elementsLoaded = 0;
    }
    public void UpdateProgress()
    {
        elementsLoaded++;
        material.SetFloat("_Percentage", Percentage);
    }

}

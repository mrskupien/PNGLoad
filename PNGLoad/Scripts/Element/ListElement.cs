using System;
using UnityEngine;
using UnityEngine.UI;

public class ListElement : MonoBehaviour
{
    public static Action OnSuccessfulyLoaded;

    [SerializeField] private RawImage image;
    [SerializeField] private TMPro.TextMeshProUGUI fileName;
    [SerializeField] private TMPro.TextMeshProUGUI fileBirthTime;

    private int requestID;

    public ElementInfo ElementInfo { get; private set; }    
    private bool HasAllRefs => image != null && fileName != null && fileBirthTime != null;


    public void Initialize(ElementInfo info)
    {
        if(!HasAllRefs)
        {
            Debug.LogError($"{this} has wrong references!");
            return;
        }

        ElementInfo = info;

        UpdateName(info.name);
        UpdateSpan(info.timeSinceBirth);
        UpdateImage(info.imagePath);
    }

    private void UpdateName(string name) => fileName.text = name;
    private void UpdateSpan(string name) => fileBirthTime.text = name;
    private void UpdateImage(string path)
    {
        ImageLoader.RequestImage(path, out requestID);
        ImageLoader.OnLoadedImage += LoadImage;
    }
    private void OnDestroy()
    {
        //destroy loaded texture to prevent memory leak
        Destroy(image.texture);
        ImageLoader.OnLoadedImage -= LoadImage;
    }
    public void LoadImage(Texture2D texture, int requestID)
    {
        if(requestID != this.requestID)
            return;
        
        image.texture = texture;
        OnSuccessfulyLoaded?.Invoke();
    }

}

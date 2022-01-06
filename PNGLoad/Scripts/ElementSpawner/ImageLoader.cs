using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ImageLoader
{
    /// <summary>
    /// Texture with request ID
    /// </summary>
    public Action<Texture2D, int> OnLoadedImage;

    private Queue<(string path, int id)> imageRequests = new Queue<(string, int)>();
    private CancellationTokenSource cancellation;

    public bool IsRequestingActive { get; private set; }
    private bool ShouldCancel { get; set; }

    public void RequestImage(string path, int id)
    {
        if(ShouldCancel && IsRequestingActive)
            StopActiveRequesting();

        imageRequests.Enqueue((path, id));

        if(IsRequestingActive)
            return;

        StartAsyncRequest();
    }

    public void Reset() => ShouldCancel = true;

    private void StopActiveRequesting()
    {
        cancellation.Dispose();
        imageRequests.Clear();
        IsRequestingActive = false;
        ShouldCancel = false;
    }

    private async void StartAsyncRequest()
    {
        IsRequestingActive = true;
        cancellation = new CancellationTokenSource();

        while(imageRequests.Count > 0)
        {
            try
            {
                (string path, int id) request = imageRequests.Dequeue();
                Texture2D texture = await Utility.Texture.LoadImageAsync(request.path);
                OnLoadedImage?.Invoke(texture, request.id);
            }
            catch(OperationCanceledException)
            {
                Debug.LogWarning($"Image load canceled exception thrown");
            }
        }

        imageRequests.Clear();
        cancellation.Dispose();
        IsRequestingActive = false;
    }


}

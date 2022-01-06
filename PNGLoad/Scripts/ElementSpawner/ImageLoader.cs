using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public static class ImageLoader
{
    /// <summary>
    /// Texture with request ID
    /// </summary>
    public static Action<Texture2D, int> OnLoadedImage;

    private static Queue<(string path, int id)> imageRequests = new Queue<(string, int)>();
    private static CancellationTokenSource cancellation;
    private static int requestID;

    public static bool IsRequestingActive { get; private set; }
    private static bool ShouldCancel { get; set; }

    /// <summary>
    /// Request image with an ID to receive proper image
    /// </summary>
    /// <param name="path">Path to requested image</param>
    /// <param name="id">ID of the request. Save and check on image loaded action</param>
    public static void RequestImage(string path, out int id)
    {
        if(ShouldCancel && IsRequestingActive)
            StopActiveRequesting();

        id = requestID;

        imageRequests.Enqueue((path, requestID++));

        if(IsRequestingActive)
            return;

        StartAsyncRequest();
    }

    public static void Reset() => ShouldCancel = true;

    private static void StopActiveRequesting()
    {
        cancellation.Dispose();
        imageRequests.Clear();
        IsRequestingActive = false;
        ShouldCancel = false;
    }

    private static async void StartAsyncRequest()
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

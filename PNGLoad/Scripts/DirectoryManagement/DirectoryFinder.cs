using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DirectoryFinder : MonoBehaviour
{
    public static Action<ErrorLog> OnErrorLogged;
    public static Action<List<ElementInfo>> OnFilesFound;

    private string directoryPath;
    private readonly string desiredExtension = ".png";

    private void Awake() => DirectorySetter.OnDirectorySet += SaveDirectoryPath;
    private void OnDestroy() => DirectorySetter.OnDirectorySet -= SaveDirectoryPath;
    private void SaveDirectoryPath(string path) => directoryPath = path;

    //button method
    public void CheckDirectoryForFiles()
    {
        if(Directory.Exists(directoryPath))
        {
            CheckForProperFiles();
        }
        else
        {
            OnErrorLogged?.Invoke(ErrorLog.NoDirectory);
        }
    }

    private void CheckForProperFiles()
    {
        IEnumerable<string> numeratedFiles = Directory.EnumerateFiles(directoryPath);
        List<ElementInfo> elements = new List<ElementInfo>();

        foreach(string file in numeratedFiles)
        {
            if(File.Exists(file))
            {
                if(Path.GetExtension(file) != desiredExtension)
                    continue;

                elements.Add(CreateElementInfo(file));
            }
        }

        if(elements.Count > 0)
        {
            OnFilesFound?.Invoke(elements);
            OnErrorLogged?.Invoke(ErrorLog.OK);
        }
        else
        {
            OnErrorLogged?.Invoke(ErrorLog.NoFiles);
        }
    }

    private ElementInfo CreateElementInfo(string file)
    {
        DateTime birthTime = File.GetCreationTime(file);
        TimeSpan timeSpan = DateTime.Now.Subtract(birthTime);
        string fileName = Path.GetFileName(file);

        return new ElementInfo(timeSpan, fileName, file);
    }
}

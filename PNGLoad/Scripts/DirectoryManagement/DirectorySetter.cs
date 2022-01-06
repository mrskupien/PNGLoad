using System;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_InputField))]
public class DirectorySetter : MonoBehaviour
{
    public static Action<string> OnDirectorySet;

    private TMP_InputField inputField;

    private void Awake() => inputField = GetComponent<TMP_InputField>();

    //on input field finish change
    public void SetDirectory() => OnDirectorySet?.Invoke(inputField.text);
}

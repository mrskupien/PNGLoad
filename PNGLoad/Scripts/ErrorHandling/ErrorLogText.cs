using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ErrorLogText : MonoBehaviour
{
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        DirectoryFinder.OnErrorLogged += LogError;
    }
    private void OnDestroy()
    {
        DirectoryFinder.OnErrorLogged -= LogError;
    }

    private void LogError(ErrorLog log) => text.text = log.Message;

}

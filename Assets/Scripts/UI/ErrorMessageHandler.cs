using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SceneMessageHandler : MonoBehaviour
{
    public GameObject backgroundPanel;
    public TMP_Text messageText;

    public void ShowScene(string message)
    {
        messageText.text = message;
        backgroundPanel.SetActive(true);
        CancelInvoke(nameof(HideScene));
        Invoke(nameof(HideScene), 5f);
    }

    private void HideScene()
    {
        backgroundPanel.SetActive(false);
    }
}

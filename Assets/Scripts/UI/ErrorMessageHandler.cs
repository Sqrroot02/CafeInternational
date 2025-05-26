using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SceneMessageHandler : MonoBehaviour
{
    // Referenz auf das UI-Panel, das die Nachricht visuell hinterlegt
    public GameObject backgroundPanel;

    // Textfeld, in dem die Nachricht angezeigt wird
    public TMP_Text messageText;

    /// <summary>
    /// Zeigt eine Szene-Nachricht an und blendet sie nach 5 Sekunden automatisch wieder aus.
    /// </summary>
    /// <param name="message">Die anzuzeigende Nachricht.</param>
    public void ShowScene(string message)
    {
        Debug.Log("[SceneMessageHandler] ShowScene - Showing message: " + message);

        messageText.text = message;
        backgroundPanel.SetActive(true);

        // Vorherige geplante Ausblendung abbrechen (wenn vorhanden)
        CancelInvoke(nameof(HideScene));

        // Nach 5 Sekunden automatisch ausblenden
        Invoke(nameof(HideScene), 5f);
    }

    /// <summary>
    /// Blendet die Nachricht aus, indem das Panel deaktiviert wird.
    /// </summary>
    private void HideScene()
    {
        Debug.Log("[SceneMessageHandler] HideScene - Hiding message panel.");
        backgroundPanel.SetActive(false);
    }
}

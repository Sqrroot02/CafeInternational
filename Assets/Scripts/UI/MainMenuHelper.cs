using TMPro;
using UnityEngine;

public static class MainMenuHelper
{
    public static void SetPlaceholder(TMP_InputField field, string text)
    {
        if (field.placeholder is TextMeshProUGUI placeholder)
            placeholder.text = text;
    }

    public static string GetInputText(TMP_InputField field)
    {
        return field.text;
    }

    public static void ResetInputText(TMP_InputField field)
    {
        field.text = "";
    }
}

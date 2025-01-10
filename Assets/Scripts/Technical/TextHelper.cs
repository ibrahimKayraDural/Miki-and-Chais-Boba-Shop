using TMPro;
using UnityEngine;

public class TextHelper : MonoBehaviour
{
    [SerializeField] TMP_Text textObject;
    
    public void SetText(string newText)
    {
        textObject.text = newText;
    }
}
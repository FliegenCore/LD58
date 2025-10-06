using TMPro;
using UnityEngine;

public class MainCanvasView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    public TMP_Text FilmCountText;
    
    private string _lastText;

    public void SetText(string text)
    {
        if(_lastText != text)
        {
            _text.text = text;
            _lastText = text;
        }
    }
}

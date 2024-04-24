using DG.Tweening;
using TMPro;
using UnityEngine;

public class Info : MonoBehaviour
{
    private const int ShowingTime = 3;

    [SerializeField] private CanvasGroup _canvas;
    [SerializeField] private TMP_Text _label;

    private void Awake()
    {
        _canvas.gameObject.SetActive(false);
        _canvas.alpha = 0;
    }

    public void Show(string text)
    {
        _canvas.gameObject.SetActive(true);
        _label.text = text;
        DOTween.Sequence().Join(_canvas.DOFade(1, ShowingTime)).Join(_canvas.DOFade(0, ShowingTime).OnComplete(OnFinish));
    }

    private void OnFinish()
    {
        _canvas.gameObject.SetActive(false);
        _label.text = "";
    }
}

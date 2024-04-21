using UnityEngine;

public class TouchscreenUIConnector : MonoBehaviour
{
    [SerializeField] private Canvas _touchscreenUI;

    private void Awake()
    {
        _touchscreenUI.enabled = Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
    }
}

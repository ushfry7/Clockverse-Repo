using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartingMenuManager : MonoBehaviour
{
    [SerializeField] CanvasGroup _startMenuPanel;
    [SerializeField] Button _startBtn, _exitBtn;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;
        StartCoroutine(PanelSequence(_startMenuPanel, 1f));
        _startBtn.onClick.AddListener(StartClicked);
        _exitBtn.onClick.AddListener(Exit);
    }

    IEnumerator PanelSequence(CanvasGroup _targetPanel, float _targetValue, float _changeAmount = 0.1f, float _timeInterval = 0.1f)
    {
        while (_targetPanel.alpha < _targetValue)
        {
            _targetPanel.alpha += _changeAmount;
            yield return new WaitForSeconds(_timeInterval);
        }
    }

    void StartClicked()
    {
        SceneManager.LoadScene(1);
    }

    void Exit()
    {
        Application.Quit();
    }
}

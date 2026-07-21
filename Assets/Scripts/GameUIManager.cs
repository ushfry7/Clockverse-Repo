using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] GameObject _waveOne, _waveTwo, _waveThree, _boss;
    [SerializeField] GameObject _settingPanel, _gamePanel, _abilitySelectionPanel;
    [SerializeField] Button _resumeBtn, _restartBtn, _mainMenuBtn, _speedBtn, _slowBtn, _freezeBtn, _speedTestBtn, _slowTestBtn, _freezeTestBtn,
        _chronoBtn, _tempusBtn,_wardenBtn;
    [SerializeField] TMP_Text _abilityTimer;
    [SerializeField] Slider _sensitivitySlider;
    BasePlayerController _player;
    public Image _healthBar;
    float _valueChangeAmount = 0.5f;
    public int _killCount = 0;

    private void Start()
    {
        ComponentAssignment();
    }

    void ComponentAssignment()
    {
        _player = FindFirstObjectByType<BasePlayerController>();
        Time.timeScale = 0f;
        _resumeBtn.onClick.AddListener(Resume);
        _restartBtn.onClick.AddListener(Restart);
        _mainMenuBtn.onClick.AddListener(MainMenu);
        _speedBtn.onClick.AddListener(Speed);
        _slowBtn.onClick.AddListener(Slow);
        _freezeBtn.onClick.AddListener(Freeze);
        _speedTestBtn.onClick.AddListener(SpeedTest);
        _slowTestBtn.onClick.AddListener(SlowTest);
        _freezeTestBtn.onClick.AddListener(FreezeTest);
        _chronoBtn.onClick.AddListener(DoNothing);
        _tempusBtn.onClick.AddListener(DoNothing);
        _wardenBtn.onClick.AddListener(DoNothing);
        _sensitivitySlider.onValueChanged.AddListener(SensitivityChange);
    }

    void DoNothing()
    {

    }

    void Speed()
    {
        _player._abilitySelected = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        StartCoroutine(PanelSequence(_abilitySelectionPanel.GetComponent<CanvasGroup>(), 0, 1, -0.25f));
    }

    void Slow()
    {
        _player._abilitySelected = 2;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        StartCoroutine(PanelSequence(_abilitySelectionPanel.GetComponent<CanvasGroup>(), 0, 1, -0.25f));
    }

    void Freeze()
    {
        _player._abilitySelected = 3;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        StartCoroutine(PanelSequence(_abilitySelectionPanel.GetComponent<CanvasGroup>(), 0, 1, -0.25f));
    }

    void SpeedTest()
    {
        _player._abilitySelected = 1;
        Resume();
    }

    void SlowTest()
    {
        _player._abilitySelected = 2;
        Resume();
    }

    void FreezeTest()
    {
        _player._abilitySelected = 3;
        Resume();
    }

    public void SettingMenu()
    {
        if (!_settingPanel.activeSelf)
        {
            if(_player._currentHealth <= 0)
            {
                _resumeBtn.gameObject.SetActive(false);
            }
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            StartCoroutine(PanelSequence(_settingPanel.GetComponent<CanvasGroup>(), 1f, 0f));
        }
    }

    IEnumerator PanelSequence(CanvasGroup _targetPanel, float _targetValue, float _timeScale, float _changeAmount = 0.25f, float _timeInterval = 0.1f)
    {
        if(_targetValue == 1f)
        {
            _targetPanel.gameObject.SetActive(true);
        }
        else 
        {
            Time.timeScale = _timeScale;
        }
        while (_targetPanel.alpha != _targetValue)
        {
            _targetPanel.alpha += _changeAmount;
            yield return new WaitForSeconds(_timeInterval);
        }
        if (_targetValue == 0)
        {
            _targetPanel.gameObject.SetActive(false);
        }
        Time.timeScale = _timeScale;
    }

    public void HealthUpdate(float _value)
    {
        if (_value != _healthBar.fillAmount)
        {
            _healthBar.fillAmount = Mathf.MoveTowards(_healthBar.fillAmount, _value, _valueChangeAmount * Time.deltaTime);
        }
    }

    void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StartCoroutine(PanelSequence(_settingPanel.GetComponent<CanvasGroup>(), 0f, 1f, -0.1f));
    }

    void Restart()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(1);
    }

    void MainMenu()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(0);
    }

    void SensitivityChange(float value)
    {
        FindFirstObjectByType<ThirdPersonCmControl>()._sensitivity = value;
        PlayerPrefs.SetFloat("Sensitivity", value);
    }

    public float CameraSensitivity()
    {
        if (!PlayerPrefs.HasKey("Sensitivity"))
        {
            PlayerPrefs.SetFloat("Sensitivity", 300f);
        }
        _sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity");
        return _sensitivitySlider.value;
    }

    public void WaveSpawn()
    {
        _killCount++;
        if(_killCount < 2)
        {
            _waveOne.SetActive(true);
        }
        else if(_killCount == 2)
        {
            _waveTwo.SetActive(true);
        }
        else if(_killCount == 5)
        {
            _waveThree.SetActive(true);
        }
        else if(_killCount == 10)
        {
            _boss.SetActive(true);
        }
        else if(_killCount > 10)
        {
            SettingMenu();
        }
    }

    public void TimerUpdate()
    {
        if (_player._currentCoolDownTimer <= 0)
        {
            _abilityTimer.text = "Space";
        }
        else
        {
            _abilityTimer.text = _player._currentCoolDownTimer.ToString() + "s";
        }
    }
}

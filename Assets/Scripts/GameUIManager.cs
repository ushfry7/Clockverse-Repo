using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] GameObject _waveOne, _waveTwo, _waveThree, _boss;
    [SerializeField] GameObject _gamePanel, _abilitySelectionPanel;
    [SerializeField] Button _startBtn, _restartBtn, _mainMenuBtn, _haltBtn, _pathBtn, _ruleBtn, _starBtn, _zionBtn, _goldBtn, _rushBtn, _diveBtn, _massBtn, _omitBtn, _dirtBtn, _discBtn,
        _modZeroBtn, _modOneBtn, _modTwoBtn, _modThreeBtn, _modFourBtn,
                _chronoBtn, _tempusBtn, _wardenBtn;
    [SerializeField] TMP_Text _abilityTimerTxt, _rechargeTxt, _chargesTxt,
        _modOneTxt, _modTwoTxt, _modThreeTxt, _modFourTxt;
    [SerializeField] Slider _sensitivitySlider;
    public BasePlayerController _player;
    public TempoSystem _tempoSystem;
    public Image _healthBar;
    float _valueChangeAmount = 0.5f;
    public int _killCount = 0;
    [SerializeField] TempoSystem _haltTempo, _pathTempo, _ruleTempo, _starTempo, _zionTempo, _goldTempo, _rushTempo, _diveTempo, _massTempo, _omitTempo, _dirtTempo, _discTempo;
    private void Start()
    {
        ComponentAssignment();
    }

    void ComponentAssignment()
    {
        _player = FindFirstObjectByType<BasePlayerController>();       
        Time.timeScale = 0f;
        _startBtn.onClick.AddListener(StartGame);
        _restartBtn.onClick.AddListener(Restart);
        _mainMenuBtn.onClick.AddListener(MainMenu);
        _haltBtn.onClick.AddListener(HaltTempo);
        _pathBtn.onClick.AddListener(PathTempo);
        _ruleBtn.onClick.AddListener(RuleTempo);
        _starBtn.onClick.AddListener(StarTempo);
        _zionBtn.onClick.AddListener(ZionTempo);
        _goldBtn.onClick.AddListener(GoldTempo);
        _rushBtn.onClick.AddListener(RushTempo);
        _diveBtn.onClick.AddListener(DiveTempo);
        _massBtn.onClick.AddListener(MassTempo);
        _omitBtn.onClick.AddListener(OmitTempo);
        _dirtBtn.onClick.AddListener(DirtTempo);
        _discBtn.onClick.AddListener(DiscTempo);
        _modZeroBtn.onClick.AddListener(ModZeroSelected);
        _modOneBtn.onClick.AddListener(ModOneSelected);
        _modTwoBtn.onClick.AddListener(ModTwoSelected);
        _modThreeBtn.onClick.AddListener(ModThreeSelected);
        _modFourBtn.onClick.AddListener(ModFourSelected);
        _chronoBtn.onClick.AddListener(DoNothing);
        _tempusBtn.onClick.AddListener(DoNothing);
        _wardenBtn.onClick.AddListener(DoNothing);
        _sensitivitySlider.onValueChanged.AddListener(SensitivityChange);
        TempoReset();
    }

    void DoNothing()
    {

    }

    void TempoReset()
    {
        _haltTempo.enabled = false;
        _pathTempo.enabled = false;
        _ruleTempo.enabled = false;
        _starTempo.enabled = false;
        _zionTempo.enabled = false;
        _goldTempo.enabled = false;
        _rushTempo.enabled = false;
        _diveTempo.enabled = false;
        _massTempo.enabled = false;
        _dirtTempo.enabled = false;
        _discTempo.enabled = false;
        _modOneTxt.text = "Mod1";
        _modTwoTxt.text = "Mod2";
        _modThreeTxt.text = "Mod3";
        _modFourTxt.text = "Mod4";
    }

    void HaltTempo()
    {
        TempoReset();
        _haltTempo.enabled = true;
        _tempoSystem = _haltTempo;
        _modOneTxt.text = "Ire";
        _modTwoTxt.text = "Joy";
        _modThreeTxt.text = "Cry";
        _modFourTxt.text = "Vex";
    }

    void PathTempo()
    {
        TempoReset();
        _pathTempo.enabled = true;
        _tempoSystem = _pathTempo;
        _modOneTxt.text = "Vex";
        _modTwoTxt.text = "Cry";
        _modThreeTxt.text = "Ant";
        _modFourTxt.text = "Gap";
    }

    void RuleTempo()
    {
        TempoReset();
        _ruleTempo.enabled = true;
        _tempoSystem = _ruleTempo;
        _modOneTxt.text = "Ire";
        _modTwoTxt.text = "Ant";
        _modThreeTxt.text = "Vex";
        _modFourTxt.text = "Ice";
    }

    void StarTempo()
    {
        TempoReset();
        _starTempo.enabled = true;
        _tempoSystem = _starTempo;
        _modOneTxt.text = "Rue";
        _modTwoTxt.text = "Foe";
        _modThreeTxt.text = "Ant";
        _modFourTxt.text = "Pay";
    }

    void ZionTempo()
    {
        TempoReset();
        _zionTempo.enabled = true;
        _tempoSystem = _zionTempo;
        _modOneTxt.text = "Ant";
        _modTwoTxt.text = "Cry";
        _modThreeTxt.text = "Gem";
        _modFourTxt.text = "But";
    }

    void GoldTempo()
    {
        TempoReset();
        _goldTempo.enabled = true;
        _tempoSystem = _goldTempo;
        _modOneTxt.text = "Rue";
        _modTwoTxt.text = "Raw";
        _modThreeTxt.text = "But";
        _modFourTxt.text = "Zen";
    }

    void RushTempo()
    {
        TempoReset();
        _rushTempo.enabled = true;
        _tempoSystem = _rushTempo;
        _modOneTxt.text = "Gem";
        _modTwoTxt.text = "Ant";
        _modThreeTxt.text = "Rue";
        _modFourTxt.text = "Yen";
    }

    void DiveTempo()
    {
        TempoReset();
        _diveTempo.enabled = true;
        _tempoSystem = _diveTempo;
        _modOneTxt.text = "Hex";
        _modTwoTxt.text = "Ice";
        _modThreeTxt.text = "Raw";
        _modFourTxt.text = "Yen";
    }

    void MassTempo()
    {
        TempoReset();
        _massTempo.enabled = true;
        _tempoSystem = _massTempo;
        _modOneTxt.text = "Den";
        _modTwoTxt.text = "Raw";
        _modThreeTxt.text = "Cry";
        _modFourTxt.text = "Ice";
    }

    void OmitTempo()
    {
        TempoReset();
        _omitTempo.enabled = true;
        _tempoSystem = _omitTempo;
        _modOneTxt.text = "Joy";
        _modTwoTxt.text = "Hex";
        _modThreeTxt.text = "Mix";
        _modFourTxt.text = "Sin";
    }

    void DirtTempo()
    {
        TempoReset();
        _dirtTempo.enabled = true;
        _tempoSystem = _dirtTempo;
        _modOneTxt.text = "Ire";
        _modTwoTxt.text = "Vex";
        _modThreeTxt.text = "Ant";
        _modFourTxt.text = "Raw";
    }

    void DiscTempo()
    {
        TempoReset();
        _discTempo.enabled = true;
        _tempoSystem = _discTempo;
        _modOneTxt.text = "Cry";
        _modTwoTxt.text = "Beg";
        _modThreeTxt.text = "Vex";
        _modFourTxt.text = "Foe";
    }

    void ModZeroSelected()
    {
        _tempoSystem._epochSelected = 0;
    }

    void ModOneSelected()
    {
        _tempoSystem._epochSelected = 1;
    }

    void ModTwoSelected()
    {
        _tempoSystem._epochSelected = 2;
    }

    void ModThreeSelected()
    {
        _tempoSystem._epochSelected = 3;
    }

    void ModFourSelected()
    {
        _tempoSystem._epochSelected = 4;
    }

    public void SettingMenu()
    {
        if (!_abilitySelectionPanel.activeSelf)
        {
            StartCoroutine(PanelSequence(_abilitySelectionPanel.GetComponent<CanvasGroup>(), 1f, 0f));
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if (_player._currentHealth <= 0)
            {
                _startBtn.gameObject.SetActive(false);
                foreach (BasePlayerController player in FindObjectsByType<BasePlayerController>(FindObjectsInactive.Include, FindObjectsSortMode.None))
                {
                    if (player != null)
                    {
                        if (player.gameObject != _player.gameObject)
                        {
                            Destroy(player.gameObject);
                        }
                    }
                }
            }
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

    void StartGame()
    {
        _tempoSystem._charges = _tempoSystem._chargeLimit;
        foreach (BasePlayerController player in FindObjectsByType<BasePlayerController>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (player != null)
            {
                if (player.gameObject != _player.gameObject)
                {
                    Destroy(player.gameObject);
                }
            }
        }
        _player.Normal();
        foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            enemy.Normal();
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        _tempoSystem.StopALL();
        StartCoroutine(PanelSequence(_abilitySelectionPanel.GetComponent<CanvasGroup>(), 0f, 1f, -0.1f));
        _tempoSystem.EpochSelect();
        TimerUpdate();
    }

    void Restart()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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

    bool _stageChange = false;
    public void WaveSpawn()
    {
        _killCount++;
        if(_killCount < 2)
        {
            _waveOne.SetActive(true);
        }
        else if(_killCount == 2)
        {
            _stageChange = true;
            _waveTwo.SetActive(true);
        }
        else if(_killCount == 5)
        {
            _stageChange = true;
            _waveThree.SetActive(true);
        }
        else if(_killCount == 10)
        {
            _stageChange = true;
            _boss.SetActive(true);
        }
        else if(_killCount > 10)
        {
            SettingMenu();
        }
    }

    public bool StageChange()
    {
        if (_stageChange)
        {
            _stageChange = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void TimerUpdate()
    {
        //if (_tempoSystem._currentCoolDownTimer >= _tempoSystem._tempoCooldown - _tempoSystem._tempoEffectDuration && _tempoSystem._currentCoolDownTimer <= _tempoSystem._tempoCooldown + _tempoSystem._tempoEffectDuration)
        //{
        //    _abilityTimerTxt.text = "InEffect";
        //}
        //else
        if (_tempoSystem._currentCoolDownTimer <= 0)
        {
            _abilityTimerTxt.text = "Space";
            _chargesTxt.text = _tempoSystem._charges.ToString();
        }
        else
        {
            _abilityTimerTxt.text = _tempoSystem._currentCoolDownTimer.ToString() + "s";
        }
        _chargesTxt.text = _tempoSystem._charges.ToString();
    }

    public void RechargeUpdate()
    {
        if (_tempoSystem._currentRecharge >= 100)
        {
            _rechargeTxt.text = "Space";
        }
        else
        {
            _rechargeTxt.text = _tempoSystem._currentRecharge.ToString() + "%";
        }
    }
}

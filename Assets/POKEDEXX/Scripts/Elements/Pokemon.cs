using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class Pokemon : MonoBehaviour
{
    public const float PanelHeight = 100;

    public RectTransform RectTransform { get; private set; }

    [Header("Obj refs")]
    [SerializeField] private Text _pokeMaxCpText;
    [SerializeField] private Image _pokeImage;
    [SerializeField] private Text _pokeNameText;
    [SerializeField] private PokeStats _pokeStats;
    [SerializeField] private PokeTypeLabel[] _pokeTypeLabels;
    [SerializeField] private Button _button;

    void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
    }

    public void Set(PokeInfo pokeInfo, UnityAction clickAction)
    {
        _pokeNameText.text =
            pokeInfo.Name +
            UIManager.GetFormattedText("  #" + pokeInfo.Id, TextColorType.GreyLight, false, 12);
        
        _pokeMaxCpText.text =
            TextManager.GetText(TextType.Element, 0) +
            UIManager.NewLine(1) +
            UIManager.GetFormattedText(TextManager.GetNumericFormat((int) pokeInfo.MaxCp), TextColorType.GreenSmooth,
                true, 34);

        _pokeStats.SetStats(pokeInfo.AttackRate, pokeInfo.DefenseRate, pokeInfo.StaminaRate);

        _pokeImage.sprite = pokeInfo.Image;        

        foreach (var pokeTypeLabel in _pokeTypeLabels)
            pokeTypeLabel.gameObject.SetActive(false);

        for (int i = 0; i < pokeInfo.Type.Length; i++)
        {
            if (i < _pokeTypeLabels.Length)
            {
                _pokeTypeLabels[i].SetType(pokeInfo.Type[i]);
                _pokeTypeLabels[i].gameObject.SetActive(true);
            }                
        }

        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(clickAction);
    }

    public void EnableObj()
    {
        gameObject.SetActive(true);
    }

    public void DisableObj()
    {
        gameObject.SetActive(false);
    }
}

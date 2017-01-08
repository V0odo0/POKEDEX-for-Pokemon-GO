using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TypeChartPanel : MonoBehaviour
{
    public const float LabelStartPosY = 30;
    public const float LabelSpaceY = 5;

    public GameObject TypeLabelTemplate;

    [Header("Obj refs")]
    [SerializeField] private RectTransform _strenghtsRect;
    [SerializeField] private RectTransform _weaksRect;

    [SerializeField] private Text _strenghtText;
    [SerializeField] private Text _weaksText;

    private RectTransform _rectTransform;

    private float _labelHeightY;
    private List<GameObject> _curLabelsObj = new List<GameObject>(); 

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();

        _labelHeightY = TypeLabelTemplate.GetComponent<RectTransform>().sizeDelta.y;
    }

    public void Set(TypeChartPanelType panelType, PokeType[] strenghts, PokeType[] weaks)
    {
        for (int i = 0; i < _curLabelsObj.Count; i++)
            Destroy(_curLabelsObj[i]);
        _curLabelsObj = new List<GameObject>();

        //Set rect
        int maxLabels = Mathf.Max(strenghts.Length, weaks.Length);
        _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, LabelStartPosY + ((_labelHeightY + LabelSpaceY) * maxLabels));

        switch (panelType)
        {
            case TypeChartPanelType.Pokemon:
                _strenghtText.text = TextManager.GetText(TextType.Element, 42);
                _weaksText.text = TextManager.GetText(TextType.Element, 43);
                break;
            case TypeChartPanelType.Move:
                _strenghtText.Rebuild(CanvasUpdate.PreRender);
                _strenghtText.text = TextManager.GetText(TextType.Element, 44);
                _weaksText.text = TextManager.GetText(TextType.Element, 45);
                break;
        }

        //Create labels
        for (int i = 0; i < strenghts.Length; i++)
        {
            RectTransform labelRect = Instantiate(TypeLabelTemplate).GetComponent<RectTransform>();
            labelRect.SetParent(_strenghtsRect);
            labelRect.localScale = Vector3.one;
            labelRect.anchoredPosition = new Vector2(0, -LabelStartPosY - (_labelHeightY + 5) * i);

            labelRect.GetComponent<PokeTypeLabel>().SetType(strenghts[i]);

            _curLabelsObj.Add(labelRect.gameObject);
        }

        for (int i = 0; i < weaks.Length; i++)
        {
            RectTransform labelRect = Instantiate(TypeLabelTemplate).GetComponent<RectTransform>();
            labelRect.SetParent(_weaksRect);
            labelRect.localScale = Vector3.one;
            labelRect.anchoredPosition = new Vector2(0, -LabelStartPosY - (_labelHeightY + 5) * i);

            labelRect.GetComponent<PokeTypeLabel>().SetType(weaks[i]);

            _curLabelsObj.Add(labelRect.gameObject);
        }
        
    }
}

[Serializable]
public enum TypeChartPanelType
{
    Pokemon = 0, Move
}

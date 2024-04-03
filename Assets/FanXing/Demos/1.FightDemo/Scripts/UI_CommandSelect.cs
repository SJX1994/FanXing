using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UI_CommandSelect : MonoBehaviour
{
    [SerializeField] RectTransform buttons;
    public Button btn_Command_Move,btn_Command_Fight,btn_Command_Defense;
    void Start()
    {
        btn_Command_Move.onClick.AddListener(() =>
        {
            HideButtons();
        });
    }

    public void Show()
    {
        transform.gameObject.SetActive(true);
        buttons.localScale = new Vector3(1, 0, 1);
        buttons.DOScaleY(1, 0.3f).SetEase(Ease.OutSine);
    }
    public void Hide()
    {
        buttons.DOScaleY(0, 0.3f).SetEase(Ease.InSine).OnComplete(() =>
        {
            transform.gameObject.SetActive(false);
        });
    }
    void HideButtons()
    {
        buttons.DOScaleY(0, 0.3f).SetEase(Ease.InSine);
    }
}

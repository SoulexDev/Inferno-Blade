using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tip : MonoBehaviour
{
    [SerializeField] private float width = 500;
    [SerializeField] private TextMeshProUGUI tipText;
    [SerializeField] private GameObject enabler;
    public Image textContainer;

    private Vector2 textSizeDelta => textContainer.rectTransform.sizeDelta;

    private Vector3 pos;
    private bool active;

    private Camera cam => Camera.main;

    private void Update()
    {
        if (active)
        {
            enabler.SetActive(Vector3.Dot(cam.transform.forward, pos - cam.transform.position) > 0);
        }
        if (gameObject.activeSelf)
        {
            transform.position = cam.WorldToScreenPoint(pos);
            textContainer.rectTransform.sizeDelta = Vector2.Lerp(textSizeDelta, new Vector2(width, textSizeDelta.y), Time.deltaTime * 5);
        }
    }
    public void Init(string info, Vector3 pos)
    {
        active = true;

        this.pos = pos;
        tipText.text = info;

        gameObject.SetActive(true);
        textContainer.rectTransform.sizeDelta = new Vector2(0, tipText.preferredHeight);
    }
    public void DestroyTip()
    {
        active = false;

        gameObject.SetActive(false);
        pos = Vector3.zero;
        tipText.text = "";
    }
}
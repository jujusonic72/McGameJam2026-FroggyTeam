using UnityEngine;
using TMPro;
using UnityEditor.UI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class menuUIButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject indicator;
    public Text textButton;
    public Color textcolor;
    public Color textcolorHover;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        indicator.SetActive(false);
        textButton.color = textcolor;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        indicator.SetActive(true);
        textButton.color = textcolorHover;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        indicator.SetActive(false);
        textButton.color = textcolor;
    }
}

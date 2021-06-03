using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DragUI : MonoBehaviour
{
    public TMP_Text rightText, leftText;
    public Image image;
    public TMP_Text text, deathText, personName;

    [SerializeField]
    private Color neutralStatsMarkerColor, negativeStatsMarkerColor, positiveStatsMarkerColor;

    private Drag drag;

    private void Awake()
    {
        drag = GetComponent<Drag>();
    }

    public void SetUIValues(string rightOptionText, string leftOptionText, Sprite characterSprite, string characterText, string characterName)
    {
        rightText.text = rightOptionText;
        leftText.text = leftOptionText;
        image.sprite = characterSprite;
        text.text = characterText;
        personName.text = characterName;
    }

    //Coroutine to make selected object red for some time, then fade away
    public IEnumerator FadeRed(GameObject obj)
    {
        obj.GetComponent<Renderer>().material.color = negativeStatsMarkerColor;
        yield return new WaitForSeconds(2);
        obj.SetActive(false);

        obj.GetComponent<Renderer>().material.color = neutralStatsMarkerColor;
        drag.answerChosen = false;
    }

    //Coroutine to make selected object green for some time, then fade away
    public IEnumerator FadeGreen(GameObject obj)
    {
        obj.GetComponent<Renderer>().material.color = positiveStatsMarkerColor;
        yield return new WaitForSeconds(1);
        obj.SetActive(false);

        obj.GetComponent<Renderer>().material.color = neutralStatsMarkerColor;
        drag.answerChosen = false;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Countdown : MonoBehaviour
{
    public Sprite[] sprites;
    public string[] strings;

    private Image image;
    private Text textField;

    public UnityEvent onComplete = null;


    void Awake()
    {
        image = GetComponent<Image>();
        textField = GetComponent<Text>();
    }
    void OnEnable()
    {
        if (image)
            image.sprite = sprites[0];
        if (textField)
            textField.text = strings[0];

        StartCoroutine(_Countdown());
    }

    IEnumerator _Countdown()
    {
        RectTransform rectTran = gameObject.GetComponent<RectTransform>();

        if (image)
            foreach (var num in sprites)
            {
                if(this.gameObject.name == "NextNumber")
                {
                    rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 60);
                    rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 75);
                }
                else
                {
                    if (num.name == SceneManager.GetActiveScene().name + "Title")
                    {
                        rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1210);
                        rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 210);
                    }
                    else
                    {
                        rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 292);
                        rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 403);
                    }
                }
               
                image.sprite = num;
                yield return new WaitForSeconds(1f);
            }
        if (textField)
            foreach (var num in strings)
            {
                textField.text = num;
                yield return new WaitForSeconds(1f);
            }
        onComplete.Invoke();
    }
}

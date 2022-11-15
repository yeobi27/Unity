using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Swip : MonoBehaviour
{
    public GameObject scrollbar;
    public GameObject [] page;
    float scroll_pos = 0f;
    float [] pos;
    //bool wasChanged = false;

    private void OnEnable()
    {
        scrollbar.GetComponent<Scrollbar>().value = 0;
    }

    private void OnDisable()
    {
        page[0].SetActive(true);
        page[1].SetActive(false);
        page[2].SetActive(false);
        page[3].SetActive(false);

        scroll_pos = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length - 1f);

        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }

        if (Input.GetMouseButton(0))
        {
            scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
        }
        else
        {
            for (int i = 0; i < pos.Length ; i++)
            {
                if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                {
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.15f);
                    if(scrollbar.GetComponent<Scrollbar>().value < 0f) { scrollbar.GetComponent<Scrollbar>().value = 0f; }
                }
            }
        }

        CurrentPage();
    }

    private void CurrentPage()
    {
        if (Mathf.Abs(pos[0] - scrollbar.GetComponent<Scrollbar>().value) < 0.001f && !page[0].activeSelf)
        {
            Debug.Log("0");
            page[0].SetActive(true);
            page[1].SetActive(false);
            page[2].SetActive(false);
            page[3].SetActive(false);
        }
        else if (Mathf.Abs(pos[1] - scrollbar.GetComponent<Scrollbar>().value) < 0.001f && !page[1].activeSelf)
        {
            Debug.Log("1");
            page[0].SetActive(false);
            page[1].SetActive(true);
            page[2].SetActive(false);
            page[3].SetActive(false);
        }
        else if (Mathf.Abs(pos[2] - scrollbar.GetComponent<Scrollbar>().value) < 0.001f && !page[2].activeSelf)
        {
            Debug.Log("2");
            page[0].SetActive(false);
            page[1].SetActive(false);
            page[2].SetActive(true);
            page[3].SetActive(false);
        }
        else if (Mathf.Abs(pos[3] - scrollbar.GetComponent<Scrollbar>().value) < 0.001f && !page[3].activeSelf)
        {
            Debug.Log("3");
            page[0].SetActive(false);
            page[1].SetActive(false);
            page[2].SetActive(false);
            page[3].SetActive(true);
        }

        //return wasChanged;
    }

}

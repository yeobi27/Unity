using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //// Active before Renderring : gaming code
    //void Update()
    //{

    //}
    public float speed;

    private Rigidbody rb;

    private int count;

    public Text countText;

    public Text WinText;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        WinText.text = "";
        SetCountText();
    }

    // Active before phisics calc : phisics code
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement*speed);
    }

    // Destroy everything that enters the trigger
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if(count >= 12)
        {
            WinText.text = "You Win";
        }
    }
}


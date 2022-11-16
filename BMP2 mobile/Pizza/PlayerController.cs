using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Pizza
{
    public class PlayerController : MonoBehaviour
    {
        public float pointerFollowSpeed;
        public GameObject pizzaPrefab;

        Pizza.OrderManager _orderManager;
        Camera _cam;
        float _dragThreshold;


        void Start()
        {
            _cam = Camera.main;
            _orderManager = FindObjectOfType<Pizza.OrderManager>();
            _dragThreshold = Screen.height * 0.05f;
            Input.multiTouchEnabled = false;
        }

        private void Update()
        {
            if (AppManager.Instance.gameRunning && PopUpSystem.PopUpState.Equals(false))
                HandleInput();
        }

        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(_CheckForDrag());
            }
        }

        IEnumerator _CheckForDrag()
        {
            Vector3 startPos, currentPos;
            startPos = currentPos = Input.mousePosition;

            float deltaMag;

            while (Input.GetMouseButton(0))
            {
                currentPos = Input.mousePosition;
                deltaMag = (startPos - currentPos).magnitude;

                if (deltaMag > _dragThreshold)
                {
                    StartNewPizza();
                    yield break;
                }
                yield return null;
            }
        }

        public void StartNewPizza()
        {
            StopCoroutine("_StartNewPizza");
            StartCoroutine("_StartNewPizza");
        }

        private IEnumerator _StartNewPizza()
        {
            var mousePosArray = new List<Vector3>();
            var mousePrevious = Input.mousePosition;
            var currentPizza = Instantiate(pizzaPrefab).GetComponent<PizzaDough>();
            float mouseDelta, buildUp;
            float time = 0f;
            int rand = 0;

            SoundManager.Instance.PlaySFX("07 spin a pizza dough");
            currentPizza.transform.position = new Vector3(-3.48f, -0.45f, 5.86f);
            mouseDelta = buildUp = 0f;

            for (int i = 0; i < 3; i++)
            {
                mousePosArray.Add(Input.mousePosition);
            }

            while (Input.GetMouseButton(0) && AppManager.Instance.gameRunning && buildUp < .9999f)
            {
                mouseDelta = (Input.mousePosition - mousePrevious).magnitude;
                mousePrevious = Input.mousePosition;

                buildUp += mouseDelta * 0.00005f;
                buildUp += 0.05f * Time.deltaTime;
                buildUp = Mathf.Clamp(buildUp, 0, .9999f);

                currentPizza.transform.Rotate(0, (mouseDelta * 0.5f) + (100f * Time.deltaTime), 0f, Space.World);
                currentPizza.Knead(buildUp);

                currentPizza.transform.position = Vector3.Lerp(currentPizza.transform.position,
                                                              _cam.ScreenToWorldPoint(new Vector3(mousePosArray[0].x, 
                                                              mousePosArray[0].y + 100, 
                                                              _cam.WorldToScreenPoint(currentPizza.transform.position).z)),
                                                              pointerFollowSpeed * Time.deltaTime);
                CodeWrapper.AbsoluteZ(currentPizza.transform, 6.00843f, Space.Self);

                mousePosArray.RemoveAt(0);
                mousePosArray.Add(Input.mousePosition);

                time += Time.deltaTime;
                if (time > 0.4f + (rand * 0.1f))
                {
                    rand = Random.Range(0, 2);
                    switch (rand)
                    {
                        case 0:
                            SoundManager.Instance.PlaySFX("07 spin a pizza dough");
                            break;
                        case 1:
                            SoundManager.Instance.PlaySFX("09 spin a pizza dough");
                            break;
                    }
                    time = 0f;
                }
                yield return new WaitForFixedUpdate();
            }

            _orderManager.EvaluatePizza(currentPizza);
        }
    }
}
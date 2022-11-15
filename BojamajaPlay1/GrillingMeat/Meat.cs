using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Meats
{
    public string meatName;
    public float points;
    public float overTime;
    public float welldonecookedTime;
}

public class Meat : MonoBehaviour
{
    [SerializeField] Meats[] meats;

    private float top_timeOver;
    private float bottom_timeOver;
    private float topWelldoneTime;
    private float bottomWelldoneTime;
    private float meatPoint;

    private bool b_isSetParticle;
    private bool b_isTopVibeState;
    private bool b_isBottomVibeState;

    private bool b_isRoastingBottom;
    private bool b_isRoastingTop;

    private bool b_isWelldoneBottom;
    private bool b_isWelldoneTop;

    private Animator animator;
    
    private Material cookedMaterial;
    private Material burntMaterial;

    private float exitTime = 0.8f;
    private Vector3 initScale = new Vector3();
    private float timer = 0f;

    [Header("Move Sfx")]
    [SerializeField] string[] sound_move;

    [Header("Cooking Sfx")]
    [SerializeField] string[] sound_cooked;

    // Start is called before the first frame update
    void Start()
    {
        b_isSetParticle = true;

        b_isRoastingBottom = true;
        b_isRoastingTop = false;

        b_isWelldoneBottom = false;
        b_isWelldoneTop = false;

        animator = GetComponent<Animator>();
        initScale = transform.localScale;

        // initialize
        for (int i = 0; i < meats.Length; i++)
        {
            if (meats[i].meatName == this.name)
            {
                top_timeOver = meats[i].overTime;
                bottom_timeOver = meats[i].overTime;
                topWelldoneTime = meats[i].welldonecookedTime;
                bottomWelldoneTime = meats[i].welldonecookedTime;
                meatPoint = meats[i].points;
            }
        }

        // get Material
        // change after location : transform.GetChild(0).gameObject.GetComponentInChildren<MeshRenderer>().material; , transform.GetChild(1).gameObject.GetComponentInChildren<MeshRenderer>().material;
        cookedMaterial = Resources.Load<Material>("Meat/Materials/MeatPack_Cooked");
        burntMaterial = Resources.Load<Material>("Meat/Materials/MeatPack_Burnt");  
    }

    // Update is called once per frame
    void Update()
    {
        // roasting bottom
        if (b_isRoastingBottom)
        {
            // both are good
            if (b_isWelldoneBottom && b_isWelldoneTop)
            {
                b_isWelldoneBottom = false;
                b_isWelldoneTop = false;

                GrillingMeat_SoundManager.Instance.PlaySE(sound_cooked[0]);

                GrillingMeat_DataManager.Instance.AddScore(meatPoint);

                StartCoroutine(_OnClear());
            }

            // grilling
            bottom_timeOver -= Time.deltaTime;

            // 8sec < 4sec : vibe
            if (bottom_timeOver < bottomWelldoneTime)
            {
                timer += Time.deltaTime * 20;
            
                // 0.8 ~ 1.8
                float scale = Mathf.Sin(timer) + 0.7f;

                if (initScale.x < scale)
                {
                    Scaling(new Vector3(scale, scale, scale));
                }               

                if (b_isSetParticle)
                {
                    b_isSetParticle = false;
                    transform.GetChild(2).gameObject.SetActive(true);
                }               
            }

            if (bottom_timeOver < 0f)
            {
                StartCoroutine(_OnDestroy());
            }
        }
        else if (b_isRoastingTop)   // roasting Top
        {
            if (b_isWelldoneBottom && b_isWelldoneTop)
            {
                b_isWelldoneBottom = false;
                b_isWelldoneTop = false;

                GrillingMeat_SoundManager.Instance.PlaySE(sound_cooked[0]);
                
                GrillingMeat_DataManager.Instance.AddScore(meatPoint);

                StartCoroutine(_OnClear());
            }

            top_timeOver -= Time.deltaTime;

            if (top_timeOver < topWelldoneTime)
            {
                timer += Time.deltaTime * 20;

                // 0.8 ~ 1.8
                float scale = Mathf.Sin(timer) + 0.7f;

                if (initScale.x < scale)
                {
                    Scaling(new Vector3(scale, scale, scale));
                }

                if (b_isSetParticle)
                {
                    b_isSetParticle = false;

                    transform.GetChild(2).gameObject.SetActive(true);
                }
            }

            if (top_timeOver < 0f)
            {
                GrillingMeat_SoundManager.Instance.PlaySE(sound_cooked[1]);

                StartCoroutine(_OnDestroy());
            }
        }
    }
   
    public float Scaling(Vector3 scale)
    {
        transform.localScale = scale;

        return transform.localScale.x;
    }

    IEnumerator _OnClear()
    {
        WaitForSecondsRealtime ws = new WaitForSecondsRealtime(0.4f);

        yield return new WaitForSecondsRealtime(0.4f); ;
        Destroy(gameObject);

        // Particle On
        transform.parent.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1f); ;

        yield return null;
    }

    IEnumerator _OnDestroy()
    {
        transform.GetChild(0).gameObject.GetComponentInChildren<MeshRenderer>().material = burntMaterial;
        transform.GetChild(1).gameObject.GetComponentInChildren<MeshRenderer>().material = burntMaterial;

        yield return new WaitForSecondsRealtime(0.2f);

        GrillingMeat_SoundManager.Instance.PlaySE(sound_cooked[1]);

        //yield return new WaitUntil(() => Scaling(new Vector3(transform.localScale.x - 0.001f, transform.localScale.y - 0.001f, transform.localScale.z - 0.001f)) > 0.1f);

        Destroy(gameObject);

        // Particle On
        transform.parent.gameObject.transform.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        
        yield return null;
    }

    public void TurnUp()
    {
        timer = 0f;

        b_isSetParticle = true;
        transform.GetChild(2).gameObject.SetActive(false);

        GrillingMeat_SoundManager.Instance.PlaySE(sound_move[Random.Range(0, 2)]);

        if (CurrentAnimationStateCheck("RoastingMeat"))
        {
            //StopAllCoroutines();
            StartCoroutine(_TurnUp());
        }
    }

    IEnumerator _TurnUp()
    {
        WaitForSecondsRealtime ws = new WaitForSecondsRealtime(0.3f);


        if (b_isRoastingBottom)
        {
            animator.SetBool("TopFaceReverse", true);
            //animator.SetTrigger("TopFaceReverse");

            b_isRoastingBottom = false;
            b_isRoastingTop = true;
                
            // bottomWelldoneTime
            // bottom_timeOver(8sec) bottomWelldoneTime(4sec),
            // bottom_timeOver > 0 : time before burn black
            if (bottom_timeOver < bottomWelldoneTime && bottom_timeOver > 0)
            {
                // Welldone
                b_isWelldoneBottom = true;
            }
            else
            {
                // top ripe or unripe, bottom side raw
                b_isWelldoneBottom = false;
            }

            // When turned over, the object changes to a ripe state when it is ripe.
            if (b_isWelldoneBottom)
            {
                // Only the lower part is changed to a ripe state
                // Mesh Renderer > Materials
                transform.GetChild(1).gameObject.GetComponentInChildren<MeshRenderer>().material = cookedMaterial;
            }
        }
        else if (b_isRoastingTop)
        {
            animator.SetBool("BottomFaceReverse", true);
 
            b_isRoastingBottom = true;
            b_isRoastingTop = false;

            if (top_timeOver < topWelldoneTime && top_timeOver > 0)
            {
                b_isWelldoneTop = true;
            }
            else
            {
                b_isWelldoneTop = false;
            }

            if (b_isWelldoneTop)
            {
                transform.GetChild(0).gameObject.GetComponentInChildren<MeshRenderer>().material = cookedMaterial;
            }
        }

        yield return ws;

        if (b_isRoastingBottom)
        {
            animator.SetBool("BottomFaceReverse", false);
        }
        else if(b_isRoastingTop)
        {
            animator.SetBool("TopFaceReverse", false);            
        }
    }

    private bool CurrentAnimationStateCheck(string aniName)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(aniName);
    }
}


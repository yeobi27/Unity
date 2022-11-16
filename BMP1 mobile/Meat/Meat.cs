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

    void Start()
    {
        b_isSetParticle = true;

        b_isRoastingBottom = true;
        b_isRoastingTop = false;

        b_isWelldoneBottom = false;
        b_isWelldoneTop = false;

        animator = GetComponent<Animator>();
        initScale = transform.localScale;

        // 고기당 굽는시간 초기화
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

        // 윗면, 아래면 탄거, 잘익은거 Material 가져오기
        // 나중에 바꿔줘야할 곳 : transform.GetChild(0).gameObject.GetComponentInChildren<MeshRenderer>().material; , transform.GetChild(1).gameObject.GetComponentInChildren<MeshRenderer>().material;
        cookedMaterial = Resources.Load<Material>("Meat/Materials/MeatPack_Cooked");
        burntMaterial = Resources.Load<Material>("Meat/Materials/MeatPack_Burnt");  
    }

    // Update is called once per frame
    void Update()
    {
        // 아래 굽는중
        if (b_isRoastingBottom)
        {
            // 양쪽 다 익음
            if (b_isWelldoneBottom && b_isWelldoneTop)
            {
                b_isWelldoneBottom = false;
                b_isWelldoneTop = false;

                // 반짝 파티클 생기고 잘구워졌을 때 효과음 나오고
                GrillingMeat_SoundManager.Instance.PlaySE(sound_cooked[0]);

                // 점수 계산하고
                GrillingMeat_DataManager.Instance.AddScore(meatPoint);
                // 사라짐
                StartCoroutine(_OnClear());
            }

            // 굽는 중
            bottom_timeOver -= Time.deltaTime;
            //Debug.Log("고기종류 : " + gameObject + "아래쪽 굽는중 : " + bottom_timeOver);

            // 8초... 줄어듦.. < 4초 : 익은상태 이후 , 바이브
            if (bottom_timeOver < bottomWelldoneTime)
            {
                timer += Time.deltaTime * 20;
            
                // 0.8 ~ 1.8
                float scale = Mathf.Sin(timer) + 0.7f;

                if (initScale.x < scale)
                {
                    Scaling(new Vector3(scale, scale, scale));
                }
               
                // 파티클..
                if (b_isSetParticle)
                {
                    b_isSetParticle = false;
                    transform.GetChild(2).gameObject.SetActive(true);
                }               
            }

            if (bottom_timeOver < 0f)
            {
                // 파괴
                StartCoroutine(_OnDestroy());
            }
        }
        else if (b_isRoastingTop)   // 윗쪽 굽는중
        {
            // 양쪽 다 익음
            if (b_isWelldoneBottom && b_isWelldoneTop)
            {
                b_isWelldoneBottom = false;
                b_isWelldoneTop = false;

                // 반짝 파티클 생기고 잘구워졌을 때 효과음 나오고
                GrillingMeat_SoundManager.Instance.PlaySE(sound_cooked[0]);
                
                // 점수 계산하고
                GrillingMeat_DataManager.Instance.AddScore(meatPoint);

                // 사라짐
                StartCoroutine(_OnClear());
            }

            // 굽는 중
            top_timeOver -= Time.deltaTime;

            // 8초... 줄어듦.. < 4초 : 익은상태 이후..
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
                    // 파티클..
                    transform.GetChild(2).gameObject.SetActive(true);
                }
            }

            if (top_timeOver < 0f)
            {
                // 실패시 사운드
                GrillingMeat_SoundManager.Instance.PlaySE(sound_cooked[1]);
                // 파괴
                StartCoroutine(_OnDestroy());
            }
        }
    }
   
    public float Scaling(Vector3 scale)
    {
        transform.localScale = scale;

        return transform.localScale.x;
    }
    
    // 성공시
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

    // 실패시
    IEnumerator _OnDestroy()
    {
        //WaitForSecondsRealtime ws = new WaitForSecondsRealtime(0.4f);
        
        transform.GetChild(0).gameObject.GetComponentInChildren<MeshRenderer>().material = burntMaterial;
        transform.GetChild(1).gameObject.GetComponentInChildren<MeshRenderer>().material = burntMaterial;

        // 파티클 생기고
        yield return new WaitForSecondsRealtime(0.2f);

        // 실패시 사운드
        GrillingMeat_SoundManager.Instance.PlaySE(sound_cooked[1]);
        // 스케일 작아지면서 사라지기
        //yield return new WaitUntil(() => Scaling(new Vector3(transform.localScale.x - 0.001f, transform.localScale.y - 0.001f, transform.localScale.z - 0.001f)) > 0.1f);

        Destroy(gameObject);

        // Particle On
        transform.parent.gameObject.transform.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        
        yield return null;
    }

    // 뒤집어!
    public void TurnUp()
    {
        timer = 0f;
        // 현재 애니메이션 정지 시키고 뒤집
        b_isSetParticle = true;
        transform.GetChild(2).gameObject.SetActive(false);
        // 효과음
        GrillingMeat_SoundManager.Instance.PlaySE(sound_move[Random.Range(0, 2)]);
        // 상태체크 : 뒤집는 애니메이션이 아닐 때
        if (CurrentAnimationStateCheck("RoastingMeat"))
        {
            //StopAllCoroutines();
            StartCoroutine(_TurnUp());
        }
    }

    IEnumerator _TurnUp()
    {
        WaitForSecondsRealtime ws = new WaitForSecondsRealtime(0.3f);

        // 아래굽는중
        if (b_isRoastingBottom)
        {
            animator.SetBool("TopFaceReverse", true);
            //animator.SetTrigger("TopFaceReverse");

            b_isRoastingBottom = false;
            b_isRoastingTop = true;
                
            //Debug.Log("위쪽이 구워지게 뒤집음");
            // bottomWelldoneTime : 초기화때 저장해놓은 잘 구워지는 시간
            // 아래쪽 굽는중이었음
            // bottom_timeOver(8초) 가 bottomWelldoneTime(4초) 보다 낮고, 
            // bottom_timeOver > 0 : 타기전까지 시간
            if (bottom_timeOver < bottomWelldoneTime && bottom_timeOver > 0)
            {
                // 잘익음
                b_isWelldoneBottom = true;
            }
            else
            {
                // 위쪽이 익거나 안익거나, 아래쪽면은 익지않은 상태
                b_isWelldoneBottom = false;
            }

            // 뒤집었더니 잘 익은 상태면 색 익은 상태로 오브젝트 변함
            if (b_isWelldoneBottom)
            {
                // 잘 익은 상태가 바뀌지 않음
                // 아랫부위만 익은상태로 바꿈
                // Mesh Renderer > Materials 를 바꿈
                transform.GetChild(1).gameObject.GetComponentInChildren<MeshRenderer>().material = cookedMaterial;
            }
        }
        else if (b_isRoastingTop)
        {
            animator.SetBool("BottomFaceReverse", true);
 
            b_isRoastingBottom = true;
            b_isRoastingTop = false;

            //Debug.Log("아래쪽이 구워지게 뒤집음");
            // 아래쪽 굽는중 , overTimeLeft 가 lowerTimeLeft 보다 낮고, overTimeLeft가 0 보다 낮으면 탔음
            if (top_timeOver < topWelldoneTime && top_timeOver > 0)
            {
                // 잘익음
                b_isWelldoneTop = true;
            }
            else
            {
                // 아래쪽이 익거나 안익거나, 위쪽면은 익지않은 상태
                b_isWelldoneTop = false;
            }

            // 뒤집었더니 잘 익은 상태면 색 익은 상태로 오브젝트 변함
            if (b_isWelldoneTop)
            {
                // 잘 익은 상태가 바뀌지 않음
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

    // 현재 애니메이션 체크
    private bool CurrentAnimationStateCheck(string aniName)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(aniName);
    }
}


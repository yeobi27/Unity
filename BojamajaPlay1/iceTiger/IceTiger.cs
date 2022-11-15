using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTiger : MonoBehaviour
{
    private Vector3 initPos;
    private float move;
    private bool randMove = false;
    private float DownCount;
    private Animator animator;
    private GameObject hitParticle;

    [Header("BackHoe Info")]
    public float health = 10;
    public float damage = 10;
    public float points = 1000;

    [Header("Random Info")]
    public float speed = 15;
    public float minSecondbetweenCubes = 1f;
    public float maxSecondbetweenCubes = 3f;

    [Header("Hit Sfx")]
    [SerializeField] string[] sound_hit;

    [Header("Move Sfx")]
    [SerializeField] string[] sound_move;

    private void Start()
    {
        animator = GetComponent<Animator>();
        initPos = gameObject.transform.position;

        animator.SetBool("Idle", true);
    }

    private void Update()
    {
        if (randMove)
        {
            if (initPos == transform.position)
            {
                IceTiger_SoundManager.Instance.PlaySE(sound_move[Random.Range(0, 3)]);
            }

            move = Time.deltaTime * speed;
            DownCount -= Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, new Vector3(initPos.x, initPos.y + 0.2f, initPos.z), move);

	    // Here, the time must be randomly given and passed, the routine going straight down is not allowed
            // Whether it is smaller than the randomly given time or the position value that has been raised
            if (DownCount < 0f && transform.position == new Vector3(initPos.x, initPos.y + 0.2f, initPos.z))
            {
                randMove = false;
                StartCoroutine(_OnDown());
            }
        }
    }

    public void OnDown()
    {
        if (randMove)
        {
            hitParticle = Instantiate(IceTiger_DataManager.Instance.hitParticle[Random.Range(0, 3)], gameObject.transform);
            hitParticle.transform.position = new Vector3(transform.position.x, transform.position.y + 0.4f, transform.position.z);
            
            IceTiger_SoundManager.Instance.PlaySE(sound_hit[Random.Range(0, 3)]);

            health -= IceTiger_DataManager.Instance.hammerDamage;

            if (health <= 0)
            {
                //Debug.Log("points! : " + points);
                IceTiger_DataManager.Instance.AddScore(points);
                health = 10;
            }
        }

        randMove = false;

        animator.SetBool("Idle", false);
        animator.SetBool("Eat", true);
        StartCoroutine(_OnDown());
    }

    IEnumerator _OnDown()
    {
        yield return new WaitUntil(() => DownCharacter());
    }

    private bool DownCharacter()
    {
        if (transform.position != initPos)
        {
            move = Time.deltaTime * speed;
            transform.position = Vector3.Lerp(transform.position, initPos, move);
            return false;
        }
        else
        {
            animator.SetBool("Idle", true);
            animator.SetBool("Eat", false);
            return true;
        }
    }

    private void OnEnable()
    {
        IceTiger_AppManager.RoundStart += RoundStart;
        IceTiger_Timer.RoundEnd += RoundEnd;
    }

    private void OnDisable()
    {
        IceTiger_AppManager.RoundStart -= RoundStart;
        IceTiger_Timer.RoundEnd -= RoundEnd;
    }

    private void RoundStart()
    {
        StopAllCoroutines();
        StartCoroutine(_RoundStart());
    }

    private IEnumerator _RoundStart()
    {
        float UpCount;

        yield return new WaitUntil(() => IceTiger_Timer.isPlaying);

        while (IceTiger_Timer.Instance.timeLeft > 0)
        {
            // rand 1, 2 
            // 2.3 ~ 2.6

            if (IceTiger_Timer.Instance.timeLeft > 19.9f)
            {
                // 2.3 ~ 2.6
                UpCount = 2f + (Random.Range(minSecondbetweenCubes, maxSecondbetweenCubes) * 0.3f);
            }
            else if(IceTiger_Timer.Instance.timeLeft > 9f)
            {
                // 1.5 ~ 1.8
                UpCount = 1.5f + (Random.Range(minSecondbetweenCubes, maxSecondbetweenCubes) * 0.3f);
            }
            else
            {
                // 1.5 ~ 1.8
                UpCount = 1f + (Random.Range(minSecondbetweenCubes, maxSecondbetweenCubes) * 0.3f);
            }

            // 0.7 ~ 0.9

            if (IceTiger_Timer.Instance.timeLeft > 19.9f)
            {
                // 0.7 ~ 0.9
                DownCount = 0.5f + (Random.Range(minSecondbetweenCubes, maxSecondbetweenCubes) * 0.2f);
            }
            else if(IceTiger_Timer.Instance.timeLeft > 9f)
            {
                // 0.6 ~ 0.7
                DownCount = 0.4f + (Random.Range(minSecondbetweenCubes, maxSecondbetweenCubes) * 0.2f);
            }
            else
            {
                // 0.4 ~ 0.5
                DownCount = 0.15f + (Random.Range(minSecondbetweenCubes, maxSecondbetweenCubes) * 0.2f);
            }

            // 1 == Up / 0 == Down
            randMove = Random.Range(0, 2) == 1 ? true : false;
            //Debug.Log("randMove: " + randMove);
            /*********************************************************************/

            // 2.3 ~ 2.6
            yield return new WaitForSecondsRealtime(UpCount);
        }

        yield return null;
    }

    private void RoundEnd()
    {
        StopAllCoroutines();
        StartCoroutine(_RoundEnd());
    }

    private IEnumerator _RoundEnd()
    {
        yield return new WaitUntil(() => DownCharacter());

        yield return null;
    }
}

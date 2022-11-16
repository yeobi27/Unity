using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoAd : MonoBehaviour
{
    Animator animator;

    public static GoAd Instance { get; private set; }
    void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else Instance = this;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public IEnumerator OnRoundStart()
    {
        animator.SetBool("BalloonMoving", true);
        yield return null;
    }

    private void OnEnable()
    {
        CatchPang_Timer.RoundEnd += OnRoundEnd;
    }

    void OnRoundEnd()
    {
        StopAllCoroutines();
        animator.SetBool("BalloonMoving", false);
    }

    private IEnumerator Go()
    {
        yield return new WaitUntil(() => CatchPang_Timer.isPlaying);

        animator.SetBool("BalloonMoving", true);
    }
}

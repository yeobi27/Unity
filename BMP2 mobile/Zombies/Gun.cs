using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Pistol에 존재 
public class Gun : MonoBehaviour
{
    public Transform gunPoint;
    public ParticleSystem muzzleFlash; //총 화구에서 나오는 파티클
    public GameObject bullet;
    public int damage;

    Transform _pivot;
    Animator _animator;
    ZombieSpawner _zombieSpawner;
    Zombie _closestZ;
    Vector3 _startingEulerAngles;
    Vector3 _startingPosition;
    public static GameObject _bullet;

    void Start()
    {
        _startingPosition = transform.localPosition;
        _startingEulerAngles = transform.localEulerAngles;
        _zombieSpawner = FindObjectOfType<ZombieSpawner>();
        _pivot = transform.parent;
        _animator = GetComponent<Animator>();
    }
 
    public void Shoot()
    {
        transform.localPosition = _startingPosition;
        transform.localEulerAngles = _startingEulerAngles;
        _animator.SetTrigger("Shoot"); //총이 움직이는 애니메이션

        muzzleFlash.Play(); //총 구,파티클
        SoundManager.Instance.PlaySFX("pistol shot", 3f); //총 발사, 사운드

        // spawn bullet trail
        _bullet = Instantiate(bullet);
        _bullet.transform.position = gunPoint.position; //gunPoint에서 총알 발사   
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mosquito
{
    public class Insect : MonoBehaviour
    {
        private Animator animator;
        private new Rigidbody rigidbody;
        private new Collider collider;
        private InsectSpawner spawner;
        private Vector3 scale;
        private Vector3 destination;

        public GameObject slapParticle;
        public int points;
        public float flySpeed = 15f;
        public float growthSpeed = 10f;
        public float floatSpeed = 3f;
        public float floatAmplitude = 0.01f;


        void Awake()
        {
            scale = transform.localScale;

            transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().receiveShadows = false;
        }
        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            collider = GetComponent<Collider>();
            spawner = transform.parent.GetComponent<InsectSpawner>();
            animator = GetComponent<Animator>();

            StartCoroutine(Grow());
        }
        void Update()
        {
            // Float
            transform.localPosition += new Vector3(0f, Mathf.Sin(Time.time * floatSpeed) * floatAmplitude, 0f);
        }

        IEnumerator Fly()
        {
            destination = spawner.GetRandomPointInCollider();
            animator.SetTrigger("Fly");

            Vector3 dir = destination - transform.position;
            dir = new Vector3(dir.x, 0f, dir.z);
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);

            while (Vector3.Distance(transform.position, destination) > 0.1f)
            {
                rigidbody.AddForce((destination - transform.position).normalized * flySpeed, ForceMode.Force);

                yield return new WaitForFixedUpdate();
            }

            animator.ResetTrigger("Fly");

            StartCoroutine(Idle());
        }
        IEnumerator Idle()
        {
            float duration = Random.Range(1f, 2f);
            animator.SetTrigger("Idle");

            while (duration > 0f)
            {
                duration -= Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }

            animator.ResetTrigger("Idle");

            StartCoroutine(Fly());
        }
        IEnumerator Grow()
        {
            Vector3 newScale = Vector3.zero;
            float size = 0f;

            while (size < scale.x)
            {
                size += growthSpeed * Time.deltaTime;
                newScale = new Vector3(size, size, size);

                transform.localScale = newScale;

                yield return new WaitForFixedUpdate();
            }

            transform.localScale = scale;

            StartCoroutine(Fly());
        }

        public void Perish()
        {
            Instantiate(slapParticle, transform.position, Quaternion.identity);

            DataManager.Instance.scoreManager.Add(points);

            Destroy(gameObject);
            Resources.UnloadUnusedAssets();
        }
    }
}

using UnityEngine;
using System.Collections;

namespace Mosquito
{
    public class PlayerController : MonoBehaviour
    {
        private Camera cam;
        private Vector3 rayPointPos;
        
        public float trailDist;
        public LayerMask layerMask;

        public static PlayerController Instance { get; private set; }


        void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this.gameObject);
            else Instance = this;
        }
        void Start()
        {
            cam = GetComponentInChildren<Camera>();
            if (!cam)
                cam = Camera.main;
        }
        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 15f, layerMask))
                {
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Insects"))
                    {
                        hit.transform.GetComponent<Insect>().Perish();

                        SoundManager.Instance.PlaySFX("slap-sound");
                    }
                }

                rayPointPos = ray.GetPoint(trailDist);
                transform.GetChild(0).position = rayPointPos;
            }
        }
    }
}
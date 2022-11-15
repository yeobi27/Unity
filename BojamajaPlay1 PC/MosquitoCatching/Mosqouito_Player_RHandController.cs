using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mosquito
{
    public class Mosqouito_Player_RHandController : MonoBehaviour
    {
        public GameObject rightHand_currentPos;
        public GameObject leftHand_currentPos;
        public bool is_RColCheck = false;

        private void Start()
        {
            is_RColCheck = false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Insects"))
            {
                collision.collider.gameObject.transform.GetComponent<Insect>().Perish();
                SoundManager.Instance.PlaySFX("slap-sound");
            }
        }
    }
}


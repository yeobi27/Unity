using System;
using System.Collections;
using UnityEngine;

namespace Mosquito
{
    public class AppManager_Mosquito : AppManager
    {
        private InsectSpawner spawner;
        private LightFlicker lightFlicker;


        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            spawner = FindObjectOfType<InsectSpawner>();
            lightFlicker = FindObjectOfType<LightFlicker>();
        }

        protected override IEnumerator _OnRoundStart()
        {
            yield return base._OnRoundStart();

            spawner.StartSpawner();
            lightFlicker.StartFlicker();
        }

        protected override IEnumerator _OnRoundEnd()
        {
            yield return base._OnRoundEnd();

            spawner.OnRoundEnd();
            lightFlicker.StopFlicker();
        }

        private void OnDestroy()
        {
            Resources.UnloadUnusedAssets();
        }
    }
}
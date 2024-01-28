using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace PanicBuying
{
    public class InGameTimer : NetworkBehaviour
    {
        [SerializeField]
        float initialRemainTime = 900.0f;

        private NetworkVariable<float> remainTime = new(900.0f);

        public float RemainTime { get => remainTime.Value; }

        private void Awake()
        {
            remainTime.Value = initialRemainTime;
        }

        private void Update()
        {
            remainTime.Value -= Time.deltaTime;

            if (remainTime.Value <= 0.0f)
            {
                Event.Emit(new GameOver());

                enabled = false;
            }
        }
    }
}
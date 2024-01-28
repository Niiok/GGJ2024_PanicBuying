using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PanicBuying
{

    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TimeText : MonoBehaviour
    {
        [SerializeField]
        private InGameTimer timer;

        private TextMeshProUGUI textUI = null;

        private void Awake()
        {
            textUI = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            const int oneDayBySecond = 3600 * 24;

            int currentTimeBySecond = oneDayBySecond - (int)(timer.RemainTime * 64);
            var hour = currentTimeBySecond / 3600;
            var minute = (currentTimeBySecond % 3600) / 60;

            var hourString = hour < 10 ? "0" + hour.ToString() : hour.ToString();
            var minuteString = minute < 10 ? "0" + minute.ToString() : minute.ToString();

            textUI.text = string.Format("{0}:{1}", hourString, minuteString);
        }
    }
}
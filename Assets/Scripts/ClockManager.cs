using System;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class ClockManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text timeText;
        private DateTime startTime;
    
        void Start()
        {
            startTime = DateTime.Now;
        }

        void Update()
        {
            TimeSpan timeSinceStart = DateTime.Now.Subtract(startTime);
            string timeString = $"{Math.Floor(timeSinceStart.TotalMinutes)}:{Math.Floor(timeSinceStart.TotalSeconds % 60):00}";
            timeText.SetText(timeString);
        
        }
    }
}

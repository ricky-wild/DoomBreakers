

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace wildlogicgames
{
    public class FPSCalculator : MonoBehaviour
    {
        [Header("Measurement Time Period")]
        [Tooltip("The number of seconds to measure the frame rate over")]
        public float _measurementPeriod = 1.0f;

        [Header("Text To Display")]
        [Tooltip("Plugin TMP object")]
        public TextMeshProUGUI _displayText;

        private float _fps;
        private float _timer;
        private int _frames;

        void Update()
        {
            if (_displayText == null) return;

            _frames++;
            _timer += Time.deltaTime;
            if (_timer >= _measurementPeriod)
            {
                _fps = _frames / _timer;
                _timer = 0.0f;
                _frames = 0;
                _displayText.text = GetFPS();
            }
        }

        public string GetFPS() => "FPS " + _fps.ToString("F1");

        //void OnGUI() => GUI.Label(new Rect(10, 10, 100, 20), "FPS: " + _fps.ToString("F2"));
    }
}


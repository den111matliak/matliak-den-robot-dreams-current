using System;
using TMPro;
using UnityEngine;

namespace Lesson14
{
    public class ScoreCanvas : MonoBehaviour
    {
        [SerializeField] private ScoreSystem _scoreSystem;

        [SerializeField] private CanvasGroup _canvasGroup;

        [SerializeField] private TextMeshProUGUI _kills;
        [SerializeField] private TextMeshProUGUI _headshots; // ✅ Make sure this is assigned properly
        [SerializeField] private TextMeshProUGUI _deaths;
        [SerializeField] private TextMeshProUGUI _assists;
        [SerializeField] private TextMeshProUGUI _accuracy;

        private bool _updateRequested;

        private void Start()
        {
            _scoreSystem.OnDataUdpated += DataUpdateHandler;
            Lesson8.InputController.OnScoreInput += ScoreInputHandler;

            _updateRequested = true;

            // ✅ Hide score screen at start
            _canvasGroup.alpha = 0f;
        }

        private void DataUpdateHandler()
        {
            _updateRequested = true;
        }

        private void LateUpdate()
        {
            if (!_updateRequested)
                return;
            _updateRequested = false;

            _kills.text = _scoreSystem.KDA.x.ToString();
            _headshots.text = _scoreSystem.Headshots.ToString(); // ✅ Now correctly updates Headshots
            _deaths.text = _scoreSystem.KDA.y.ToString();
            _assists.text = _scoreSystem.KDA.z.ToString();
            _accuracy.text = _scoreSystem.Accuracy.ToString();
        }

        private void ScoreInputHandler(bool show)
        {
            _canvasGroup.alpha = show ? 1f : 0f;
        }
    }
}

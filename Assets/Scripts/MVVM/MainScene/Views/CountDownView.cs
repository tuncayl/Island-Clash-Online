using onlinetutorial.Extensions;
using onlinetutorial.Signals;
using TMPro;
using UnityEngine;

namespace onlinetutorial.MVVM
{
    public class CountDownView : MonoBehaviour
    {
        #region SelfVariables

        [Header("UIObjects")] [SerializeField] private TextMeshProUGUI GameCountDown;

        [SerializeField] private TextMeshProUGUI SpawnCoutDown;

        private ICountDownViewModel _CountDownViewModel;

        #endregion

        #region UnityMethods

        void Awake()
        {
            _CountDownViewModel = GetComponent<ICountDownViewModel>();
        }

        void Start()
        {
        }

        void OnEnable()
        {
            UiMainSignals.Instance.OnActiveSpawnCoutDown += HandleOnActiveSpawnCount;
            UiMainSignals.Instance.OnChangeSpawnCoutDown += HandleOnChangeSpawnCount;
            UiMainSignals.Instance.OnChangeGameCoutDown += HandleOnChangeGameCount;
        }

        void OnDisable()
        {
            UiMainSignals.Instance.OnActiveSpawnCoutDown -= HandleOnActiveSpawnCount;
            UiMainSignals.Instance.OnChangeSpawnCoutDown -= HandleOnChangeSpawnCount;
            UiMainSignals.Instance.OnChangeGameCoutDown -= HandleOnChangeGameCount;
        }

        #endregion

        private void HandleOnActiveSpawnCount(bool value) => SpawnCoutDown.gameObject.SetActive(value);

        private void HandleOnChangeSpawnCount(int value) => SpawnCoutDown.text = value.ToString();

        private void HandleOnChangeGameCount(int value) => GameCountDown.text = value.ConvertMinuteSecond();
    }
}
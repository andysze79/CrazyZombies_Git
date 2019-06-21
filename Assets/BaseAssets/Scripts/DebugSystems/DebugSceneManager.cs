using UnityEngine;

namespace BaseAssets.Debugger
{
    public class DebugSceneManager : MonoBehaviour
    {
        public static DebugSceneManager Instance = null;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                if (Time.timeScale < 10)
                {
                    Time.timeScale += 1;
                }
            }

            if (Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                if (Time.timeScale > 0)
                {
                    Time.timeScale -= 1;
                }
            }
        }
    }
}
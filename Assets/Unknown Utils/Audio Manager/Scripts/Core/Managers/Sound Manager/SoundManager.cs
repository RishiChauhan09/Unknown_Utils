using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unknown.Manager.AudioManager {
    public partial class SoundManager : MonoBehaviour {

        #region varibales

        private static SoundManager _instance;
        public static SoundManager Instance {
            get {
                if(_instance == null) {
                    Debug.LogWarning("Make sure you initialize AudioManager using AudioManager.Init()");
                    return null;
                }
                return _instance;
            }
        }

        [Header("Sound Manager References")]
        [SerializeField] private AudioDatabase audioDatabase;

        #endregion

        private void Awake() {
            InitializePlayback();

            DontDestroyOnLoad(gameObject);
        }


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Init() {
            var prefab = Resources.Load<GameObject>(nameof(SoundManager));

            if(prefab == null) {
                Debug.LogWarning("Failed to load SoundManager prefab.");
                return;
            }

            var instance = Instantiate(prefab);
            _instance = instance.GetComponent<SoundManager>();
        }

    }
}
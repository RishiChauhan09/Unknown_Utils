using UnityEngine;

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
            InitializeSoundManagerMasterSettings();
            InitializeSoundManagerCategorySettings();
            InitializeSoundManagerEffects();

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
            instance.name = "[Unknown Sound Manager]";
            _instance = instance.GetComponent<SoundManager>();
        }

    }
}
namespace Unknown.Manager.AudioManager {

    [System.Flags]
    public enum AudioType {
        None = 0,
        SFX = 1 << 0,
        Music = 1 << 1,
        Ambient = 1 << 2,
        Voice = 1 << 3,
        UI = 1 << 4,
    }
}
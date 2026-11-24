namespace Unknown.Manager.AudioManager {

    [System.Flags]
    public enum RandomFlags {
        None = 0,
        Volume = 1 << 0,
        Pitch = 1 << 2
    }
}
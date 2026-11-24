namespace Unknown.Manager.AudioManager {

    public enum ClipSelectionStrategy {
        Single,             // selects the first clip
        Random,             // selects the random clip
        Shuffle,            // same as random but makes sure no clip is repeated
        Sequence,           // 0 -> end and then restarts 
        PingPong,           // 0 -> end and then end -> 0
    }
}
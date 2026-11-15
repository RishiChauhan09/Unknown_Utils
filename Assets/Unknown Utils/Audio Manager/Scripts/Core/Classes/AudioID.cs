using System;

namespace Unknown.Manager.AudioManager {

    [System.Serializable]
    public class AudioID : IEquatable<AudioID> {

        public string id;
        public string groupName;

        public string Value => groupName + id;

        public bool Equals(AudioID other) {
            return Value == other.Value;
        }

        public override bool Equals(object obj) {
            return obj is AudioID other && Equals(other);
        }

        public override int GetHashCode() {
            return Value?.GetHashCode() ?? 0;
        }

        public static bool operator ==(AudioID a, AudioID b) => a.Equals(b);
        public static bool operator !=(AudioID a, AudioID b) => !a.Equals(b);

        public override string ToString() => Value;


        public static class NameOf {
            public static string ID = nameof(id);
            public static string GroupName = nameof(groupName);
        }


    }
}
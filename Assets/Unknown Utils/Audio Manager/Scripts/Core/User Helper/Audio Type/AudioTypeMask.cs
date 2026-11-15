using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unknown.Manager.AudioManager {

    [Serializable]
    public struct AudioTypeMask {
        [SerializeField] private uint value;

        public uint Value => value;

        public bool Contains(AudioType type) {
            return type != null && (value & type.Mask) != 0;
        }

        public void Add(AudioType type) {
            if(type != null)
                value |= type.Mask;
        }

        public void Remove(AudioType type) {
            if(type != null)
                value &= ~type.Mask;
        }

        public IEnumerable<AudioType> GetAudioTypes(IEnumerable<AudioType> allAudioTypes) {
            foreach(AudioType type in allAudioTypes) {
                if(Contains(type))
                    yield return type;
            }
        }
    }
}
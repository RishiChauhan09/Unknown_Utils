using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unknown.Manager.AudioManager {

    [Serializable]
    public struct AudioTypeMask {
        [SerializeField] public uint maskValue;

        public bool Contains(AudioType type) {
            return type != null && (maskValue & type.Mask) != 0;
        }

        public void Add(AudioType type) {
            if(type != null)
                maskValue |= type.Mask;
        }

        public void Remove(AudioType type) {
            if(type != null)
                maskValue &= ~type.Mask;
        }

        public IEnumerable<AudioType> GetAudioTypes(IEnumerable<AudioType> allAudioTypes) {
            foreach(AudioType type in allAudioTypes) {
                if(Contains(type))
                    yield return type;
            }
        }

        /// <summary>
        /// Returns the audio type from string of name
        /// </summary>
        public static AudioType NameToAudioType(string name) {
            AudioCategory audioCategory = Resources.Load(nameof(AudioCategory)) as AudioCategory;
            int index = audioCategory.GetIndex(name);
            if(index < 0)
                return null;

            return new AudioType() {
                audioTypeName = name,
                index = index
            };
        }

        public static class NameOf {
            public static string MaskValue = nameof(maskValue);
        }
    }
}
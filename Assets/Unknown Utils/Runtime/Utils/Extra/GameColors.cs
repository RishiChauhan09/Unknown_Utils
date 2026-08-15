using System.Runtime.CompilerServices;
using UnityEngine;

namespace Unknown.Utils {
    public static class GameColors {

        public static Color goldenYellow {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return new Color32(0xFE, 0xCA, 0x57, 0xFF);
            }
        }

        public static Color warmOrange {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return new Color32(0xFF, 0x9F, 0x43, 0xFF);
            }
        }

        public static Color gray {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return new Color32(0xAD, 0xAD, 0xAD, 0xFF);
            }
        }

        public static Color darkGray {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return new Color32(0x60, 0x60, 0x60, 0xFF);
            }
        }

        public static Color mintGreen {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return new Color32(0x1D, 0xD1, 0xA1, 0xFF);
            }
        }

        public static Color darkMintGreen {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return new Color32(0x10, 0xAC, 0x84, 0xFF);
            }
        }

        public static Color lightRed {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return new Color32(0xEE, 0x52, 0x53, 0xFF);
            }
        }

        public static Color blueVoilet {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return new Color32(0x66, 0x5F, 0xE2, 0xFF);
            }
        }

        public static Color redishBrown {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return new Color32(0x49, 0x10, 0x00, 0xFF);
            }
        }
    }
}
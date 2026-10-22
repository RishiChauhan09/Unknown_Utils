using TMPro;
using UnityEngine;
using Unknown.Manager;

public static class WorldTextUtils {

    private static ObjectPool<TextMeshPro> tmptextPool;
    private static bool isInitialized;

    private static void Initialize() {
        if(isInitialized)
            return;

        isInitialized = true;

        tmptextPool = new ObjectPool<TextMeshPro>(CreateText);
    }

    private static TextMeshPro CreateText() {
        GameObject go = new GameObject("World Text");
        TextMeshPro text = go.AddComponent<TextMeshPro>();
        go.AddComponent<BillboardCamera>();

        return text;
    }

    public static TextMeshPro ShowText(string text, Vector3 position,float fontSize = .75f) {
        Initialize();

        TextMeshPro textObject = tmptextPool.Get();
        textObject.text = text;
        textObject.fontSize = .75f;

        textObject.alignment = TextAlignmentOptions.Center;
        textObject.alignment = TextAlignmentOptions.Midline;
        textObject.textWrappingMode = TextWrappingModes.NoWrap;

        textObject.transform.position = position;

        return textObject;
    }

    public static void Release(TextMeshPro textMeshPro) {
        tmptextPool.Release(textMeshPro);
    }

    public static void ReleaseAll() {
        tmptextPool.ReleaseAll();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Reset() {
        isInitialized = false;
        tmptextPool = null;
    }

}
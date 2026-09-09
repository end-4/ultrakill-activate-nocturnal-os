using System.Reflection;
using System.IO;
using System.Linq;
using BepInEx;
using BepInEx.Logging;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ActivateNocturnalOS;

[BepInPlugin("com.github.end-4.activateNocturnalOS", "ActivateNocturnalOS", PluginVersion)]
[BepInDependency("com.github.end-4.thornClient")]
public class Core : BaseUnityPlugin {
    // Logger
    internal static ManualLogSource? Log;

    // Meta
    public static string workingPath = Assembly.GetExecutingAssembly().Location;
    public static string workingDir = Path.GetDirectoryName(workingPath);
    public const string PluginGUID = "com.github.end-4.activateNocturnalOS";
    public const string PluginName = "ActivateNocturnalOS";
    public const string PluginVersion = "2.0.0";

    // Nag stuff
    private static GameObject? nagCanvas;
    private static TMP_FontAsset? ukFont;

    private void Awake() {
        Log = Logger;

        SceneManager.sceneLoaded += OnSceneLoaded;
        UserHints.Initialize();

        Log.LogInfo("ActivateNocturnalOS loaded!");
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (ukFont == null) {
            ukFont = Resources.FindObjectsOfTypeAll<TMP_FontAsset>()
                .FirstOrDefault(f => f.name.Contains("VCR"));
        }

        CreateLicenseNag();
        NocturnalLicense.Activated.OnChanged += UpdateNagText;
        NocturnalLicense.NagTitle.OnChanged += UpdateNagText;
        NocturnalLicense.NagTitleFontSize.OnChanged += UpdateNagText;
        NocturnalLicense.NagTitleColor.OnChanged += UpdateNagText;
        NocturnalLicense.NagTitleColor.OnEffectiveValueChanged += UpdateNagText;
        NocturnalLicense.NagBodyText.OnChanged += UpdateNagText;
        NocturnalLicense.NagBodyFontSize.OnChanged += UpdateNagText;
        NocturnalLicense.NagOpacity.OnChanged += UpdateNagText;
        NocturnalLicense.NagBodyColor.OnChanged += UpdateNagText;
        NocturnalLicense.NagBodyColor.OnEffectiveValueChanged += UpdateNagText;
    }

    private static void CreateLicenseNag() {
        if (nagCanvas != null) return;

        nagCanvas = new GameObject("NocturnalOS_NagCanvas");
        DontDestroyOnLoad(nagCanvas);

        UnityEngine.Canvas canvas = nagCanvas.AddComponent<UnityEngine.Canvas>();
        canvas.renderMode = UnityEngine.RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999;

        CanvasScaler scaler = nagCanvas.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(Screen.width, Screen.height);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;

        GameObject container = new GameObject("NagContainer");
        container.transform.SetParent(nagCanvas.transform);

        RectTransform rect = container.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(1, 0);
        rect.anchorMax = new Vector2(1, 0);
        rect.pivot = new Vector2(1, 0);
        rect.anchoredPosition = new Vector2(-45, 45);
        rect.sizeDelta = new Vector2(410, 150);

        VerticalLayoutGroup layout = container.AddComponent<VerticalLayoutGroup>();
        layout.childAlignment = UnityEngine.TextAnchor.LowerLeft;
        layout.childControlHeight = true;
        layout.childControlWidth = true;
        layout.childForceExpandHeight = false;
        layout.childForceExpandWidth = false;
        layout.spacing = 5;

        CreateText(container.transform, "Activate Nocturnal OS", NocturnalLicense.NagTitleFontSize.Value,
            FontStyles.Normal, NocturnalLicense.NagTitleColor.Value.GetCurrentColor());
        CreateText(container.transform, "Go to Settings to activate Nocturnal OS",
            NocturnalLicense.NagBodyFontSize.Value, FontStyles.Normal,
            NocturnalLicense.NagBodyColor.Value.GetCurrentColor());
        
        UpdateNagText();
    }

    internal static void UpdateNagText() {
        if (nagCanvas == null) return;

        // Hide the nag if the OS is "activated"
        nagCanvas.SetActive(!NocturnalLicense.Activated.Value);

        var texts = nagCanvas.GetComponentsInChildren<TextMeshProUGUI>(true);
        if (texts.Length >= 2) {
            if (texts[0].text != NocturnalLicense.NagTitle.Value) texts[0].text = NocturnalLicense.NagTitle.Value;
            if (!Mathf.Approximately(texts[0].fontSize, NocturnalLicense.NagTitleFontSize.Value)) texts[0].fontSize = NocturnalLicense.NagTitleFontSize.Value;
            texts[0].color = NocturnalLicense.NagTitleColor.Value.GetCurrentColor();
            if (texts[1].text != NocturnalLicense.NagBodyText.Value) texts[1].text = NocturnalLicense.NagBodyText.Value;
            if (!Mathf.Approximately(texts[1].fontSize, NocturnalLicense.NagBodyFontSize.Value)) texts[1].fontSize = NocturnalLicense.NagBodyFontSize.Value;
            texts[1].color = NocturnalLicense.NagBodyColor.Value.GetCurrentColor();
        }

        foreach (var text in texts) {
            Color c = text.color;
            c.a = NocturnalLicense.NagOpacity.Value;
            text.color = c;
        }
    }

    private static void CreateText(Transform parent, string content, float fontSize, FontStyles style, Color color) {
        GameObject textObj = new GameObject("NagText_" + (content.Length >= 5 ? content.Substring(0, 5) : content));
        textObj.transform.SetParent(parent);

        TextMeshProUGUI text = textObj.AddComponent<TextMeshProUGUI>();
        text.text = content;
        text.fontSize = fontSize;
        text.fontStyle = style;
        text.color = color;
        text.alignment = TextAlignmentOptions.Left;

        if (ukFont != null) {
            text.font = ukFont;
        }
    }

    private void Update() {
        RandomNotifications.OnUpdate();
    }
}


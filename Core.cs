using System.Reflection;
using System.IO;
using System.Linq;
using BepInEx;
using BepInEx.Logging;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PluginConfig.API;
using PluginConfig.API.Fields;
using PluginConfig.API.Decorators;

namespace ActivateNocturnalOS {
    [BepInPlugin("com.github.end-4.activateNocturnalOS", "ActivateNocturnalOS", "1.0.0")]
    public class Core : BaseUnityPlugin {
        // Logger
        internal static ManualLogSource? Log;

        // Plugin config
        public static string workingPath = Assembly.GetExecutingAssembly().Location;
        public static string workingDir = Path.GetDirectoryName(workingPath);
        public const string PluginGUID = "com.github.end-4.activateNocturnalOS";
        public static PluginConfigurator? config = null;
        public static BoolField? activated;
        public static StringField? nagTitle;
        public static StringField? nagBodyText;
        public static FloatField? nagOpacity;

        // Nag stuff
        private GameObject? nagCanvas;
        private TMP_FontAsset? ukFont;

        private void Awake() {
            Log = Logger;

            CreateSettings();
            SceneManager.sceneLoaded += OnSceneLoaded;

            Log.LogInfo("ActivateNocturnalOS loaded!");
        }

        private void CreateSettings() {
            config = PluginConfigurator.Create("Nocturnal OS License", Core.PluginGUID);
            string iconPath = Path.Combine(workingDir, "icon.png");
            if (File.Exists(iconPath)) config.SetIconWithURL(iconPath);

            new ConfigHeader(config.rootPanel, "-- ACTIVATION SETTINGS --", 24);
            activated = new BoolField(config.rootPanel, "Activate Nocturnal OS", "activatedNocturnal", false);
            nagTitle = new StringField(config.rootPanel, "Title text", "titleText", "Activate Nocturnal OS");
            nagBodyText = new StringField(config.rootPanel, "Body text", "bodyText", "Go to Settings to activate Nocturnal OS");
            nagOpacity = new FloatField(config.rootPanel, "Opacity", "nagOpacity", 0.35f, 0f, 1f);

            activated.postValueChangeEvent += (bool b) => {
                UpdateNagText();
            };
            nagTitle.postValueChangeEvent += (string s) => {
                UpdateNagText();
            };
            nagBodyText.postValueChangeEvent += (string s) => {
                UpdateNagText();
            };
            nagOpacity.postValueChangeEvent += (float f) => {
                UpdateNagText();
            };
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            if (ukFont == null) {
                ukFont = Resources.FindObjectsOfTypeAll<TMP_FontAsset>()
                    .FirstOrDefault(f => f.name.Contains("VCR"));
            }

            CreateLicenseNag();
        }

        private void CreateLicenseNag() {
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

            CreateText(container.transform, "Activate Nocturnal OS", 28, FontStyles.Normal, new Color(1, 1, 1, 0.35f));
            CreateText(container.transform, "Go to Settings to activate Nocturnal OS", 18, FontStyles.Normal,
                new Color(1, 1, 1, 0.5f));

            UpdateNagText();
        }

        private void UpdateNagText() {
            if (nagCanvas == null || activated == null || nagTitle == null || nagBodyText == null || nagOpacity == null) return;

            // Hide the nag if the OS is "activated"
            nagCanvas.SetActive(!activated.value);

            var texts = nagCanvas.GetComponentsInChildren<TextMeshProUGUI>(true);
            if (texts.Length >= 2) {
                texts[0].text = nagTitle.value;
                texts[1].text = nagBodyText.value;
            }

            foreach (var text in texts) {
                Color c = text.color;
                c.a = nagOpacity.value;
                text.color = c;
            }
        }

        private void CreateText(Transform parent, string content, float fontSize, FontStyles style, Color color) {
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
    }
}

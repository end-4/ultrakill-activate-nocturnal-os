using System.Reflection;
using System.IO;
using System.Linq;
using BepInEx;
using BepInEx.Logging;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Notiffy.API;
using System;
using Notiffy.Utils;

namespace ActivateNocturnalOS {
    [BepInPlugin("com.github.end-4.activateNocturnalOS", "ActivateNocturnalOS", "1.1.0")]
    public class Core : BaseUnityPlugin {
        // Logger
        internal static ManualLogSource? Log;

        // Plugin config
        public static string workingPath = Assembly.GetExecutingAssembly().Location;
        public static string workingDir = Path.GetDirectoryName(workingPath);
        public const string PluginGUID = "com.github.end-4.activateNocturnalOS";
        public const string PluginName = "ActivateNocturnalOS";
        public const string PluginVersion = "1.1.0";

        // Notifications
        private float? lastNotification;
        private static Notification[] notifs = [
            new Notification {
                Summary = "Backup to FourDrive",
                Body = "Keep your files safe from attacks",
                NotificationIcon = Img2Sprite.LoadNewSprite(Path.Combine(workingDir, "assets/fourdrive.png")),
                Actions = ["yes", "Set up", "no", "Maybe later"],
            },
            new Notification {
                Summary = "Get a genuine NocturnalOS license",
                Body = "You may be a Fraud",
                NotificationIcon = Img2Sprite.LoadNewSprite(Path.Combine(workingDir, "icon.png")),
            },
            new Notification {
                Summary = "Try Smilesoft365",
                Body = "Give your productivity a boost",
                NotificationIcon = Img2Sprite.LoadNewSprite(Path.Combine(workingDir, "assets/smilesoft365.png")),
                Actions = ["yes", "30 days free", "no", "I'll pass"],
            },
        ];

        // Nag stuff
        private static GameObject? nagCanvas;
        private static TMP_FontAsset? ukFont;

        private void Awake() {
            Log = Logger;

            ConfigManager.Initialize();
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

            CreateText(container.transform, "Activate Nocturnal OS", 28, FontStyles.Normal, new Color(1, 1, 1, 0.35f));
            CreateText(container.transform, "Go to Settings to activate Nocturnal OS", 18, FontStyles.Normal,
                new Color(1, 1, 1, 0.5f));

            UpdateNagText();
        }

        internal static void UpdateNagText() {
            if (nagCanvas == null || ConfigManager.activated == null || ConfigManager.nagTitle == null || ConfigManager.nagBodyText == null || ConfigManager.nagOpacity == null) return;

            // Hide the nag if the OS is "activated"
            nagCanvas.SetActive(!ConfigManager.activated.value);

            var texts = nagCanvas.GetComponentsInChildren<TextMeshProUGUI>(true);
            if (texts.Length >= 2) {
                texts[0].text = ConfigManager.nagTitle.value;
                texts[1].text = ConfigManager.nagBodyText.value;
            }

            foreach (var text in texts) {
                Color c = text.color;
                c.a = ConfigManager.nagOpacity.value;
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
            if (lastNotification == null) lastNotification = Time.time;
            else if ((ConfigManager.sendNotifications?.value ?? false) && Time.time - lastNotification >= ConfigManager.notificationInterval?.value) {
                lastNotification = Time.time;
                Notification selectedNotif = notifs[UnityEngine.Random.Range(0, notifs.Length)];
                NotificationSystem.Notify(selectedNotif);
            }
        }
    }
}

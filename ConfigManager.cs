using PluginConfig.API;
using PluginConfig.API.Fields;
using PluginConfig.API.Decorators;
using System.IO;
using System;

namespace ActivateNocturnalOS {
    public class ConfigManager {
        public static PluginConfigurator config;
        public static BoolField activated;
        public static StringField nagTitle;
        public static StringField nagBodyText;
        public static FloatField nagOpacity;
        public static BoolField sendNotifications;
        public static IntField notificationInterval;

        public static StringField lastVersion;

        public static void Initialize() { }

        static ConfigManager() {
            config = PluginConfigurator.Create("Nocturnal OS License", Core.PluginGUID);
            string iconPath = Path.Combine(Core.workingDir, "icon.png");
            if (File.Exists(iconPath)) config.SetIconWithURL(iconPath);

            new ConfigHeader(config.rootPanel, "", 10);
            new ConfigHeader(config.rootPanel, "-- ACTIVATION SETTINGS --", 24);
            activated = new BoolField(config.rootPanel, "Activate Nocturnal OS", "activatedNocturnal", false);
            nagTitle = new StringField(config.rootPanel, "Title text", "titleText", "Activate Nocturnal OS");
            nagBodyText = new StringField(config.rootPanel, "Body text", "bodyText", "Go to Settings to activate Nocturnal OS");
            nagOpacity = new FloatField(config.rootPanel, "Opacity", "nagOpacity", 0.35f, 0f, 1f);

            activated.postValueChangeEvent += (bool _) => {
                Core.UpdateNagText();
            };
            nagTitle.postValueChangeEvent += (string _) => {
                Core.UpdateNagText();
            };
            nagBodyText.postValueChangeEvent += (string _) => {
                Core.UpdateNagText();
            };
            nagOpacity.postValueChangeEvent += (float _) => {
                Core.UpdateNagText();
            };

            new ConfigHeader(config.rootPanel, "-- NOTIFICATIONS --", 24);
            sendNotifications = new BoolField(config.rootPanel, "Send notifications", "sendNotifications", false);
            notificationInterval = new IntField(config.rootPanel, "Notification interval (seconds)", "notificationInterval", 2700);

            // Misc
            lastVersion = new StringField(config.rootPanel, "Last version", "lastVersion", "1.0.0");
            lastVersion.hidden = true;
        }
    }
}

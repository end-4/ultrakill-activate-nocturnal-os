using System.IO;
using Notiffy.Utils;
using ThornClient.Core;
using ThornClient.Core.DataTypes;
using ThornClient.Core.ConfigurableElements;
using UnityEngine;

namespace ActivateNocturnalOS;

public class NocturnalLicense : Module {
    public static Setting<bool> Activated;
    public static Setting<string> NagTitle;
    public static Setting<float> NagTitleFontSize;
    public static Setting<EnhancedColor> NagTitleColor;
    public static Setting<string> NagBodyText;
    public static Setting<float> NagBodyFontSize;
    public static Setting<EnhancedColor> NagBodyColor;
    public static Setting<float> NagOpacity;
    public static Setting<bool> SendNotifications;
    public static Setting<int> NotificationInterval;
    public static Setting<string> LastVersion;

    public override string[] Tags => ["legal", "genuine", "smilesoft"];
    public override Sprite Icon => Img2Sprite.LoadNewSprite(Path.Combine(Core.workingDir, "icon.png"));

    public NocturnalLicense() : base(
        "activateNocturnalOS.activation",
        "Nocturnal OS License",
        "Get a genuine license from Smilesoft.",
        ModuleCategory.Misc, hasToggling: false
    ) {
        CreateHeader("status", "Activation status");
        Activated = CreateSetting("activated", "Activate NocturnalOS license", "Click to activate the license", false);
        CreateHeader("reminderSettings", "Reminder settings", "Note: There's a draggable version in the HUD tab");
        // nagTitle = new StringField(config.rootPanel, "Title text", "titleText", "Activate Nocturnal OS");
        // nagBodyText = new StringField(config.rootPanel, "Body text", "bodyText", "Go to Settings to activate Nocturnal OS");
        // nagOpacity = new FloatField(config.rootPanel, "Opacity", "nagOpacity", 0.35f, 0f, 1f);
        NagTitle = CreateSetting("nagTitle", "Title text", "Title text for the reminder", "Activate Nocturnal OS");
        NagTitleFontSize = CreateSetting("nagTitleFontSize", "Title font size", "Font size of the title", 28f);
        NagTitleColor = CreateSetting("nagTitleColor", "Title color", "Color of the title text", new EnhancedColor(Color.white));
        NagBodyText = CreateSetting("nagBodyText", "Body text", "Body text for the reminder", "Go to Thorn menu to activate Nocturnal OS");
        NagBodyFontSize = CreateSetting("nagBodyFontSize", "Body text font size", "Font size of the body text", 18f);
        NagBodyColor = CreateSetting("nagBodyColor", "Body color", "Color of the body text", new EnhancedColor(Color.white));
        NagOpacity = CreateSetting("nagOpacity", "Opacity", "Opacity for the reminder", 0.4f);
        NagOpacity.Hints = InterfaceHints.RangeHint();
        CreateHeader("notifications", "Notifications", "Receive news and updates from Smilesoft");
        SendNotifications = CreateSetting("notificationsEnabled", "Send notifications", "Whether to send random notifications", true);
        NotificationInterval = CreateSetting("notificationInterval", "Notification interval (secs)", "Interval between notifications in seconds", 2700);
        LastVersion = CreateSetting("lastVersion", "Last version", "The last version of the application", "1.0.0");
        LastVersion.Hints = new InterfaceHints {
            Hidden = true
        };
    }
}

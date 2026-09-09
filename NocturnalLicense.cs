using ThornClient.Core;
using ThornClient.Core.ConfigurableElements;

namespace ActivateNocturnalOS;

public class NocturnalLicense : Module {
    public static Setting<bool> Activated;
    public static Setting<string> NagTitle;
    public static Setting<string> NagBodyText;
    public static Setting<float> NagOpacity;
    public static Setting<bool> SendNotifications;
    public static Setting<int> NotificationInterval;
    public static Setting<string> LastVersion;

    public NocturnalLicense() : base(
        "activateNocturnalOS.activation",
        "NocturnalOS License",
        "Get a genuine license from Smilesoft.",
        ModuleCategory.Misc, hasToggling: false
    ) {
        CreateHeader("status", "Activation status");
        Activated = CreateSetting("activated", "Activate NocturnalOS license", "Click to activate the license", false);
        CreateHeader("reminderSettings", "Reminder settings");
        // nagTitle = new StringField(config.rootPanel, "Title text", "titleText", "Activate Nocturnal OS");
        // nagBodyText = new StringField(config.rootPanel, "Body text", "bodyText", "Go to Settings to activate Nocturnal OS");
        // nagOpacity = new FloatField(config.rootPanel, "Opacity", "nagOpacity", 0.35f, 0f, 1f);
        NagTitle = CreateSetting("nagTitle", "Title text", "Title text for the reminder", "Activate Nocturnal OS");
        NagBodyText = CreateSetting("nagBodyText", "Body text", "Body text for the reminder", "Go to Thorn menu to activate Nocturnal OS");
        NagOpacity = CreateSetting("nagOpacity", "Nag opacity", "Opacity for the nag reminder", 0.35f);
        NagOpacity.Hints = InterfaceHints.RangeHint();
        LastVersion = CreateSetting("lastVersion", "Last version", "The last version of the application", "1.0.0");
    }
}

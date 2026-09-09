using System.IO;
using Notiffy.Utils;
using ThornClient.Core.ConfigurableElements;
using UnityEngine;
using ThornClient.HUD;

namespace ActivateNocturnalOS;

// Subclassing TextHudModule...
public class DraggableReminder : TextHudModule {
    public override string[] Tags => ["legal", "genuine", "smilesoft"];
    public override Sprite Icon => Img2Sprite.LoadNewSprite(Path.Combine(Core.workingDir, "icon.png"));

    public DraggableReminder() : base("activateNocturnalOS.draggableReminder", "Nocturnal OS License Reminder",
        "A friendly reminder that you should activate your license") {
        CreateHeader("note", "Note", "Appearance settings are adjusted in the main Nocturnal OS License module");
        this.ForegroundColor.Hints = new InterfaceHints {
            Hidden = true
        };
    }

    protected override void OnHudModuleEnable() {
        NocturnalLicense.Activated.OnChanged += UpdateText;
        NocturnalLicense.NagTitle.OnChanged += UpdateText;
        NocturnalLicense.NagTitleFontSize.OnChanged += UpdateText;
        NocturnalLicense.NagTitleColor.OnChanged += UpdateText;
        NocturnalLicense.NagTitleColor.OnEffectiveValueChanged += UpdateText;
        NocturnalLicense.NagBodyText.OnChanged += UpdateText;
        NocturnalLicense.NagBodyFontSize.OnChanged += UpdateText;
        NocturnalLicense.NagOpacity.OnChanged += UpdateText;
        NocturnalLicense.NagBodyColor.OnChanged += UpdateText;
        NocturnalLicense.NagBodyColor.OnEffectiveValueChanged += UpdateText;
        UpdateText();
    }

    protected override void OnHudModuleDisable() {
        NocturnalLicense.Activated.OnChanged -= UpdateText;
        NocturnalLicense.NagTitle.OnChanged -= UpdateText;
        NocturnalLicense.NagTitleFontSize.OnChanged -= UpdateText;
        NocturnalLicense.NagTitleColor.OnChanged -= UpdateText;
        NocturnalLicense.NagTitleColor.OnEffectiveValueChanged -= UpdateText;
        NocturnalLicense.NagBodyText.OnChanged -= UpdateText;
        NocturnalLicense.NagBodyFontSize.OnChanged -= UpdateText;
        NocturnalLicense.NagOpacity.OnChanged -= UpdateText;
        NocturnalLicense.NagBodyColor.OnChanged -= UpdateText;
        NocturnalLicense.NagBodyColor.OnEffectiveValueChanged -= UpdateText;
    }

    private void UpdateText() {
        var rawTitle = NocturnalLicense.NagTitle.Value;
        var rawBody = NocturnalLicense.NagBodyText.Value;
        var titleColor = NocturnalLicense.NagTitleColor.Value.GetCurrentColor();
        var bodyColor = NocturnalLicense.NagBodyColor.Value.GetCurrentColor();
        var opacity = NocturnalLicense.NagOpacity.Value;
        var opacityAppliedTitleColor = new Color(titleColor.r, titleColor.g, titleColor.b, opacity);
        var opacityAppliedBodyColor = new Color(bodyColor.r, bodyColor.g, bodyColor.b, opacity);
        var titleColorString = "#" + ColorUtility.ToHtmlStringRGBA(opacityAppliedTitleColor);
        var bodyColorString = "#" + ColorUtility.ToHtmlStringRGBA(opacityAppliedBodyColor);
        var formattedTitle = $"<size={NocturnalLicense.NagTitleFontSize.Value}><color={titleColorString}>{rawTitle}</color></size>";
        var formattedBody = $"<size={NocturnalLicense.NagBodyFontSize.Value}><color={bodyColorString}>{rawBody}</color></size>";
        var spacing = $"<size=5> </size>";
        Text = $"{formattedTitle}\n{spacing}\n{formattedBody}";
    }
}

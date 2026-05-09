using System;
using System.IO;
using Notiffy.API;

namespace ActivateNocturnalOS;

public static class UserHints {

    public static void IssueUpdateNoticeIfNecessary() {
        Version lastVersion = new Version(ConfigManager.lastVersion.value);
        Version currVersion = new Version(Core.PluginVersion);
        if (currVersion.CompareTo(lastVersion) == 1) {
            ConfigManager.lastVersion.value = Core.PluginVersion;
            NotificationSystem.NotifySend("<color=#3a93e9>Activate Nocturnal OS</color> updated",
                "<b>1.1.0:</b> Introduced random funny ads. This is off by default and you can enable it manually in Plugin Config", 
                expireTime: 10000, iconFilePath: Path.Combine(Core.workingDir, "icon.png"));
        }
    }

    public static void Initialize() { }

    static UserHints() {
        NotificationSystem.ReadyForScene += IssueUpdateNoticeIfNecessary;
    }
}

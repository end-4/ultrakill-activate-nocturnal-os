using System;
using System.IO;
using Notiffy.API;
using ThornClient.System;
using UnityEngine.SceneManagement;

namespace ActivateNocturnalOS;

public static class UserHints {

    public static void IssueUpdateNoticeIfNecessary() {
        Version lastVersion = new Version(NocturnalLicense.LastVersion.Value);
        Version currVersion = new Version("2.0.0");
        if (currVersion.CompareTo(lastVersion) == 1) {
            NocturnalLicense.LastVersion.Value = Core.PluginVersion;
            NotificationSystem.NotifySend("<color=#3a93e9>Nocturnal OS</color> notice",
                $"Your license has expired. Go to the Thorn menu ({ThornModule.Instance!.OpenClickGUI.Value.ToString(true)}) to activate a new license.",
                expireTime: 10000, iconFilePath: Path.Combine(Core.workingDir, "icon.png"));
        }
    }

    public static void IssueUpdateNoticeIfNecessary(Scene _, LoadSceneMode __) {
        IssueUpdateNoticeIfNecessary();
    }

    public static void Initialize() { }

    static UserHints() {
        SceneManager.sceneLoaded += IssueUpdateNoticeIfNecessary;
    }
}

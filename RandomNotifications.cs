using System;
using Notiffy.API;
using Notiffy.Utils;
using UnityEngine;

namespace ActivateNocturnalOS;

public static class RandomNotifications {
    private static float? lastNotification;

    private static Notification[] notifs = [
        new Notification {
            Summary = "Backup to FourDrive",
            Body = "Keep your files safe from attacks",
            NotificationIcon =
                Img2Sprite.LoadNewSprite(System.IO.Path.Combine(Core.workingDir, "assets/fourdrive.png")),
            Actions = ["yes", "Set up", "no", "Maybe later"],
        },
        new Notification {
            Summary = "Get a genuine NocturnalOS license",
            Body = "You may be a Fraud",
            NotificationIcon = Img2Sprite.LoadNewSprite(System.IO.Path.Combine(Core.workingDir, "icon.png")),
        },
        new Notification {
            Summary = "Try Smilesoft365",
            Body =
                "Work smarter across Smilesoft365 with Coroutine. AI built into Smilesoft365 helps you create, collaborate, and work across documents, presentations, and data.",
            NotificationIcon =
                Img2Sprite.LoadNewSprite(System.IO.Path.Combine(Core.workingDir, "assets/smilesoft365.png")),
            Actions = ["yes", "30 days free", "no", "I'll pass"],
        },
        new Notification {
            Summary = "Smilesoft Incel",
            Body = "View, filter and analyze at scale. Become a freak in the sheets.",
            NotificationIcon =
                Img2Sprite.LoadNewSprite(System.IO.Path.Combine(Core.workingDir, "assets/incel.png")),
            Actions = ["yes", "Try it"],
        },
        new Notification {
            Summary = "Smilesoft Letter",
            Body = "I like your funny words magic man",
            NotificationIcon =
                Img2Sprite.LoadNewSprite(System.IO.Path.Combine(Core.workingDir, "assets/letter.png")),
            Actions = ["yes", "Try it"],
        },
        new Notification {
            Summary = "Smilesoft PowerFinger",
            Body = "Ready to impress, when you are.",
            NotificationIcon =
                Img2Sprite.LoadNewSprite(System.IO.Path.Combine(Core.workingDir, "assets/powerfinger.png")),
            Actions = ["yes", "Try it"],
        },
        new Notification {
            Summary = "Smilesoft FourNote",
            Body = "All your notes, in four places.",
            NotificationIcon =
                Img2Sprite.LoadNewSprite(System.IO.Path.Combine(Core.workingDir, "assets/fournote.png")),
        },
        new Notification {
            Summary = "Underlook: New message",
            Body = "v2@gmale.com:\nFUCKING NIKKON",
            NotificationIcon =
                Img2Sprite.LoadNewSprite(System.IO.Path.Combine(Core.workingDir, "assets/underlook.png")),
        },
        new Notification {
            Summary = "Smilesoft Gangs",
            Body = "Stay in the know. Turn on desktop notifications.",
            NotificationIcon =
                Img2Sprite.LoadNewSprite(System.IO.Path.Combine(Core.workingDir, "assets/gangs.png")),
            Actions = ["yes", "Turn on", "no", "Not now"],
        },
        new Notification {
            Summary = "Get YBox Game Pass",
            Body = "Play next-level games on your NocturnalOS system with YBox Game Pass.",
            NotificationIcon = Img2Sprite.LoadNewSprite(System.IO.Path.Combine(Core.workingDir, "assets/ybox.png")),
            Actions = ["yes", "Join now", "no", "Dismiss"],
        },
        new Notification {
            Summary = "Try Smilesoft Goon",
            Body = "Built on the same technology as Chroma, with the added trust of Smilesoft.",
            NotificationIcon = Img2Sprite.LoadNewSprite(System.IO.Path.Combine(Core.workingDir, "assets/goon.png")),
            Actions = ["yes", "Take me there", "no", "No thanks"],
        },
    ];

    public static void OnUpdate() {
        if (lastNotification == null) lastNotification = Time.time;
        else if ((NocturnalLicense.SendNotifications.Value) &&
                 Time.time - lastNotification >= NocturnalLicense.NotificationInterval.Value) {
            lastNotification = Time.time;
            Notification selectedNotif = notifs[UnityEngine.Random.Range(0, notifs.Length)];
            NotificationSystem.Notify(selectedNotif);
        }
    }
}

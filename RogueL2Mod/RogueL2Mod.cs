using BepInEx;
using System;
using System.Security;
using System.Security.Permissions;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace RogueL2Mod {

    [BepInPlugin("TheTimeSweeper.RogueL", "RogueL2Mod", "0.1.0")]
    public class RogueL2Mod : BaseUnityPlugin {

        void Awake() {

        }
    }
}

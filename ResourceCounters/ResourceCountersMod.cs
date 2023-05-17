using BepInEx;
using System;
using System.Security;
using System.Security.Permissions;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace ResourceCountersMod {

    [BepInPlugin("TheTimeSweeper.ResourceCounters", "ResourceCounters", "0.1.0")]
    public class ResourceCountersMod : BaseUnityPlugin {

        public ResourceReporter goldChangedReporter = new ResourceReporter();
        public ResourceReporter oreChangedReporter = new ResourceReporter();
        public ResourceReporter aetherchangedReporter = new ResourceReporter();
        public ResourceReporter soulsChangedReporter = new ResourceReporter();

        void Awake() {

            On.MinimapHUDController.Awake += MinimapHUDController_Awake;

            On.MinimapHUDController.OnGoldChanged += MinimapHUDController_OnGoldChanged;
            On.MinimapHUDController.OnEquipmentOreChanged += MinimapHUDController_OnEquipmentOreChanged;
            On.MinimapHUDController.OnRuneOreChanged += MinimapHUDController_OnRuneOreChanged;
            On.MinimapHUDController.OnSoulChanged += MinimapHUDController_OnSoulChanged;
        }

        private void MinimapHUDController_Awake(On.MinimapHUDController.orig_Awake orig, MinimapHUDController self) {
            orig(self);

            self.m_goldText.transform.parent.gameObject.AddComponent<GoldCountupController>()
                .init(self.m_goldText, goldChangedReporter, self.m_goldModTextRectTransform);

            self.m_equipmentOreText.transform.parent.gameObject.AddComponent<ResourceCountupController>()
                .init(self.m_equipmentOreText, oreChangedReporter);

            self.m_runeOreText.transform.parent.gameObject.AddComponent<ResourceCountupController>()
                .init(self.m_runeOreText, aetherchangedReporter);

            self.m_soulText.transform.parent.gameObject.AddComponent<SoulCountupController>()
                .init(self.m_soulText, soulsChangedReporter);

        }

        private void MinimapHUDController_OnGoldChanged(On.MinimapHUDController.orig_OnGoldChanged orig, MinimapHUDController self, UnityEngine.MonoBehaviour sender, EventArgs args) {

            orig(self, sender, args);
            goldChangedReporter.Invoke(SaveManager.PlayerSaveData.GoldCollected);
        }

        private void MinimapHUDController_OnEquipmentOreChanged(On.MinimapHUDController.orig_OnEquipmentOreChanged orig, MinimapHUDController self, UnityEngine.MonoBehaviour sender, EventArgs args) {

            orig(self, sender, args);
            oreChangedReporter.Invoke(SaveManager.PlayerSaveData.EquipmentOreCollected);
        }

        private void MinimapHUDController_OnRuneOreChanged(On.MinimapHUDController.orig_OnRuneOreChanged orig, MinimapHUDController self, UnityEngine.MonoBehaviour sender, EventArgs args) {

            orig(self, sender, args);
            aetherchangedReporter.Invoke(SaveManager.PlayerSaveData.RuneOreCollected);
        }

        private void MinimapHUDController_OnSoulChanged(On.MinimapHUDController.orig_OnSoulChanged orig, MinimapHUDController self, UnityEngine.MonoBehaviour sender, EventArgs args) {

            //code copied from dnspy. attempting to fix souls not counting up properly like other currencies because of the "fake souls" system
            int num = Souls_EV.GetTotalSoulsCollected(SaveManager.PlayerSaveData.GameModeType, true) - SoulDrop.FakeSoulCounter_STATIC;
            bool flag = SaveManager.PlayerSaveData.InHubTown && (!self.m_soulCanvasGroup.gameObject.activeSelf || self.m_soulCanvasGroup.alpha <= 0f);
            bool flag2 = self.m_previousSoulAmount == num && !flag;

            orig(self, sender, args);

            if (flag2)
                return;
            soulsChangedReporter?.Invoke(self.m_previousSoulAmount);
        }
    }
}

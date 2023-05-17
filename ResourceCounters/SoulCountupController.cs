using UnityEngine;

namespace ResourceCountersMod {
    public class SoulCountupController : ResourceCountupController {

        protected override void ResourceCountersMod_onPostResourceChanged(int amount) {
            Debug.LogWarning($"SoulsChanged | amount: {amount}, fakeSouls {SoulDrop.FakeSoulCounter_STATIC}, total {Souls_EV.GetTotalSoulsCollected(SaveManager.PlayerSaveData.GameModeType, true)}");

            base.ResourceCountersMod_onPostResourceChanged(amount);
        }
    }
}

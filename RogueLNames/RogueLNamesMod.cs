using BepInEx;
using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace RogueLNames {
    //*checks files' Date Modified*
    //8-20-2020
    //yeah this mod's almost two years old. only keeping it in the solution because it's special to me
    //don't look at this, look at the other one lol
    [Obsolete("the whole fuckin mod is obsolete")]
    [BepInPlugin("com.TheTimeSweeper.RogueL", "RogueL", "0.1.0")]
    public class RogueLNamesMod : BaseUnityPlugin {

        private string[] _maleNames2;
        private string[] _femaleNames2;

        void Awake() {

            config();
            On.LocalizationManager.LoadNames += LocalizationManager_LoadNames;
        }

        private void config() {

#pragma warning disable CS0618 // Type or member is obsolete. sorry I'm lazy

            string sectionString = "Fellers";

            string nameString = Config.Wrap(sectionString,
                                      "MaleNames",
                                      "Additional male names. separated by comma and space (', ')",
                                      "Stamper, Mick, Zach, Cory, Jeff, Chris, Niall, Bartholomew").Value;

            _maleNames2 = nameString.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

            nameString = Config.Wrap(sectionString,
                                      "FemaleNames",
                                      "Additional female names. separated by comma and space (', ')",
                                      "Nikki, Sabrina, Senpai, Malon, Rosa, Ernesta, ").Value;

            _femaleNames2 = nameString.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
        }

        private void LocalizationManager_LoadNames(On.LocalizationManager.orig_LoadNames orig, LocalizationManager self) {
            orig(self);
            addNames();
        }

        private void addNames() {

            logStringArray(LocalizationManager.FemaleNameArray, "namearrayF");
            logStringArray(LocalizationManager.MaleNameArray, "namearrayM");

            List<string> nameList = LocalizationManager.FemaleNameArray.ToList();
            for (int i = 0; i < _femaleNames2.Length; i++) {
                nameList.Add(_femaleNames2[i]);
            }

            SetInstanceField(typeof(LocalizationManager), LocalizationManager.Instance, "m_femaleNameArray", nameList.ToArray());

            nameList = LocalizationManager.MaleNameArray.ToList();
            for (int i = 0; i < _maleNames2.Length; i++) {
                nameList.Add(_maleNames2[i]);
            }

            SetInstanceField(typeof(LocalizationManager), LocalizationManager.Instance, "m_maleNameArray", nameList.ToArray());

            logStringArray(LocalizationManager.FemaleNameArray, "namearrayF");
            logStringArray(LocalizationManager.MaleNameArray, "namearrayM");
        }

        private void logStringArray(string[] arr, string arrayNameToLog) {

            string log = $"logging {arrayNameToLog}:";

            for (int i = 0; i < arr.Length; i++) {
                log += $"\n {arr[i]}";
            }

            Logger.LogWarning(log);
        }

        internal static void SetInstanceField(Type type, object instance, string fieldName, object valueToSet) {

            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

            FieldInfo field = type.GetField(fieldName, bindFlags);
            field.SetValue(instance, valueToSet);
        }

        internal static object GetInstanceField(Type type, object instance, string fieldName) {

            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

            FieldInfo field = type.GetField(fieldName, bindFlags);
            return field.GetValue(instance);
        }
    }
}

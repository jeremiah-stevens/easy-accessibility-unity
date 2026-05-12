using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace EasyAccessibility
{
    public static class AuditorRequirementKeys
    {
        #region WCAG

        public const string Source_WCAG = "WCAG";
        public const string WCAGJsonPath =
            "Packages/com.xx.easyaccessibility/Editor/Auditor/wcag.json";

        static JObject _WCAGJson;

        public static JObject WCAGJson
        {
            get
            {
                if (_WCAGJson == null)
                {
                    TextAsset json = AssetDatabase.LoadAssetAtPath<TextAsset>(WCAGJsonPath);
                    _WCAGJson = JObject.Parse(json.text);
                }
                return _WCAGJson;
            }
        }

        public static string GetTitle(int requirement, int guideline, int successCriteria)
        {
            var obj = WCAGJson["principles"][requirement - 1]["guidelines"][guideline - 1][
                "successcriteria"
            ][successCriteria - 1];
            return $"{(string)obj["num"]}: {(string)obj["handle"]}";
        }

        public static string GetDescription(int requirement, int guideline, int successCriteria)
        {
            var obj = WCAGJson["principles"][requirement - 1]["guidelines"][guideline - 1][
                "successcriteria"
            ][successCriteria - 1];
            //TODO: consider expanding on details in description
            return $"{(string)obj["title"]}";
        }

        public static string GetUrl(int requirement, int guideline, int successCriteria)
        {
            var obj = WCAGJson["principles"][requirement - 1]["guidelines"][guideline - 1][
                "successcriteria"
            ][successCriteria - 1];
            return $"https://www.w3.org/WAI/WCAG22/quickref/#{(string)obj["id"]}";
        }

        #endregion
    }
}

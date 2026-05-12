using EasyAccessibility;
using UnityEngine;

namespace EasyAccessibility
{
    [CreateAssetMenu(
        fileName = "AssetTranscriptSO",
        menuName = "Easy Accessibility/AssetTranscriptSO"
    )]
    public class AssetTranscriptSO : ScriptableObject, IAssetTranscript<UnityEngine.Object>
    {
        [SerializeField]
        Object asset;

        [SerializeField]
        TextAsset transcript;

        public UnityEngine.Object Asset
        {
            get => asset;
        }
        public TextAsset Transcript
        {
            get => transcript;
        }
    }
}

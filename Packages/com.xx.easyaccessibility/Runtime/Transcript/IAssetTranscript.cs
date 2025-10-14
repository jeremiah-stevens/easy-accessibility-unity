using UnityEngine;

namespace EasyAccessibility
{
    public interface IAssetTranscript<T> where T : UnityEngine.Object
    {
        public T Asset { get; }
        public TextAsset Transcript { get; }
    }
}
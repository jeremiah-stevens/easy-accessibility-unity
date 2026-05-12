using UnityEngine;

namespace EasyAccessibility
{
    /// <summary>
    /// Applies colorblind correction using the procedural approach (Built-In Render Pipeline).
    /// </summary>
    public class ColorblindCorrectionProceduralBuiltIn : ColorblindCorrectionBuiltIn
    {
        [Header("Override")]
        [SerializeField]
        bool overrideSettings;

        [SerializeField]
        AccessibilitySettings.ColorblindCorrectionMode mode;

        [SerializeField]
        [Range(0f, 1f)]
        float amount = 1;

        //state
        private Material m_renderMaterial;

        void Start()
        {
            var shader = Shader.Find(Constants.ColorblindCorrectionShaderProceduralBuiltInPath);
            if (shader == null)
            {
                Debug.LogError("Failed to find shader, cannot render colorblind correction.");
                m_renderMaterial = null;
                return;
            }
            m_renderMaterial = new Material(shader);
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (m_renderMaterial == null)
                return;

            m_renderMaterial.SetInt(
                "_Mode",
                (int)AccessibilitySettings.Instance.colorblindCorrectionMode
            );
            m_renderMaterial.SetFloat(
                "_Amount",
                AccessibilitySettings.Instance.colorblindCorrectionAmount
            );

            if (overrideSettings)
            {
                m_renderMaterial.SetInt("_Mode", (int)mode);
                m_renderMaterial.SetFloat("_Amount", amount);
            }

            Graphics.Blit(source, destination, m_renderMaterial);
        }
    }
}

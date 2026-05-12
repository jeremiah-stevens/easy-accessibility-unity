using UnityEngine;


namespace EasyAccessibility
{
    /// <summary>
    /// Applies colorblind correction using a look-up table approach (Built-In Render Pipeline).
    /// </summary>
    public class ColorblindCorrectionLUTBuiltIn : ColorblindCorrectionBuiltIn
    {
        [Header("Override")]
        [SerializeField] bool overrideSettings;
        [SerializeField] AccessibilitySettings.ColorblindCorrectionMode mode;
        [SerializeField][Range(0f, 1f)] float amount = 1;
        [SerializeField] Texture3D textureProtanopia;
        [SerializeField] Texture3D textureDeutranopia;
        [SerializeField] Texture3D textureTritanopia;

        //state
        private Material m_renderMaterial;
        



        void Start()
        {
            var shader = Shader.Find(Constants.ColorblindCorrectionShaderLUTBuiltInPath);
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
            if (m_renderMaterial == null) return;

            var activeMode = AccessibilitySettings.Instance.colorblindCorrectionMode;
            m_renderMaterial.SetInt("_Mode", (int)activeMode);
            m_renderMaterial.SetFloat("_Amount", AccessibilitySettings.Instance.colorblindCorrectionAmount);
            ColorblindMaterialUtils.SetLUT(m_renderMaterial, activeMode, textureProtanopia, textureDeutranopia, textureTritanopia);

            if (overrideSettings)
            {
                m_renderMaterial.SetInt("_Mode", (int)mode);
                m_renderMaterial.SetFloat("_Amount", amount);
                ColorblindMaterialUtils.SetLUT(m_renderMaterial, mode, textureProtanopia, textureDeutranopia, textureTritanopia);
            }

            Graphics.Blit(source, destination, m_renderMaterial);
        }
    }
}
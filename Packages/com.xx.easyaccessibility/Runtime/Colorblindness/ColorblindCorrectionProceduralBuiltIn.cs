using UnityEngine;


namespace EasyAccessibility
{
    public class ColorblindCorrectionProceduralBuiltIn : MonoBehaviour
    {
        public Shader shader = null;
        [SerializeField] ColorblindSettings.ColorblindMode mode;
        [SerializeField][Range(0f, 1f)] float amount = 1;
        private Material m_renderMaterial;




        void Start()
        {
            if (shader == null)
            {
                m_renderMaterial = null;
                return;
            }
            m_renderMaterial = new Material(shader);
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            m_renderMaterial.SetInt("_Mode", (int)mode);
            m_renderMaterial.SetFloat("_Amount", amount);

            Graphics.Blit(source, destination, m_renderMaterial);
        }
    }
}
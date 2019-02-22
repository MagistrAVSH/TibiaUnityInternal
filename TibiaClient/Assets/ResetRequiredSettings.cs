using UnityEngine;

namespace Utils
{
    public static class ResetRequiredSettings
    {
        public static void Reset()
        {
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
            QualitySettings.antiAliasing = 0;
            QualitySettings.pixelLightCount = 0;
            QualitySettings.shadowCascades = 0;
            QualitySettings.shadowDistance = 0;
            QualitySettings.shadows = ShadowQuality.Disable;
            QualitySettings.masterTextureLimit = 0;
        }
    }
}

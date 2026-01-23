using System;
using UnityEngine;

namespace io.github.azukimochi
{
    [Serializable]
    public class LightLimitChangerParameters
    {
        public bool IsDefaultUse  = false;
        public bool IsValueSave = false;
        public bool OverwriteDefaultLightMinMax = true;

        [Range(0, 1)]
        public float DefaultLightValue = 0.5f;

        [Range(0, 1)]
        public float DefaultMinLightValue = 0.0f;

        [Range(0, 1)]
        public float DefaultMaxLightValue = 1.0f;

        [Range(0, 10)]
        public float MaxLightValue = 1.0f;

        [Range(0, 10)]
        public float MinLightValue = 0.0f;

        [Obsolete] public int TargetShader = 0;

        public TargetShaders TargetShaders = TargetShaders.Everything;

        public bool AllowColorTempControl = false;
        public bool AllowSaturationControl = false;
        public bool AllowMonochromeControl = false;
        public bool AllowUnlitControl = false;
        public bool AllowEmissionControl = false;
        public bool AddResetButton = false;
        
        [Range(0,1)]
        public float InitialTempControlValue = 0.5f;

        [Range(0,1)]
        public float InitialSaturationControlValue = 0.5f;
        
        [Range(0,1)]
        public float InitialMonochromeControlValue = 0.0f;

        [Range(0,1)]
        public float DefaultMonochromeLightingValue = 0.0f;

        [Range(0,1)]
        public float DefaultMonochromeAdditiveLightingValue = 0.0f;
        
        [Range(0,1)]
        public float InitialUnlitControlValue = 0.0f;

        public bool IsSeparateLightControl = false;

        public bool IsGroupingAdditionalControls = false;

        // HSV Controls (lilToon)
        public bool AllowHueControl = false;
        public bool AllowValueControl = false;
        public bool AllowGammaControl = false;

        [Range(0,1)]
        public float InitialHueControlValue = 0.5f;

        [Range(0,1)]
        public float InitialValueControlValue = 0.5f;

        [Range(0,1)]
        public float InitialGammaControlValue = 0.5f;

        // lilToon Specific Controls
        public bool AllowLightDirectionControl = false;
        public bool AllowRimLightControl = false;
        public bool AllowBacklightControl = false;
        public bool AllowDistanceFadeControl = false;
        public bool AllowShadowEnvStrengthControl = false;
        public bool AllowVertexLightStrengthControl = false;

        [Range(0,1)]
        public float InitialRimLightStrengthValue = 0.5f;

        [Range(0,1)]
        public float InitialBacklightStrengthValue = 0.5f;

        [Range(0,1)]
        public float InitialDistanceFadeStrengthValue = 0.5f;

        [Range(0,1)]
        public float InitialShadowEnvStrengthValue = 0.5f;

        [Range(0,1)]
        public float InitialVertexLightStrengthValue = 0.5f;
    }
}

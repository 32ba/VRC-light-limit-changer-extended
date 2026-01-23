using System;

namespace io.github.azukimochi
{
    [Flags]
    public enum LightLimitControlType
    {
        Light = LightMin | LightMax, // 1 << 0,
        Saturation = 1 << 1,
        Unlit = 1 << 2,
        ColorTemperature = 1 << 3,
        LightMin = 1 << 4,
        LightMax = 1 << 5,
        Monochrome = 1 << 6,
        Emission = 1 << 7,
        LightDirection = 1 << 8,
        RimLight = 1 << 9,
        Backlight = 1 << 10,
        DistanceFade = 1 << 11,
        ShadowEnvStrength = 1 << 12,
        VertexLightStrength = 1 << 13,
        Hue = 1 << 14,
        Value = 1 << 15,
        Gamma = 1 << 16,
        RimLightBorder = 1 << 17,
        RimLightBlur = 1 << 18,
        RimLightFresnelPower = 1 << 19,
        BacklightBorder = 1 << 20,
        BacklightBlur = 1 << 21,
        BacklightDirectivity = 1 << 22,

        AdditionalControls = Saturation | Unlit | ColorTemperature | Monochrome | Emission,
        LilToonControls = LightDirection | RimLight | Backlight | DistanceFade | ShadowEnvStrength | VertexLightStrength | Hue | Value | Gamma,
        LilToonDetailedControls = RimLightBorder | RimLightBlur | RimLightFresnelPower | BacklightBorder | BacklightBlur | BacklightDirectivity,
    }
}

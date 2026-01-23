using System;
using System.Collections.Generic;
using gomoru.su;
using nadena.dev.modular_avatar.core;
using nadena.dev.ndmf;
using nadena.dev.ndmf.fluent;
using UnityEditor.Animations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace io.github.azukimochi
{
    internal static partial class Passes
    {
        public static void RunningPasses(Sequence sequence)
        {
            sequence
                .Run(CollectTargetRenderers).Then
                .Run(CloningMaterials).Then
                .Run(NormalizeMaterials).Then
                .Run(CheckMixedShadersPass.Instance).Then
                .Run(GenerateAdditionalControl).Then
                .Run(GenerateAnimations).Then
                .Run(Finalize);
        }

        public readonly static CollectTargetRenderersPass CollectTargetRenderers = CollectTargetRenderersPass.Instance;
        public readonly static CloningMaterialsPass CloningMaterials = CloningMaterialsPass.Instance;
        public readonly static NormalizeMaterialsPass NormalizeMaterials = NormalizeMaterialsPass.Instance;
        public readonly static GenerateAdditionalControlPass GenerateAdditionalControl = GenerateAdditionalControlPass.Instance;
        public readonly static GenerateAnimationsPass GenerateAnimations = GenerateAnimationsPass.Instance;
        public readonly static FinalizePass Finalize = FinalizePass.Instance;

        internal const string ParameterName_Toggle = "LightLimitEnable";
        internal const string ParameterName_Value = "LightLimitValue";
        internal const string ParameterName_Min = "LightLimitMin";
        internal const string ParameterName_Max = "LightLimitMax";
        internal const string ParameterName_Saturation = "LightLimitSaturation";
        internal const string ParameterName_Unlit = "LightLimitUnlit";
        internal const string ParameterName_ColorTemp = "LightLimitColorTemp";
        internal const string ParameterName_Emission = "LightLimitEmission";
        internal const string ParameterName_Reset = "LightLimitReset";
        internal const string ParameterName_Monochrome = "LightLimitMonochrome";
        internal const string ParameterName_Hue = "LightLimitHue";
        internal const string ParameterName_Value_HSV = "LightLimitValue_HSV";
        internal const string ParameterName_Gamma = "LightLimitGamma";
        internal const string ParameterName_LightDirection = "LightLimitLightDirection";
        internal const string ParameterName_RimLight = "LightLimitRimLight";
        internal const string ParameterName_RimLightBorder = "LightLimitRimLightBorder";
        internal const string ParameterName_RimLightBlur = "LightLimitRimLightBlur";
        internal const string ParameterName_RimLightFresnelPower = "LightLimitRimLightFresnelPower";
        internal const string ParameterName_Backlight = "LightLimitBacklight";
        internal const string ParameterName_BacklightBorder = "LightLimitBacklightBorder";
        internal const string ParameterName_BacklightBlur = "LightLimitBacklightBlur";
        internal const string ParameterName_BacklightDirectivity = "LightLimitBacklightDirectivity";
        internal const string ParameterName_DistanceFade = "LightLimitDistanceFade";
        internal const string ParameterName_ShadowEnvStrength = "LightLimitShadowEnvStrength";
        internal const string ParameterName_VertexLightStrength = "LightLimitVertexLightStrength";

        private static Session GetSession(BuildContext context)
        {
            var session = context.GetState<Session>();
            session.InitializeSession(context);
            return session;
        }

        private static LightLimitChangerObjectCache GetObjectCache(BuildContext context)
        {
            var cache = context.GetState<LightLimitChangerObjectCache>();
            if (cache.Container != context.AssetContainer)
                cache.Container = context.AssetContainer;
            return cache;
        }

        internal abstract class LightLimitChangerBasePass<TPass> : Pass<TPass> where TPass : Pass<TPass>, new()
        {
            protected LightLimitChangerObjectCache Cache => _cache;
            protected Session Session => _session;
            private LightLimitChangerObjectCache _cache;
            private Session _session;

            protected virtual bool IsForceRun { get; } = false;

            protected override void Execute(BuildContext context)
            {
                var session = GetSession(context);
                if (session.Settings == null || (!IsForceRun && session.Cancel))
                    return;
                _session = session;
                var cache = _cache = GetObjectCache(context);

                Execute(context, session, cache);
            }

            protected abstract void Execute(BuildContext context, Session session, LightLimitChangerObjectCache cache);
        }

        internal sealed class Session
        {
            public LightLimitChangerSettings Settings;
            public LightLimitChangerParameters Parameters;
            public ControlAnimationContainer[] Controls;
            public AnimatorController Controller;
            public LightLimitControlType TargetControl;
            public HashSet<Renderer> TargetRenderers;
            public DirectBlendTree DirectBlendTree;
            public List<ParameterConfig> AvatarParameters;

            public bool Cancel { get; set; } = false;

            public HashSet<Object> Excludes;

            private bool _initialized;

            public void InitializeSession(BuildContext context)
            {
                InitializeSession(context.AvatarRootObject.GetComponentInChildren<LightLimitChangerSettings>(), GetObjectCache(context));
            }

            public void InitializeSession(LightLimitChangerSettings settings, LightLimitChangerObjectCache cache)
            {
                if (_initialized)
                    return;

                Controller = new AnimatorController() { name = "Light Limit Controller" }.AddTo(cache);
                DirectBlendTree = new DirectBlendTree();
                Settings = settings;
                var parameters = Parameters = Settings?.Parameters ?? new LightLimitChangerParameters();
                Excludes = new HashSet<Object>(Settings?.Excludes ?? (IEnumerable<Object>)Array.Empty<Object>());
                var targetControl = LightLimitControlType.Light;
                List<ControlAnimationContainer> controls = new List<ControlAnimationContainer>();
                var defaultAnimation = new AnimationClip() { name = "Default" };
                AvatarParameters = new List<ParameterConfig>();

                if (!parameters.IsSeparateLightControl)
                {
                    controls.Add(ControlAnimationContainer.Create(LightLimitControlType.Light, Localization.S("ExpressionMenu.light"), "Light", ParameterName_Value, parameters.DefaultLightValue, Icons.Light, defaultAnimation));
                }
                else
                {
                    controls.Add(ControlAnimationContainer.Create(LightLimitControlType.LightMin, Localization.S("ExpressionMenu.light_min"), "Min Light", ParameterName_Min, parameters.DefaultMinLightValue, Icons.Light_Min, defaultAnimation));
                    controls.Add(ControlAnimationContainer.Create(LightLimitControlType.LightMax, Localization.S("ExpressionMenu.light_max"), "Max Light", ParameterName_Max, parameters.DefaultMaxLightValue, Icons.Light_Max, defaultAnimation));
                }

                if (parameters.AllowColorTempControl)
                {
                    targetControl |= LightLimitControlType.ColorTemperature;
                    controls.Add(ControlAnimationContainer.Create(LightLimitControlType.ColorTemperature, Localization.S("ExpressionMenu.color_temp"), "ColorTemp", ParameterName_ColorTemp, parameters.InitialTempControlValue, Icons.Temp, defaultAnimation));
                }

                if (parameters.AllowSaturationControl)
                {
                    targetControl |= LightLimitControlType.Saturation;
                    controls.Add(ControlAnimationContainer.Create(LightLimitControlType.Saturation, Localization.S("ExpressionMenu.saturation"), "Saturation", ParameterName_Saturation, parameters.InitialSaturationControlValue, Icons.Color, defaultAnimation));
                }

                if (parameters.AllowUnlitControl)
                {
                    targetControl |= LightLimitControlType.Unlit;
                    controls.Add(ControlAnimationContainer.Create(LightLimitControlType.Unlit, Localization.S("ExpressionMenu.unlit"),  "Unlit", ParameterName_Unlit, parameters.InitialUnlitControlValue, Icons.Unlit, defaultAnimation));
                }

                if (parameters.AllowMonochromeControl)
                {
                    targetControl |= LightLimitControlType.Monochrome;
                    controls.Add(ControlAnimationContainer.Create(LightLimitControlType.Monochrome, Localization.S("ExpressionMenu.monochrome"), "Monochrome", ParameterName_Monochrome, parameters.InitialMonochromeControlValue, Icons.Monochrome, defaultAnimation));
                }

                if (parameters.AllowEmissionControl)
                {
                    targetControl |= LightLimitControlType.Emission;
                    controls.Add(ControlAnimationContainer.Create(LightLimitControlType.Emission, Localization.S("ExpressionMenu.emission"), "Emission", ParameterName_Emission, 1.0f, Icons.Emission, defaultAnimation));
                }

                // HSV Controls (lilToon)
                if (parameters.AllowHueControl)
                {
                    targetControl |= LightLimitControlType.Hue;
                    controls.Add(ControlAnimationContainer.Create(LightLimitControlType.Hue, Localization.S("ExpressionMenu.hue"), "Hue", ParameterName_Hue, parameters.InitialHueControlValue, Icons.Color, defaultAnimation));
                }

                if (parameters.AllowValueControl)
                {
                    targetControl |= LightLimitControlType.Value;
                    controls.Add(ControlAnimationContainer.Create(LightLimitControlType.Value, Localization.S("ExpressionMenu.value"), "Value", ParameterName_Value_HSV, parameters.InitialValueControlValue, Icons.Light, defaultAnimation));
                }

                if (parameters.AllowGammaControl)
                {
                    targetControl |= LightLimitControlType.Gamma;
                    controls.Add(ControlAnimationContainer.Create(LightLimitControlType.Gamma, Localization.S("ExpressionMenu.gamma"), "Gamma", ParameterName_Gamma, parameters.InitialGammaControlValue, Icons.Light, defaultAnimation));
                }

                // lilToon Specific Controls
                if (parameters.AllowLightDirectionControl)
                {
                    targetControl |= LightLimitControlType.LightDirection;
                    controls.Add(ControlAnimationContainer.Create(LightLimitControlType.LightDirection, Localization.S("ExpressionMenu.light_direction"), "LightDirection", ParameterName_LightDirection, 0.5f, Icons.Light, defaultAnimation));
                }

                if (parameters.AllowRimLightControl)
                {
                    targetControl |= LightLimitControlType.RimLight;
                    controls.Add(ControlAnimationContainer.Create(LightLimitControlType.RimLight, Localization.S("ExpressionMenu.rim_light"), "RimLight", ParameterName_RimLight, parameters.InitialRimLightStrengthValue, Icons.Light, defaultAnimation));
                }

                if (parameters.AllowRimLightBorderControl)
                {
                    targetControl |= LightLimitControlType.RimLightBorder;
                    controls.Add(ControlAnimationContainer.Create(LightLimitControlType.RimLightBorder, Localization.S("ExpressionMenu.rim_light_border"), "RimLightBorder", ParameterName_RimLightBorder, parameters.InitialRimLightBorderValue, Icons.Light, defaultAnimation));
                }

                if (parameters.AllowRimLightBlurControl)
                {
                    targetControl |= LightLimitControlType.RimLightBlur;
                    controls.Add(ControlAnimationContainer.Create(LightLimitControlType.RimLightBlur, Localization.S("ExpressionMenu.rim_light_blur"), "RimLightBlur", ParameterName_RimLightBlur, parameters.InitialRimLightBlurValue, Icons.Light, defaultAnimation));
                }

                if (parameters.AllowRimLightFresnelPowerControl)
                {
                    targetControl |= LightLimitControlType.RimLightFresnelPower;
                    controls.Add(ControlAnimationContainer.Create(LightLimitControlType.RimLightFresnelPower, Localization.S("ExpressionMenu.rim_light_fresnel"), "RimLightFresnelPower", ParameterName_RimLightFresnelPower, parameters.InitialRimLightFresnelPowerValue, Icons.Light, defaultAnimation));
                }

                if (parameters.AllowBacklightControl)
                {
                    targetControl |= LightLimitControlType.Backlight;
                    controls.Add(ControlAnimationContainer.Create(LightLimitControlType.Backlight, Localization.S("ExpressionMenu.backlight"), "Backlight", ParameterName_Backlight, parameters.InitialBacklightStrengthValue, Icons.Light, defaultAnimation));
                }

                if (parameters.AllowBacklightBorderControl)
                {
                    targetControl |= LightLimitControlType.BacklightBorder;
                    controls.Add(ControlAnimationContainer.Create(LightLimitControlType.BacklightBorder, Localization.S("ExpressionMenu.backlight_border"), "BacklightBorder", ParameterName_BacklightBorder, parameters.InitialBacklightBorderValue, Icons.Light, defaultAnimation));
                }

                if (parameters.AllowBacklightBlurControl)
                {
                    targetControl |= LightLimitControlType.BacklightBlur;
                    controls.Add(ControlAnimationContainer.Create(LightLimitControlType.BacklightBlur, Localization.S("ExpressionMenu.backlight_blur"), "BacklightBlur", ParameterName_BacklightBlur, parameters.InitialBacklightBlurValue, Icons.Light, defaultAnimation));
                }

                if (parameters.AllowBacklightDirectivityControl)
                {
                    targetControl |= LightLimitControlType.BacklightDirectivity;
                    controls.Add(ControlAnimationContainer.Create(LightLimitControlType.BacklightDirectivity, Localization.S("ExpressionMenu.backlight_directivity"), "BacklightDirectivity", ParameterName_BacklightDirectivity, parameters.InitialBacklightDirectivityValue, Icons.Light, defaultAnimation));
                }

                if (parameters.AllowDistanceFadeControl)
                {
                    targetControl |= LightLimitControlType.DistanceFade;
                    controls.Add(ControlAnimationContainer.Create(LightLimitControlType.DistanceFade, Localization.S("ExpressionMenu.distance_fade"), "DistanceFade", ParameterName_DistanceFade, parameters.InitialDistanceFadeStrengthValue, Icons.Light, defaultAnimation));
                }

                if (parameters.AllowShadowEnvStrengthControl)
                {
                    targetControl |= LightLimitControlType.ShadowEnvStrength;
                    controls.Add(ControlAnimationContainer.Create(LightLimitControlType.ShadowEnvStrength, Localization.S("ExpressionMenu.shadow_env_strength"), "ShadowEnvStrength", ParameterName_ShadowEnvStrength, parameters.InitialShadowEnvStrengthValue, Icons.Light, defaultAnimation));
                }

                if (parameters.AllowVertexLightStrengthControl)
                {
                    targetControl |= LightLimitControlType.VertexLightStrength;
                    controls.Add(ControlAnimationContainer.Create(LightLimitControlType.VertexLightStrength, Localization.S("ExpressionMenu.vertex_light_strength"), "VertexLightStrength", ParameterName_VertexLightStrength, parameters.InitialVertexLightStrengthValue, Icons.Light, defaultAnimation));
                }

                Controls = controls.ToArray();

                TargetControl = targetControl;

                TargetRenderers = new HashSet<Renderer>();

                AddParameter(new ParameterConfig() { nameOrPrefix = ParameterName_Toggle, defaultValue = parameters.IsDefaultUse ? 1 : 0, syncType = ParameterSyncType.Bool });

                foreach (ref readonly var container in Controls.AsSpan())
                {
                    if (TargetControl.HasFlag(container.ControlType))
                    {
                        AddParameter(new ParameterConfig() { nameOrPrefix = container.ParameterName, defaultValue = container.DefaultValue, syncType = ParameterSyncType.Float });
                    }
                }

                _initialized = true;
            }

            public void AddParameter(ParameterConfig parameterConfig) => AvatarParameters.Add(parameterConfig);
        }
    }
}

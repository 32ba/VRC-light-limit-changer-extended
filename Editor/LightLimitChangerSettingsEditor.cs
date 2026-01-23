using nadena.dev.ndmf.util;
using UnityEditor;
using UnityEngine;

namespace io.github.azukimochi
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(LightLimitChangerSettings))]
    internal sealed class LightLimitChangerSettingsEditor : Editor
    {
        private SerializedProperty IsDefaultUse;
        private SerializedProperty IsValueSave;
        private SerializedProperty OverwriteDefaultLightMinMax;
        private SerializedProperty DefaultLightValue;
        private SerializedProperty DefaultMinLightValue;
        private SerializedProperty DefaultMaxLightValue;
        private SerializedProperty MaxLightValue;
        private SerializedProperty MinLightValue;
        private SerializedProperty TargetShaders;
        private SerializedProperty AllowColorTempControl;
        private SerializedProperty AllowSaturationControl;
        private SerializedProperty AllowMonochromeControl;
        private SerializedProperty AllowUnlitControl;
        private SerializedProperty AllowEmissionControl;
        private SerializedProperty InitialTempControlValue;
        private SerializedProperty InitialSaturationControlValue;
        private SerializedProperty InitialMonochromeControlValue;
        private SerializedProperty InitialUnlitControlValue;
        private SerializedProperty AddResetButton;
        private SerializedProperty IsGroupingAdditionalControls;
        private SerializedProperty IsSeparateLightControl;
        private SerializedProperty UseFlatMenuHierarchy;
        private SerializedProperty Excludes;
        private SerializedProperty WriteDefaults;

        // HSV Controls (lilToon)
        private SerializedProperty AllowHueControl;
        private SerializedProperty AllowValueControl;
        private SerializedProperty AllowGammaControl;
        private SerializedProperty InitialHueControlValue;
        private SerializedProperty InitialValueControlValue;
        private SerializedProperty InitialGammaControlValue;

        // lilToon Specific Controls
        private SerializedProperty AllowLightDirectionControl;
        private SerializedProperty AllowRimLightControl;
        private SerializedProperty AllowBacklightControl;
        private SerializedProperty AllowDistanceFadeControl;
        private SerializedProperty AllowShadowEnvStrengthControl;
        private SerializedProperty AllowVertexLightStrengthControl;
        private SerializedProperty InitialRimLightStrengthValue;
        private SerializedProperty InitialBacklightStrengthValue;
        private SerializedProperty InitialDistanceFadeStrengthValue;
        private SerializedProperty InitialShadowEnvStrengthValue;
        private SerializedProperty InitialVertexLightStrengthValue;

        private static bool _isOptionFoldoutOpen = true;
        private static bool _isCepareteInitValFoldoutOpen = false;
        private static bool _isLilToonFoldoutOpen = false;

        internal bool IsWindowMode = false;

        private bool ApplySettingToAvatar = false;
        private bool ApplySettingToProject = false;

        private void OnEnable()
        {
            var parameters = serializedObject.FindProperty(nameof(LightLimitChangerSettings.Parameters));
            IsDefaultUse = parameters.FindPropertyRelative(nameof(LightLimitChangerParameters.IsDefaultUse));
            IsValueSave = parameters.FindPropertyRelative(nameof(LightLimitChangerParameters.IsValueSave));
            OverwriteDefaultLightMinMax = parameters.FindPropertyRelative(nameof(LightLimitChangerParameters.OverwriteDefaultLightMinMax));
            DefaultLightValue = parameters.FindPropertyRelative(nameof(LightLimitChangerParameters.DefaultLightValue));
            DefaultMinLightValue = parameters.FindPropertyRelative(nameof(LightLimitChangerParameters.DefaultMinLightValue));
            DefaultMaxLightValue = parameters.FindPropertyRelative(nameof(LightLimitChangerParameters.DefaultMaxLightValue));
            MaxLightValue = parameters.FindPropertyRelative(nameof(LightLimitChangerParameters.MaxLightValue));
            MinLightValue = parameters.FindPropertyRelative(nameof(LightLimitChangerParameters.MinLightValue));
            TargetShaders = parameters.FindPropertyRelative(nameof(LightLimitChangerParameters.TargetShaders));
            AllowColorTempControl = parameters.FindPropertyRelative(nameof(LightLimitChangerParameters.AllowColorTempControl));
            AllowSaturationControl = parameters.FindPropertyRelative(nameof(LightLimitChangerParameters.AllowSaturationControl));
            AllowMonochromeControl = parameters.FindPropertyRelative(nameof(LightLimitChangerParameters.AllowMonochromeControl));
            AllowUnlitControl = parameters.FindPropertyRelative(nameof(LightLimitChangerParameters.AllowUnlitControl));
            AllowEmissionControl = parameters.FindPropertyRelative(nameof(LightLimitChangerParameters.AllowEmissionControl));
            InitialTempControlValue = parameters.FindPropertyRelative(nameof(LightLimitChangerParameters.InitialTempControlValue));
            InitialSaturationControlValue = parameters.FindPropertyRelative(nameof(LightLimitChangerParameters.InitialSaturationControlValue));
            InitialMonochromeControlValue = parameters.FindPropertyRelative(nameof(LightLimitChangerParameters.InitialMonochromeControlValue));
            InitialUnlitControlValue = parameters.FindPropertyRelative(nameof(LightLimitChangerParameters.InitialUnlitControlValue));
            AddResetButton = parameters.FindPropertyRelative(nameof(LightLimitChangerParameters.AddResetButton));
            IsSeparateLightControl = parameters.FindPropertyRelative(nameof(LightLimitChangerParameters.IsSeparateLightControl));
            IsGroupingAdditionalControls = parameters.FindPropertyRelative(nameof(LightLimitChangerParameters.IsGroupingAdditionalControls));
            UseFlatMenuHierarchy = parameters.FindPropertyRelative(nameof(LightLimitChangerParameters.UseFlatMenuHierarchy));
            Excludes = serializedObject.FindProperty(nameof(LightLimitChangerSettings.Excludes));
            WriteDefaults = serializedObject.FindProperty(nameof(LightLimitChangerSettings.WriteDefaults));

            // HSV Controls (lilToon)
            AllowHueControl = parameters.FindPropertyRelative(nameof(LightLimitChangerParameters.AllowHueControl));
            AllowValueControl = parameters.FindPropertyRelative(nameof(LightLimitChangerParameters.AllowValueControl));
            AllowGammaControl = parameters.FindPropertyRelative(nameof(LightLimitChangerParameters.AllowGammaControl));
            InitialHueControlValue = parameters.FindPropertyRelative(nameof(LightLimitChangerParameters.InitialHueControlValue));
            InitialValueControlValue = parameters.FindPropertyRelative(nameof(LightLimitChangerParameters.InitialValueControlValue));
            InitialGammaControlValue = parameters.FindPropertyRelative(nameof(LightLimitChangerParameters.InitialGammaControlValue));

            // lilToon Specific Controls
            AllowLightDirectionControl = parameters.FindPropertyRelative(nameof(LightLimitChangerParameters.AllowLightDirectionControl));
            AllowRimLightControl = parameters.FindPropertyRelative(nameof(LightLimitChangerParameters.AllowRimLightControl));
            AllowBacklightControl = parameters.FindPropertyRelative(nameof(LightLimitChangerParameters.AllowBacklightControl));
            AllowDistanceFadeControl = parameters.FindPropertyRelative(nameof(LightLimitChangerParameters.AllowDistanceFadeControl));
            AllowShadowEnvStrengthControl = parameters.FindPropertyRelative(nameof(LightLimitChangerParameters.AllowShadowEnvStrengthControl));
            AllowVertexLightStrengthControl = parameters.FindPropertyRelative(nameof(LightLimitChangerParameters.AllowVertexLightStrengthControl));
            InitialRimLightStrengthValue = parameters.FindPropertyRelative(nameof(LightLimitChangerParameters.InitialRimLightStrengthValue));
            InitialBacklightStrengthValue = parameters.FindPropertyRelative(nameof(LightLimitChangerParameters.InitialBacklightStrengthValue));
            InitialDistanceFadeStrengthValue = parameters.FindPropertyRelative(nameof(LightLimitChangerParameters.InitialDistanceFadeStrengthValue));
            InitialShadowEnvStrengthValue = parameters.FindPropertyRelative(nameof(LightLimitChangerParameters.InitialShadowEnvStrengthValue));
            InitialVertexLightStrengthValue = parameters.FindPropertyRelative(nameof(LightLimitChangerParameters.InitialVertexLightStrengthValue));
        }

        public override void OnInspectorGUI()
        {
            //new custom Editor styles include bold and large labels
            GUIStyle boldLabel = new GUIStyle(EditorStyles.label) { fontStyle = FontStyle.Bold, fontSize = 15};
            
            if (!IsWindowMode)
            {
                Utils.ShowVersionInfo();
                Utils.ShowDocumentLink();
                EditorGUILayout.Separator();
            }

            serializedObject.Update();
            EditorGUI.BeginChangeCheck();

            using (new EditorGUILayout.HorizontalScope())
            {
                using (new EditorGUILayout.VerticalScope())
                {
                    EditorGUILayout.LabelField(Localization.S("label.category.general_settings"), boldLabel);
                    EditorGUILayout.Space(5);
            
                    EditorGUILayout.PropertyField(IsDefaultUse, Localization.G("label.use_default", "tip.use_default"));
                    EditorGUILayout.PropertyField(IsValueSave, Localization.G("label.save_value", "tip.save_value"));
                }

                using (new EditorGUILayout.VerticalScope(GUI.skin.box))
                {
                    EditorGUILayout.LabelField(Localization.S("category.save_settings"), boldLabel);
                    EditorGUILayout.Space(5);
                    
                    var settings = target as LightLimitChangerSettings;
                    var parameter = settings?.Parameters;
                    EditorGUI.BeginDisabledGroup(parameter == null || PrefabUtility.IsPartOfAnyPrefab(target) == false);

                    ApplySettingToAvatar = GUILayout.Button(Localization.S("label.apply_settings_avatar"), EditorStyles.miniButton);
                    if (ApplySettingToAvatar)
                    {
                        LightLimitChangerPrefab.SavePrefabSetting(serializedObject);
                    }
                    ApplySettingToProject = GUILayout.Button(Localization.S("label.apply_settings_project"), EditorStyles.miniButton);
                    if (ApplySettingToProject)
                    {
                        if (EditorUtility.DisplayDialog(Localization.S("Window.info.gloabl_settings.save"), Localization.S("Window.info.global_settings.save_message"), Localization.S("Window.info.choice.apply_save"), Localization.S("Window.info.cancel")))
                        {
                            LightLimitChangerPrefab.SavePrefabSettingAsGlobal(serializedObject, parameter);
                        }
                    }
                    
                    EditorGUI.EndDisabledGroup();

                    if (PrefabUtility.IsPartOfAnyPrefab(target) == false)
                    {
                        EditorGUILayout.HelpBox("UnpackPrefabされているので保存できません", MessageType.Info);
                    }
                }
            }
            EditorGUILayout.Space(10);
            EditorGUILayout.PropertyField(IsSeparateLightControl, Localization.G("label.separate_light_control"));
            EditorGUILayout.PropertyField(MaxLightValue, Localization.G("label.light_max", "tip.light_max"));
            EditorGUILayout.PropertyField(MinLightValue, Localization.G("label.light_min", "tip.light_min"));
            EditorGUI.BeginDisabledGroup(IsSeparateLightControl.boolValue == true);
            EditorGUILayout.PropertyField(DefaultLightValue, Localization.G("label.light_default", "tip.light_default"));
            EditorGUI.EndDisabledGroup();
            
            EditorGUI.BeginDisabledGroup(IsSeparateLightControl.boolValue == false);
            _isCepareteInitValFoldoutOpen = EditorGUILayout.Foldout(_isCepareteInitValFoldoutOpen,
                Localization.G("label.separate_light_control_init_val"));
            if (_isCepareteInitValFoldoutOpen)
            {
                EditorGUILayout.PropertyField(DefaultMaxLightValue, Localization.G("label.light_max_default"));
                EditorGUILayout.PropertyField(DefaultMinLightValue, Localization.G("label.light_min_default"));
            }
            EditorGUI.EndDisabledGroup();
            
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField(Localization.S("label.category.additional_settings"), boldLabel);
            
            using (new EditorGUILayout.HorizontalScope())
            {
                using (new EditorGUILayout.VerticalScope())
                {
                    EditorGUILayout.Space(25);
                    EditorGUILayout.PropertyField(AllowColorTempControl,
                        Localization.G("label.allow_color_tmp", "tip.allow_color_tmp"));
                    EditorGUILayout.PropertyField(AllowSaturationControl,
                        Localization.G("label.allow_saturation", "tip.allow_saturation"));
                    EditorGUILayout.PropertyField(AllowMonochromeControl,
                        Localization.G("label.allow_monochrome", "tip.allow_monochrome"));
                    EditorGUILayout.PropertyField(AllowUnlitControl,
                        Localization.G("label.allow_unlit", "tip.allow_unlit"));
                    EditorGUILayout.PropertyField(AllowEmissionControl,
                        Localization.G("label.allow_emission", "tip.allow_emission"));
                    EditorGUILayout.Space(5);
                    EditorGUILayout.PropertyField(AddResetButton, Localization.G("label.allow_reset", "tip.allow_reset"));
                    EditorGUILayout.PropertyField(IsGroupingAdditionalControls, Localization.G("label.grouping_additional_controls"));
                    EditorGUILayout.PropertyField(UseFlatMenuHierarchy, Localization.G("label.flat_menu_hierarchy", "tip.flat_menu_hierarchy"));

                }

                using (new EditorGUILayout.VerticalScope(GUI.skin.box))
                {
                    EditorGUILayout.LabelField(Localization.G("info.initial_val"));
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        EditorGUILayout.Space(10);
                        EditorGUILayout.LabelField(Localization.G("label.color_temp"), GUILayout.MaxWidth(70.0f), GUILayout.ExpandWidth(false));
                        EditorGUI.BeginDisabledGroup(AllowColorTempControl.boolValue == false);
                        EditorGUILayout.PropertyField(InitialTempControlValue, Localization.G(""));
                        EditorGUI.EndDisabledGroup();
                    }
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        EditorGUILayout.Space(10);
                        EditorGUILayout.LabelField(Localization.G("label.saturation"), GUILayout.MaxWidth(70.0f), GUILayout.ExpandWidth(false));
                        EditorGUI.BeginDisabledGroup(AllowSaturationControl.boolValue == false);
                        EditorGUILayout.PropertyField(InitialSaturationControlValue, Localization.G(""));
                        EditorGUI.EndDisabledGroup();

                    }
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        EditorGUILayout.Space(10);
                        EditorGUILayout.LabelField(Localization.G("label.monochrome"), GUILayout.MaxWidth(70.0f), GUILayout.ExpandWidth(false));
                        EditorGUI.BeginDisabledGroup(AllowMonochromeControl.boolValue == false);
                        EditorGUILayout.PropertyField(InitialMonochromeControlValue, Localization.G(""));
                        EditorGUI.EndDisabledGroup();
                    }
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        EditorGUILayout.Space(10);
                        EditorGUILayout.LabelField(Localization.G("label.unlit"), GUILayout.MaxWidth(70.0f), GUILayout.ExpandWidth(false));
                        EditorGUI.BeginDisabledGroup(AllowUnlitControl.boolValue == false);
                        EditorGUILayout.PropertyField(InitialUnlitControlValue, Localization.G(""));
                        EditorGUI.EndDisabledGroup();
                    }
                }
            }

            // lilToon Specific Settings
            EditorGUILayout.Space(10);
            using (var group = new Utils.FoldoutHeaderGroupScope(ref _isLilToonFoldoutOpen, Localization.G("label.category.liltoon_settings")))
            {
                if (group.IsOpen)
                {
                    EditorGUILayout.HelpBox(Localization.S("info.liltoon_only"), MessageType.Info);

                    using (new EditorGUILayout.HorizontalScope())
                    {
                        using (new EditorGUILayout.VerticalScope())
                        {
                            EditorGUILayout.LabelField(Localization.G("label.category.hsv_controls"), EditorStyles.boldLabel);
                            EditorGUILayout.PropertyField(AllowHueControl, Localization.G("label.allow_hue", "tip.allow_hue"));
                            EditorGUILayout.PropertyField(AllowValueControl, Localization.G("label.allow_value", "tip.allow_value"));
                            EditorGUILayout.PropertyField(AllowGammaControl, Localization.G("label.allow_gamma", "tip.allow_gamma"));

                            EditorGUILayout.Space(10);
                            EditorGUILayout.LabelField(Localization.G("label.category.light_controls"), EditorStyles.boldLabel);
                            EditorGUILayout.PropertyField(AllowLightDirectionControl, Localization.G("label.allow_light_direction", "tip.allow_light_direction"));
                            EditorGUILayout.PropertyField(AllowRimLightControl, Localization.G("label.allow_rim_light", "tip.allow_rim_light"));
                            EditorGUILayout.PropertyField(AllowBacklightControl, Localization.G("label.allow_backlight", "tip.allow_backlight"));
                            EditorGUILayout.PropertyField(AllowDistanceFadeControl, Localization.G("label.allow_distance_fade", "tip.allow_distance_fade"));
                            EditorGUILayout.PropertyField(AllowShadowEnvStrengthControl, Localization.G("label.allow_shadow_env_strength", "tip.allow_shadow_env_strength"));
                            EditorGUILayout.PropertyField(AllowVertexLightStrengthControl, Localization.G("label.allow_vertex_light_strength", "tip.allow_vertex_light_strength"));
                        }

                        using (new EditorGUILayout.VerticalScope(GUI.skin.box))
                        {
                            EditorGUILayout.LabelField(Localization.G("info.initial_val_liltoon"));
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                EditorGUILayout.Space(10);
                                EditorGUILayout.LabelField(Localization.G("label.hue"), GUILayout.MaxWidth(80.0f), GUILayout.ExpandWidth(false));
                                EditorGUI.BeginDisabledGroup(AllowHueControl.boolValue == false);
                                EditorGUILayout.PropertyField(InitialHueControlValue, Localization.G(""));
                                EditorGUI.EndDisabledGroup();
                            }
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                EditorGUILayout.Space(10);
                                EditorGUILayout.LabelField(Localization.G("label.value"), GUILayout.MaxWidth(80.0f), GUILayout.ExpandWidth(false));
                                EditorGUI.BeginDisabledGroup(AllowValueControl.boolValue == false);
                                EditorGUILayout.PropertyField(InitialValueControlValue, Localization.G(""));
                                EditorGUI.EndDisabledGroup();
                            }
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                EditorGUILayout.Space(10);
                                EditorGUILayout.LabelField(Localization.G("label.gamma"), GUILayout.MaxWidth(80.0f), GUILayout.ExpandWidth(false));
                                EditorGUI.BeginDisabledGroup(AllowGammaControl.boolValue == false);
                                EditorGUILayout.PropertyField(InitialGammaControlValue, Localization.G(""));
                                EditorGUI.EndDisabledGroup();
                            }
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                EditorGUILayout.Space(10);
                                EditorGUILayout.LabelField(Localization.G("label.rim_light"), GUILayout.MaxWidth(80.0f), GUILayout.ExpandWidth(false));
                                EditorGUI.BeginDisabledGroup(AllowRimLightControl.boolValue == false);
                                EditorGUILayout.PropertyField(InitialRimLightStrengthValue, Localization.G(""));
                                EditorGUI.EndDisabledGroup();
                            }
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                EditorGUILayout.Space(10);
                                EditorGUILayout.LabelField(Localization.G("label.backlight"), GUILayout.MaxWidth(80.0f), GUILayout.ExpandWidth(false));
                                EditorGUI.BeginDisabledGroup(AllowBacklightControl.boolValue == false);
                                EditorGUILayout.PropertyField(InitialBacklightStrengthValue, Localization.G(""));
                                EditorGUI.EndDisabledGroup();
                            }
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                EditorGUILayout.Space(10);
                                EditorGUILayout.LabelField(Localization.G("label.distance_fade"), GUILayout.MaxWidth(80.0f), GUILayout.ExpandWidth(false));
                                EditorGUI.BeginDisabledGroup(AllowDistanceFadeControl.boolValue == false);
                                EditorGUILayout.PropertyField(InitialDistanceFadeStrengthValue, Localization.G(""));
                                EditorGUI.EndDisabledGroup();
                            }
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                EditorGUILayout.Space(10);
                                EditorGUILayout.LabelField(Localization.G("label.shadow_env"), GUILayout.MaxWidth(80.0f), GUILayout.ExpandWidth(false));
                                EditorGUI.BeginDisabledGroup(AllowShadowEnvStrengthControl.boolValue == false);
                                EditorGUILayout.PropertyField(InitialShadowEnvStrengthValue, Localization.G(""));
                                EditorGUI.EndDisabledGroup();
                            }
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                EditorGUILayout.Space(10);
                                EditorGUILayout.LabelField(Localization.G("label.vertex_light"), GUILayout.MaxWidth(80.0f), GUILayout.ExpandWidth(false));
                                EditorGUI.BeginDisabledGroup(AllowVertexLightStrengthControl.boolValue == false);
                                EditorGUILayout.PropertyField(InitialVertexLightStrengthValue, Localization.G(""));
                                EditorGUI.EndDisabledGroup();
                            }
                        }
                    }
                }
            }

            EditorGUILayout.Space(10);
            using (var group = new Utils.FoldoutHeaderGroupScope(ref _isOptionFoldoutOpen, Localization.G("category.select_option")))
            {
                if (group.IsOpen)
                {
                    EditorGUILayout.PropertyField(OverwriteDefaultLightMinMax, Localization.G("label.override_min_max", "tip.override_min_max"));
                    EditorGUILayout.PropertyField(TargetShaders, Localization.G("label.target_shader", "tip.target_shader"));
                    WriteDefaults.intValue = EditorGUILayout.Popup(Utils.Label("Write Defaults"), WriteDefaults.intValue, new[] { Localization.S("label.match_avatar"), "OFF", "ON" });
                    EditorGUILayout.PropertyField(Excludes, Localization.G("label.excludes"));
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }

            if (!IsWindowMode)
            {
                EditorGUILayout.Separator();
                Localization.ShowLocalizationUI();
            }
        }
    }
}

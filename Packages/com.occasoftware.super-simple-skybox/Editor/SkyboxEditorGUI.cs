using UnityEngine;
using UnityEditor;

namespace OccaSoftware.SuperSimpleSkybox.Editor
{
    public class SkyboxEditorGUI : ShaderGUI
    {
        Material t;

        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            t = materialEditor.target as Material;
            MaterialEditor e = materialEditor;

            // Ground
            MaterialProperty groundColor = FindProperty("_GroundColor", properties);
            MaterialProperty groundEnabled = FindProperty("_GroundEnabled", properties);
            MaterialProperty groundFadeAmount = FindProperty("_GroundFadeAmount", properties);
            MaterialProperty groundHeight = FindProperty("_Ground_Height", properties);

            // Sky Colors
            MaterialProperty constantColorMode = FindProperty("_Constant_Color_Mode", properties);
            MaterialProperty skyColorBlend = FindProperty("_SkyColorBlend", properties);
            MaterialProperty horizonColorDay = FindProperty("_HorizonColorDay", properties);
            MaterialProperty skyColorDay = FindProperty("_SkyColorDay", properties);
            MaterialProperty horizonColorNight = FindProperty("_HorizonColorNight", properties);
            MaterialProperty skyColorNight = FindProperty("_SkyColorNight", properties);
            MaterialProperty horizonSaturationFalloff = FindProperty("_HorizonSaturationFalloff", properties);
            MaterialProperty horizonSaturationAmount = FindProperty("_HorizonSaturationAmount", properties);

            // Sun
            MaterialProperty sunEnabled = FindProperty("_Sun_Enabled", properties);
            MaterialProperty sunColorZenith = FindProperty("_SunColorZenith", properties);
            MaterialProperty sunColorHorizon = FindProperty("_SunColorHorizon", properties);
            MaterialProperty sunAngularDiameter = FindProperty("_SunAngularDiameter", properties);
            MaterialProperty sunFalloffIntensity = FindProperty("_SunFalloffIntensity", properties);
            MaterialProperty sunFalloff = FindProperty("_SunFalloff", properties);
            MaterialProperty sunSkyLightingEnabled = FindProperty("_SunSkyLightingEnabled", properties);

            // Sunset
            MaterialProperty sunsetHorizontalFalloff = FindProperty("_SunsetHorizontalFalloff", properties);
            MaterialProperty sunsetVerticalFalloff = FindProperty("_SunsetVerticalFalloff", properties);
            MaterialProperty sunsetRadialFalloff = FindProperty("_SunsetRadialFalloff", properties);
            MaterialProperty sunsetIntensity = FindProperty("_SunsetIntensity", properties);

            // Clouds
            MaterialProperty cloudsEnabled = FindProperty("_Clouds_Enabled", properties);
            MaterialProperty cloudTexture = FindProperty("_CloudTexture", properties);
            MaterialProperty cloudWindSpeed = FindProperty("_CloudWindSpeed", properties);
            MaterialProperty cloudiness = FindProperty("_Cloudiness", properties);
            MaterialProperty cloudOpacity = FindProperty("_CloudOpacity", properties);
            MaterialProperty cloudSharpness = FindProperty("_CloudSharpness", properties);
            MaterialProperty cloudShadingIntensity = FindProperty("_Shading_Intensity", properties);
            MaterialProperty cloudFalloff = FindProperty("_CloudFalloff", properties);
            MaterialProperty cloudScale = FindProperty("_CloudScale", properties);
            MaterialProperty cloudColorDay = FindProperty("_CloudColorDay", properties);
            MaterialProperty cloudColorNight = FindProperty("_CloudColorNight", properties);

            MaterialProperty cloudIterations = FindProperty("_Cloud_Iterations", properties);
            MaterialProperty cloudGain = FindProperty("_Cloud_Gain", properties);
            MaterialProperty cloudLacunarity = FindProperty("_Cloud_Lacunarity", properties);

            // Stars
            MaterialProperty starsEnabled = FindProperty("_Stars_Enabled", properties);
            MaterialProperty starTexture = FindProperty("_StarTexture", properties);
            MaterialProperty starHorizonFalloff = FindProperty("_StarHorizonFalloff", properties);
            MaterialProperty starScale = FindProperty("_StarScale", properties);
            MaterialProperty starSpeed = FindProperty("_StarSpeed", properties);
            MaterialProperty starIntensity = FindProperty("_StarIntensity", properties);
            MaterialProperty starDaytimeBrightness = FindProperty("_StarDaytimeBrightness", properties);
            MaterialProperty starSaturation = FindProperty("_StarSaturation", properties);
            MaterialProperty proceduralStarsEnabled = FindProperty("_ProceduralStarsEnabled", properties);

            MaterialProperty textureStarsEnabled = FindProperty("_Use_Texture_Stars", properties);
            MaterialProperty textureStarTint = FindProperty("_Star_Texture_Tint", properties);

            MaterialProperty starSharpness = FindProperty("_StarSharpness", properties);
            MaterialProperty starFrequency = FindProperty("_StarFrequency", properties);

            // Moon
            MaterialProperty moonEnabled = FindProperty("_Moon_Enabled", properties);
            MaterialProperty moonAngularDiameter = FindProperty("_MoonAngularDiameter", properties);
            MaterialProperty moonFalloffAmount = FindProperty("_MoonFalloff", properties);
            MaterialProperty moonColor = FindProperty("_MoonColor", properties);

            ColorMode currentColorMode = (ColorMode)constantColorMode.floatValue;

            DrawCommonProperties();
            DrawGroundProperties();
            DrawSkyProperties();
            DrawSunProperties();
            DrawMoonProperties();
            DrawStarProperties();
            DrawCloudProperties();
            DrawLinks();
            Validate();

            void DrawCommonProperties()
            {
                EditorGUILayout.LabelField("Common Settings", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                EditorGUI.BeginChangeCheck();
                constantColorMode.floatValue = (float)(ColorMode)EditorGUILayout.EnumPopup("Color Mode", (ColorMode)constantColorMode.floatValue);
                if (EditorGUI.EndChangeCheck())
                {
                    e.PropertiesChanged();
                }

                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }

            void DrawGroundProperties()
            {
                EditorGUILayout.LabelField("Ground Settings", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                e.ShaderProperty(groundEnabled, "Enabled");
                if (groundEnabled.floatValue == 1)
                {
                    e.ShaderProperty(groundColor, "Color");
                    e.ShaderProperty(groundHeight, "Height");
                    e.ShaderProperty(groundFadeAmount, "Fade Amount");
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }
            void DrawSkyProperties()
            {
                EditorGUILayout.LabelField("Sky Settings", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                e.ShaderProperty(skyColorBlend, "Horizon-Sky Blend");

                string title = currentColorMode == ColorMode.Constant ? "Colors" : "Day Colors";
                EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                e.ShaderProperty(horizonColorDay, "Horizon");
                e.ShaderProperty(skyColorDay, "Sky");
                EditorGUI.indentLevel--;

                if (currentColorMode == ColorMode.DayAndNightColors)
                {
                    EditorGUILayout.LabelField("Night Colors", EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;
                    e.ShaderProperty(horizonColorNight, "Horizon");
                    e.ShaderProperty(skyColorNight, "Sky");
                    EditorGUI.indentLevel--;
                }

                EditorGUILayout.LabelField("Horizon Saturation", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                e.ShaderProperty(horizonSaturationAmount, "Amount");
                if (horizonSaturationAmount.floatValue < 1.0f)
                {
                    e.ShaderProperty(horizonSaturationFalloff, "Falloff");
                }

                EditorGUI.indentLevel--;

                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }
            void DrawSunProperties()
            {
                // Sun
                EditorGUILayout.LabelField("Sun Settings", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                e.ShaderProperty(sunEnabled, "Sun Enabled");
                if (sunEnabled.floatValue == 1.0f)
                {
                    e.ShaderProperty(sunAngularDiameter, "Angular Diameter");
                    e.ShaderProperty(sunColorHorizon, "Horizon Color");
                    e.ShaderProperty(sunColorZenith, "Zenith Color");
                    e.ShaderProperty(sunSkyLightingEnabled, "Sky Lighting Enabled");
                    if (sunSkyLightingEnabled.floatValue == 1)
                    {
                        e.ShaderProperty(sunFalloff, "Falloff Amount");
                        e.ShaderProperty(sunFalloffIntensity, "Falloff Intensity");

                        EditorGUILayout.LabelField("Sunset Settings", EditorStyles.boldLabel);
                        EditorGUI.indentLevel++;
                        e.ShaderProperty(sunsetIntensity, "Intensity");
                        e.ShaderProperty(sunsetRadialFalloff, "Radial Falloff");
                        e.ShaderProperty(sunsetHorizontalFalloff, "Horizontal Falloff");
                        e.ShaderProperty(sunsetVerticalFalloff, "Vertical Falloff");
                        EditorGUI.indentLevel--;
                    }
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }
            void DrawMoonProperties()
            {
                EditorGUILayout.LabelField("Moon Settings", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                e.ShaderProperty(moonEnabled, "Moon Enabled");
                if (moonEnabled.floatValue == 1.0f)
                {
                    e.ShaderProperty(moonAngularDiameter, "Angular Diameter");
                    e.ShaderProperty(moonColor, "Color");
                    e.ShaderProperty(moonFalloffAmount, "Falloff Amount");
                }

                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }
            void DrawStarProperties()
            {
                // Stars
                EditorGUILayout.LabelField("Star Settings", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                e.ShaderProperty(starsEnabled, "Stars Enabled");
                if (starsEnabled.floatValue == 1.0f)
                {
                    EditorGUILayout.LabelField("Common");
                    EditorGUI.indentLevel++;
                    e.ShaderProperty(starIntensity, "Brightness");
                    if (currentColorMode == ColorMode.DayAndNightColors)
                    {
                        e.ShaderProperty(starDaytimeBrightness, "Daytime Brightness");
                    }

                    e.ShaderProperty(starHorizonFalloff, "Horizon Falloff");
                    e.ShaderProperty(starSaturation, "Saturation");
                    EditorGUI.indentLevel--;

                    proceduralStarsEnabled.floatValue = EditorGUILayout.Toggle(
                        new GUIContent("Procedural Stars Enabled"),
                        proceduralStarsEnabled.floatValue == 1.0f ? true : false
                    )
                        ? 1.0f
                        : 0.0f;
                    if (proceduralStarsEnabled.floatValue == 1.0f)
                    {
                        EditorGUI.indentLevel++;
                        e.ShaderProperty(starSharpness, "Sharpness");
                        e.ShaderProperty(starFrequency, "Amount");
                        EditorGUI.indentLevel--;
                    }

                    textureStarsEnabled.floatValue = EditorGUILayout.Toggle(
                        new GUIContent("Star Texture Enabled"),
                        textureStarsEnabled.floatValue == 1.0f ? true : false
                    )
                        ? 1.0f
                        : 0.0f;
                    if (textureStarsEnabled.floatValue == 1.0f)
                    {
                        EditorGUI.indentLevel++;
                        starTexture.textureValue = (Texture)
                            EditorGUILayout.ObjectField(
                                "Texture",
                                starTexture.textureValue,
                                typeof(Texture),
                                true,
                                GUILayout.Height(EditorGUIUtility.singleLineHeight)
                            );
                        if (starTexture.textureValue != null)
                        {
                            e.ShaderProperty(textureStarTint, "Tint");
                            e.ShaderProperty(starScale, "Scale");
                            e.ShaderProperty(starSpeed, "Rotation Speed");
                        }
                        EditorGUI.indentLevel--;
                    }
                }

                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }
            void DrawCloudProperties()
            {
                EditorGUILayout.LabelField("Cloud Settings", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                e.ShaderProperty(cloudsEnabled, "Clouds Enabled");
                if (cloudsEnabled.floatValue == 1.0f)
                {
                    EditorGUILayout.LabelField("Texture Settings", EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;
                    Texture cachedTexture = cloudTexture.textureValue;
                    cloudTexture.textureValue = (Texture)
                        EditorGUILayout.ObjectField(
                            "Texture",
                            cloudTexture.textureValue,
                            typeof(Texture),
                            true,
                            GUILayout.Height(EditorGUIUtility.singleLineHeight)
                        );
                    if (cachedTexture == null && cloudTexture.textureValue != null)
                        cloudiness.floatValue = 0.5f;

                    if (cloudTexture.textureValue != null)
                    {
                        Vector2Int s = EditorGUILayout.Vector2IntField(
                            "Scale",
                            new Vector2Int((int)cloudScale.vectorValue.x, (int)cloudScale.vectorValue.y)
                        );
                        cloudScale.vectorValue = new Vector4(s.x, s.y, 0, 0);

                        cloudWindSpeed.vectorValue = EditorGUILayout.Vector2Field("Speed", cloudWindSpeed.vectorValue);

                        EditorGUI.indentLevel--;
                        EditorGUILayout.LabelField("Look Settings", EditorStyles.boldLabel);
                        EditorGUI.indentLevel++;
                        e.ShaderProperty(cloudiness, "Cloudiness");
                        e.ShaderProperty(cloudOpacity, "Opacity");
                        e.ShaderProperty(cloudSharpness, "Sharpness");
                        e.ShaderProperty(cloudShadingIntensity, "Shading Intensity");
                        e.ShaderProperty(cloudFalloff, "Zenith Falloff");
                        DrawIntegerProperty(cloudIterations, new GUIContent("Iterations"), 1, 4);
                        if (cloudIterations.floatValue >= 2)
                        {
                            EditorGUI.indentLevel++;
                            e.ShaderProperty(cloudGain, "Gain");
                            DrawIntegerProperty(cloudLacunarity, new GUIContent("Lacunarity"), 2, 5);
                            EditorGUI.indentLevel--;
                        }
                        EditorGUI.indentLevel--;

                        EditorGUILayout.LabelField("Color Settings", EditorStyles.boldLabel);
                        EditorGUI.indentLevel++;
                        string title = currentColorMode == ColorMode.Constant ? "Cloud Color" : "Day";
                        e.ShaderProperty(cloudColorDay, title);

                        if (currentColorMode == ColorMode.DayAndNightColors)
                        {
                            e.ShaderProperty(cloudColorNight, "Night");
                        }

                        EditorGUI.indentLevel--;
                    }
                    else
                    {
                        cloudiness.floatValue = 0;
                    }
                }

                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }

            void DrawLinks()
            {
                EditorGUILayout.BeginVertical();
                if (EditorGUILayout.LinkButton("Manual"))
                {
                    Application.OpenURL("https://www.occasoftware.com/assets/super-simple-skybox");
                }
                EditorGUILayout.Space();
                EditorGUILayout.EndVertical();
            }

            void Validate()
            {
                sunAngularDiameter.floatValue = Mathf.Max(sunAngularDiameter.floatValue, 0);
                sunFalloff.floatValue = Mathf.Max(sunFalloff.floatValue, 0);
                sunFalloffIntensity.floatValue = Mathf.Max(sunFalloffIntensity.floatValue, 0);

                moonAngularDiameter.floatValue = Mathf.Max(moonAngularDiameter.floatValue, 0);
                moonFalloffAmount.floatValue = Mathf.Max(moonFalloffAmount.floatValue, 0);

                starSaturation.floatValue = Mathf.Max(starSaturation.floatValue, 0);
            }

            void DrawIntegerProperty(MaterialProperty p, GUIContent c, int min, int max)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = p.hasMixedValue;
                float v = EditorGUILayout.IntSlider(c, (int)p.floatValue, min, max);

                if (EditorGUI.EndChangeCheck())
                {
                    p.floatValue = v;
                }
                EditorGUI.showMixedValue = false;
            }
        }

        private enum StarControlState
        {
            Texture,
            Procedural
        }

        private enum ColorMode
        {
            DayAndNightColors,
            Constant
        }
    }
}

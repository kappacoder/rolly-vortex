using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Febucci.UI.Core
{
    [CustomEditor(typeof(TextAnimator))]
    public class TextAnimatorDrawer : Editor
    {
        const string alertTextSizeDep = "This effect's strength changes with different sizes and fonts.";

        static readonly Color expandedColor = new Color(1, 1, 1);
        static readonly Color errorColor = new Color(1, .6f, .6f);
        static readonly Color selectedShowColor = new Color(.7f, 1, .7f);
        static readonly Color notExpandedColor = Color.gray * 1.5f;
        static readonly Color sectionsColor = new Color(.95f, .95f, .95f);

        //const string availableAppTags = "size, fade, offset, vertexp, horiexp, diagexp, rot";
        static readonly string[] availableAppTags = new string[]{
            TextAnimator.ap_Size,
            TextAnimator.ap_Fade,
            TextAnimator.ap_Offset,
            TextAnimator.ap_VertExp,
            TextAnimator.ap_HoriExp,
            TextAnimator.ap_DiagExp,
            TextAnimator.ap_Rot,
        };

        static string availableAppBuiltinTagsLongText;

        #region Structs
        struct Effect
        {
            public bool isPresent;
            bool show;
            bool dependant; //if effect changes based on size
            string effectName;
            public string effectTag { get; private set; }

            List<SerializedProperty> properties;

            public Effect(string effectName, string effectTag, bool dependant, SerializedProperty parentProperty, params string[] names)
            {
                this.isPresent = false;
                this.dependant = dependant;
                this.effectName = "-> " + (dependant ? "[!] " : "") + effectName + ", <" + effectTag + '>';
                this.effectTag = effectTag;
                show = false;

                properties = new List<SerializedProperty>();
                for (int i = 0; i < names.Length; i++)
                {
                    properties.Add(parentProperty.FindPropertyRelative(names[i]));
                }
            }

            public void Show()
            {
                if (show)
                    GUI.backgroundColor = expandedColor;
                else
                    GUI.backgroundColor = notExpandedColor;

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                if (GUILayout.Button(effectName, show ? EditorStyles.boldLabel : EditorStyles.label))
                    show = !show;

                GUI.backgroundColor = Color.white;

                if (show)
                {
                    if (dependant)
                    {
                        EditorGUILayout.LabelField(alertTextSizeDep, EditorStyles.centeredGreyMiniLabel);
                    }

                    for (int i = 0; i < properties.Count; i++)
                    {
                        EditorGUILayout.PropertyField(properties[i]);
                    }
                }

                EditorGUILayout.EndVertical();
            }
        }

        class AppPreset
        {
            GUIContent tagsContainerLabel;
            SerializedProperty tagsContainer;
            SerializedProperty customs;

            bool showBuiltIn;

            Effect appSize;
            Effect appFade;
            Effect appVertExp;
            Effect appHoriExp;
            Effect appDiagExp;
            Effect appOffset;
            Effect appRot;

            public AppPreset(SerializedProperty preset)
            {
                tagsContainerLabel = new GUIContent("Default Tags");
                this.tagsContainer = preset.FindPropertyRelative("tags");
                var values = preset.FindPropertyRelative("values");
                var defaults = values.FindPropertyRelative("defaults");
                this.customs = values.FindPropertyRelative("customs");
                this.showBuiltIn = false;
                appSize = new Effect(
                    "Size",
                    TextAnimator.ap_Size,
                    false,
                    defaults,
                    "sizeDuration",
                    "sizeAmplitude"
                    );

                appFade = new Effect(
                    "Fade",
                    TextAnimator.ap_Fade,
                    false,
                    defaults,
                    "fadeDuration"
                    );

                appVertExp = new Effect(
                    "Vertical Expand",
                    TextAnimator.ap_VertExp,
                    false,
                    defaults,
                    "verticalExpandDuration",
                    "verticalFromBottom"
                    );

                appHoriExp = new Effect(
                    "Horizontal Expand",
                    TextAnimator.ap_HoriExp,
                    false,
                    defaults,
                    "horizontalExpandDuration",
                    "horizontalExpandStart"
                    );

                appDiagExp = new Effect(
                    "Diagonal Expand",
                    TextAnimator.ap_DiagExp,
                    false,
                    defaults,
                    "diagonalExpandDuration",
                    "diagonalFromBttmLeft"
                    );

                appOffset = new Effect(
                    "Offset",
                    TextAnimator.ap_Offset,
                    true,
                    defaults,
                    "offsetDir",
                    "offsetDuration",
                    "offsetAmplitude"
                    );

                appRot = new Effect(
                    "Rotation",
                    TextAnimator.ap_Rot,
                    false,
                    defaults,
                    "rotationDuration",
                    "rotationStartAngle"
                    );
            }

            bool IsTagAvailable(string tag)
            {
                if (tag.Length == 0) //def value will be used
                    return true;

                for (int i = 0; i < availableAppTags.Length; i++)
                {
                    if (tag.Equals(availableAppTags[i]))
                        return true;
                }

                return false;
            }

            void ShowEffect(ref Effect effect)
            {
                effect.isPresent = false;
                for (int i = 0; !effect.isPresent && i < tagsContainer.arraySize; i++)
                {
                    effect.isPresent = tagsContainer.GetArrayElementAtIndex(i).stringValue.Contains(effect.effectTag);
                }

                effect.Show();
            }

            public void ShowCustom()
            {
                ShowCustomValues(customs);
            }

            public void Show()
            {
                //avaiable tags
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                    EditorGUILayout.LabelField($"Available Built-in tags: {availableAppBuiltinTagsLongText}", EditorStyles.wordWrappedMiniLabel);

                    EditorGUILayout.LabelField("Default Appearance Effects", EditorStyles.miniBoldLabel);

                    if (Application.isPlaying)
                        GUI.enabled = false;

                    EditorGUILayout.PropertyField(tagsContainer, tagsContainerLabel, true);
                    GUI.enabled = false;
                    EditorGUILayout.LabelField("Write one tag for each element. In case it is not specified via appearance tags, the animator will apply the above effects to letters.", EditorStyles.wordWrappedMiniLabel);
                    GUI.enabled = true;
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.Space();

                //show default toggle
                GUI.backgroundColor = sectionsColor;
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                GUI.backgroundColor = Color.white;

                showBuiltIn = EditorGUILayout.BeginToggleGroup("Edit built-in effects", showBuiltIn);

                if (showBuiltIn)
                {

                    GUI.backgroundColor = sectionsColor;
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    GUI.backgroundColor = Color.white;

                    ShowEffect(ref appOffset);
                    ShowEffect(ref appSize);
                    ShowEffect(ref appFade);
                    ShowEffect(ref appVertExp);
                    ShowEffect(ref appHoriExp);
                    ShowEffect(ref appDiagExp);
                    ShowEffect(ref appRot);

                    EditorGUILayout.EndVertical();

                    //Errors check
                    if (appVertExp.isPresent && appHoriExp.isPresent)
                    {
                        EditorGUILayout.LabelField("[?] Consider using the 'size' effect insetead of both 'vertical' and 'horizontal' expand", EditorStyles.helpBox);
                    }
                }

                EditorGUILayout.EndToggleGroup();


                EditorGUILayout.EndVertical();
            }
        }

        [System.Serializable]
        class UserPreset
        {
            public bool show;
            public bool wantsToRemove;
            bool isAppearance;
            public string getName => isAppearance ? ('{' + effectTag.stringValue + '}') : ('<' + effectTag.stringValue + '>');

            SerializedProperty effectTag;

            EmissionCurveDrawer emission;

            FloatCurveDrawer movementX;
            FloatCurveDrawer movementY;
            FloatCurveDrawer movementZ;

            FloatCurveDrawer scaleX;
            FloatCurveDrawer scaleY;

            FloatCurveDrawer rotX;
            FloatCurveDrawer rotY;
            FloatCurveDrawer rotZ;

            ColorCurveDrawer color;

            public UserPreset(SerializedProperty parent, bool isAppearance)
            {
                effectTag = parent.FindPropertyRelative("effectTag");

                this.isAppearance = isAppearance;
                this.show = false;
                this.wantsToRemove = false;

                if (!isAppearance)
                {
                    emission = new EmissionCurveDrawer(parent.FindPropertyRelative("emission"));
                }

                movementX = new FloatCurveDrawer(parent.FindPropertyRelative("movementX"), "Movement X", true, isAppearance, Color.red);
                movementY = new FloatCurveDrawer(parent.FindPropertyRelative("movementY"), "Movement Y", true, isAppearance, Color.green);
                movementZ = new FloatCurveDrawer(parent.FindPropertyRelative("movementZ"), "Movement Z", true, isAppearance, Color.cyan);

                scaleX = new FloatCurveDrawer(parent.FindPropertyRelative("scaleX"), "Scale X", false, isAppearance, Color.red);
                scaleY = new FloatCurveDrawer(parent.FindPropertyRelative("scaleY"), "Scale Y", false, isAppearance, Color.green);

                rotX = new FloatCurveDrawer(parent.FindPropertyRelative("rotX"), "Rotation X", false, isAppearance, Color.red);
                rotY = new FloatCurveDrawer(parent.FindPropertyRelative("rotY"), "Rotation Y", false, isAppearance, Color.green);
                rotZ = new FloatCurveDrawer(parent.FindPropertyRelative("rotZ"), "Rotation Z", false, isAppearance, Color.cyan);

                color = new ColorCurveDrawer(parent.FindPropertyRelative("color"), "Color");
            }

            public void Show()
            {
                bool notLongEnough = !TextAnimator.IsTagLongEnough(effectTag.stringValue);
                //tag is short
                if (notLongEnough)
                {
                    GUI.backgroundColor = errorColor;
                }

                EditorGUI.BeginChangeCheck();
                if (Application.isPlaying)
                {
                    GUI.enabled = false;
                }
                EditorGUILayout.PropertyField(effectTag);
                if (notLongEnough)
                {
                    EditorGUILayout.LabelField("[!] This tag is too short.", EditorStyles.miniLabel);
                }

                if (Application.isPlaying)
                {
                    EditorGUILayout.LabelField("(You can't edit the tag IDs while in playmode.)", EditorStyles.centeredGreyMiniLabel);
                    GUI.enabled = true;
                }

                GUI.backgroundColor = Color.white;

                if (EditorGUI.EndChangeCheck())
                {
                    effectTag.stringValue = effectTag.stringValue.Replace(" ", "");
                }

                if (!isAppearance)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    EditorGUILayout.LabelField("--Emission--", EditorStyles.centeredGreyMiniLabel);
                    emission.Show();
                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField("--Movement--", EditorStyles.centeredGreyMiniLabel);
                movementX.Show();
                movementY.Show();
                movementZ.Show();
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField("--Scale--", EditorStyles.centeredGreyMiniLabel);
                scaleX.Show();
                scaleY.Show();
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField("--Rotation--", EditorStyles.centeredGreyMiniLabel);
                rotX.Show();
                rotY.Show();
                rotZ.Show();
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField("--Color--", EditorStyles.centeredGreyMiniLabel);
                color.Show();
                EditorGUILayout.EndVertical();

            }
        }



        class FloatCurveDrawer
        {
            string name;
            bool sizeDependant;
            SerializedProperty enabled;
            SerializedProperty amplitude;
            SerializedProperty curve;
            SerializedProperty charsTimeOffset;
            GUIContent curveLabel;
            Color curveColor;

            string curveDescription;

            public FloatCurveDrawer(SerializedProperty parent, string name, bool sizeDependant, bool isAppearance, Color curveColor)
            {
                curveLabel = new GUIContent(isAppearance ? "Decay" : "Curve");
                this.sizeDependant = sizeDependant;
                this.name = (sizeDependant ? "[!] " : "") + name;
                this.curveColor = curveColor;

                amplitude = parent.FindPropertyRelative("amplitude");
                curve = parent.FindPropertyRelative("curve");
                enabled = parent.FindPropertyRelative("enabled");
                charsTimeOffset = parent.FindPropertyRelative("charsTimeOffset");
                CalculateCurveStats();
            }

            void CalculateCurveStats()
            {
                curveDescription = $"Duration: {curve.animationCurveValue.GetDuration()}\n{curve.animationCurveValue.preWrapMode} - {curve.animationCurveValue.postWrapMode}";

            }

            public void Show()
            {
                enabled.boolValue = EditorGUILayout.ToggleLeft(name, enabled.boolValue);

                if (enabled.boolValue)
                {
                    if (sizeDependant)
                    {
                        EditorGUILayout.LabelField(alertTextSizeDep, EditorStyles.centeredGreyMiniLabel);
                    }

                    EditorStyles.wordWrappedMiniLabel.alignment = TextAnchor.MiddleCenter;
                    EditorGUILayout.LabelField(curveDescription, EditorStyles.wordWrappedMiniLabel);
                    EditorStyles.wordWrappedMiniLabel.alignment = TextAnchor.UpperLeft;

                    CalculateCurveStats(); //alyways calculating, because user may Undo and the duration could change


                    curve.animationCurveValue = EditorGUILayout.CurveField(curveLabel, curve.animationCurveValue, curveColor, Rect.zero);
                    //EditorGUILayout.PropertyField(curve, curveLabel);

                    EditorGUILayout.PropertyField(amplitude);
                    EditorGUILayout.PropertyField(charsTimeOffset);
                    GUILayout.Space(4);
                    //EditorGUILayout.Space();
                }
            }
        }

        class ColorCurveDrawer
        {

            string name;
            SerializedProperty enabled;
            SerializedProperty gradient;
            SerializedProperty duration;
            SerializedProperty charsTimeOffset;

            public ColorCurveDrawer(SerializedProperty parent, string name)
            {
                this.name = name;
                gradient = parent.FindPropertyRelative("gradient");
                enabled = parent.FindPropertyRelative("enabled");
                duration = parent.FindPropertyRelative("duration");
                charsTimeOffset = parent.FindPropertyRelative("charsTimeOffset");
            }

            public void Show()
            {
                enabled.boolValue = EditorGUILayout.ToggleLeft(name, enabled.boolValue);

                if (enabled.boolValue)
                {
                    EditorGUILayout.PropertyField(gradient);
                    EditorGUILayout.PropertyField(duration);
                    EditorGUILayout.PropertyField(charsTimeOffset);
                    EditorGUILayout.Space();
                }
            }

        }

        class EmissionCurveDrawer
        {
            string infoText;

            SerializedProperty cycles;
            SerializedProperty overrideDuration;
            SerializedProperty attackCurve;
            SerializedProperty decayCurve;
            SerializedProperty continueForever;

            public EmissionCurveDrawer(SerializedProperty parent)
            {
                this.infoText = string.Empty;
                attackCurve = parent.FindPropertyRelative("attackCurve");
                decayCurve = parent.FindPropertyRelative("decayCurve");
                cycles = parent.FindPropertyRelative("cycles");
                overrideDuration = parent.FindPropertyRelative("overrideDuration");
                continueForever = parent.FindPropertyRelative("continueForever");
            }

            public void Show()
            {
                infoText = $"Repeats { ((continueForever.boolValue || cycles.intValue <= 0 )? "forever" : (cycles.intValue + " time(s)"))}";

                GUI.enabled = false;
                EditorGUILayout.LabelField(infoText, EditorStyles.wordWrappedMiniLabel);
                EditorGUILayout.LabelField("Pro tip: Set the 'attack curve' keys to 1 to start the effect immediately", EditorStyles.wordWrappedMiniLabel);
                GUI.enabled = true;

                EditorGUILayout.CurveField(attackCurve, Color.yellow, Rect.zero);

                EditorGUILayout.PropertyField(continueForever);
                if (!continueForever.boolValue)
                {
                    EditorGUILayout.PropertyField(overrideDuration);
                    EditorGUILayout.CurveField(decayCurve, Color.yellow, Rect.zero);
                    EditorGUILayout.PropertyField(cycles);
                }

            }
        }

        #endregion

        #region Variables

        GUIContent easyIntegrationLabel = new GUIContent("Use Easy Integration");
        SerializedProperty triggerTypeWriter;
        SerializedProperty timeScale;

        SerializedProperty behaviorValues;
        SerializedProperty behavDef;
        SerializedProperty behCustomValues;
        SerializedProperty behPresetsArray;

        SerializedProperty appPresetsArray;


        SerializedProperty effectIntensity;
        SerializedProperty useDynamicScaling;
        SerializedProperty referenceFontSize;

        SerializedProperty defaultAppPreset;

        UserPreset[] behPresets = new UserPreset[0];
        UserPreset[] appPresets = new UserPreset[0];

        Effect behWiggle;
        Effect behWave;
        Effect behRotation;
        Effect behSwing;
        Effect behShake;
        Effect behSize;
        Effect behSlide;
        Effect behBounce;
        Effect behRainbow;
        Effect behFade;

        AppPreset appDefaultPreset;

        bool behShowBuiltin;
        bool behShowPresets;
        bool appShowPresets;

        #endregion

        enum Show
        {
            Behaviors,
            Appearances
        }

        Show showType;

        void CalculateBehPresets()
        {
            if (behPresets.Length != behPresetsArray.arraySize)
            {
                behPresets = new UserPreset[behPresetsArray.arraySize];
                for (int i = 0; i < behPresetsArray.arraySize; i++)
                {
                    behPresets[i] = new UserPreset(behPresetsArray.GetArrayElementAtIndex(i), false);
                }
            }

            if (appPresets.Length != appPresetsArray.arraySize)
            {
                appPresets = new UserPreset[appPresetsArray.arraySize];
                for (int i = 0; i < appPresetsArray.arraySize; i++)
                {
                    appPresets[i] = new UserPreset(appPresetsArray.GetArrayElementAtIndex(i), true);
                }
            }

        }

        private void OnEnable()
        {
            effectIntensity = serializedObject.FindProperty("effectIntensityMultiplier");
            referenceFontSize = serializedObject.FindProperty("referenceFontSize");
            useDynamicScaling = serializedObject.FindProperty("useDynamicScaling");

            triggerTypeWriter = serializedObject.FindProperty("triggerAnimPlayerOnChange");
            timeScale = serializedObject.FindProperty("timeScale");

            availableAppBuiltinTagsLongText = string.Empty;
            for (int i = 0; i < availableAppTags.Length; i++)
            {
                availableAppBuiltinTagsLongText += availableAppTags[i] + ", ";
            }

            behaviorValues = serializedObject.FindProperty("behaviorValues");
            behCustomValues = behaviorValues.FindPropertyRelative("customs");

            #region Default Behaviors
            behavDef = behaviorValues.FindPropertyRelative("defaults");

            behWiggle = new Effect(
                "Wiggle",
                TextAnimator.bh_Wiggle,
                true,
                behavDef,
                "wiggleAmplitude",
                "wiggleFrequency"
                ); ;

            behWave = new Effect(
                "Wave",
                TextAnimator.bh_Wave,
                true,
                behavDef,
                "waveFrequency",
                "waveAmplitude",
                "waveWaveSize"
                ); ;

            behRotation = new Effect(
                "Rotation",
                TextAnimator.bh_Rot,
                false,
                behavDef,
                "angleSpeed",
                "angleDiffBetweenChars"
                ); ;

            behSwing = new Effect(
                "Swing",
                TextAnimator.bh_Swing,
                false,
                behavDef,
                "swingAmplitude",
                "swingFrequency",
                "swingWaveSize"
                ); ;

            behShake = new Effect(
                "Shake",
                TextAnimator.bh_Shake,
                true,
                behavDef,
                "shakeStrength",
                "shakeDelay"
                ); ;

            behSize = new Effect(
                "Increase",
                TextAnimator.bh_Incr,
                false,
                behavDef,
                "sizeAmplitude",
                "sizeFrequency",
                "sizeWaveSize"
                ); ;

            behSlide = new Effect(
                "Slide",
                TextAnimator.bh_Slide,
                true,
                behavDef,
                "slideAmplitude",
                "slideFrequency",
                "slideWaveSize"
                ); ;

            behBounce = new Effect(
                "Bounce",
                TextAnimator.bh_Bounce,
                true,
                behavDef,
                "bounceAmplitude",
                "bounceFrequency",
                "bounceWaveSize"
                ); ;

            behRainbow = new Effect(
                "Rainbow",
                TextAnimator.bh_Rainb,
                false,
                behavDef,
                "hueShiftSpeed",
                "hueShiftWaveSize"
                ); ;

            behFade = new Effect(
                "Fade",
                TextAnimator.bh_Fade,
                false,
                behavDef,
                "fadeDelay"
                ); ;
            #endregion

            #region Default Appearances
            defaultAppPreset = serializedObject.FindProperty("appearancesContainer");
            appDefaultPreset = new AppPreset(defaultAppPreset);

            #endregion


            behPresetsArray = behaviorValues.FindPropertyRelative("presets");
            appPresetsArray = defaultAppPreset.FindPropertyRelative("values").FindPropertyRelative("presets");
            CalculateBehPresets();

        }

        void WhatToShowButton(string name, Show target)
        {
            if (showType == target)
                GUI.backgroundColor = selectedShowColor;

            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Height(30));

            if (GUILayout.Button(name, EditorStyles.boldLabel))
            {
                showType = target;
            }

            EditorGUILayout.EndVertical();

            GUI.backgroundColor = Color.white;
        }

        void WhatToShowTitle(string content)
        {
            EditorGUILayout.LabelField(content, EditorStyles.centeredGreyMiniLabel);
        }

        static void ShowCustomValues(SerializedProperty property)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.PropertyField(property, true);

            EditorGUILayout.EndVertical();
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(timeScale);

            EditorGUILayout.LabelField("With the below value, amplify the effects that are dependant on different sizes", EditorStyles.centeredGreyMiniLabel);
            EditorGUILayout.PropertyField(effectIntensity);

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(useDynamicScaling);
            if (EditorGUI.EndChangeCheck())
            {
                if (useDynamicScaling.boolValue)
                {
                    if (referenceFontSize.floatValue <= 0)
                    {
                        var tmproText = (target as TextAnimator)?.GetComponent<TMPro.TMP_Text>();
                        if (tmproText.text != null)
                        {
                            referenceFontSize.floatValue = tmproText.fontSize;
                        }
                    }
                }
            }

            if(useDynamicScaling.boolValue)
            EditorGUILayout.PropertyField(referenceFontSize);
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(triggerTypeWriter, easyIntegrationLabel);

            if (triggerTypeWriter.boolValue)
            {
                EditorGUILayout.LabelField("- Be sure to add a TextAnimatorPlayer component.\n", EditorStyles.wordWrappedMiniLabel);
            }
            else
            {
                EditorGUILayout.LabelField("[!!] If you're changing text directly from TMPro frequently (typewriter-like), please set this to true or performance may vary. Read docs for more <3", EditorStyles.wordWrappedMiniLabel);
            }

            EditorGUILayout.Space();

            //Chooses what to show
            EditorGUILayout.BeginHorizontal();

            WhatToShowButton("Edit Behaviors", Show.Behaviors);
            WhatToShowButton("Edit Appearances", Show.Appearances);

            EditorGUILayout.EndVertical();

            void ShowPresets(ref UserPreset[] userPresets, ref bool canShow, ref SerializedProperty arrayProperty)
            {

                //Presets
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                EditorGUI.BeginChangeCheck();
                canShow = EditorGUILayout.BeginToggleGroup($"Edit presets effects [{arrayProperty.arraySize} created]", canShow);

                //Resets removal confirmation
                if (EditorGUI.EndChangeCheck())
                {
                    for (int i = 0; i < userPresets.Length; i++)
                    {
                        userPresets[i].wantsToRemove = false;
                    }
                }

                if (canShow)
                {
                    //Checks for error
                    if (userPresets.Length != arrayProperty.arraySize)
                    {
                        CalculateBehPresets();
                    }

                    for (int i = 0; i < userPresets.Length; i++)
                    {

                        if (userPresets[i].show)
                            GUI.backgroundColor = expandedColor;
                        else
                            GUI.backgroundColor = notExpandedColor;

                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                        GUI.backgroundColor = Color.white;

                        if (userPresets[i].show)
                        {
                            EditorGUILayout.BeginHorizontal();
                        }

                        if (GUILayout.Button($"-> {userPresets[i].getName}", userPresets[i].show ? EditorStyles.boldLabel : EditorStyles.label))
                        {
                            userPresets[i].show = !userPresets[i].show;
                            userPresets[i].wantsToRemove = false;
                        }

                        if (userPresets[i].show)
                        {
                            if (Application.isPlaying)
                                GUI.enabled = false;

                            if (GUILayout.Button(userPresets[i].wantsToRemove ? "Confirm?" : "Remove?", EditorStyles.miniButtonRight, GUILayout.Width(85)))
                            {
                                //Confirms remove
                                if (userPresets[i].wantsToRemove)
                                {
                                    arrayProperty.DeleteArrayElementAtIndex(i);
                                    CalculateBehPresets();
                                    break;
                                }
                                else //asks for remove
                                {
                                    userPresets[i].wantsToRemove = true;
                                }
                            }

                            GUI.enabled = true;

                            EditorGUILayout.EndHorizontal();

                            GUI.backgroundColor = Color.white;

                            userPresets[i].Show();
                        }


                        EditorGUILayout.EndToggleGroup();
                    }

                    if (Application.isPlaying)
                        GUI.enabled = false;

                    if (GUILayout.Button("Add new", EditorStyles.miniButtonMid))
                    {
                        arrayProperty.InsertArrayElementAtIndex(Mathf.Clamp(arrayProperty.arraySize - 1, 0, arrayProperty.arraySize));
                        CalculateBehPresets();
                    }

                    GUI.enabled = true;
                }

                EditorGUILayout.EndToggleGroup();


                EditorGUILayout.EndVertical();

            }

            //Shows based on the type
            switch (showType)
            {
                case Show.Behaviors:

                    WhatToShowTitle("Currently showing Behavior Effects");

                    #region Default Behavior Values

                    GUI.backgroundColor = sectionsColor;
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    GUI.backgroundColor = Color.white;

                    behShowBuiltin = EditorGUILayout.BeginToggleGroup("Edit built-in effects", behShowBuiltin);

                    if (behShowBuiltin)
                    {
                        behWiggle.Show();
                        behShake.Show();
                        behWave.Show();
                        behSlide.Show();
                        behBounce.Show();

                        behRotation.Show();
                        behSwing.Show();
                        behSize.Show();
                        behRainbow.Show();
                        behFade.Show();
                    }

                    EditorGUILayout.EndToggleGroup();


                    EditorGUILayout.EndVertical();


                    #endregion

                    ShowPresets(ref behPresets, ref behShowPresets, ref behPresetsArray);

                    ShowCustomValues(behCustomValues);

                    break;

                case Show.Appearances:
                    WhatToShowTitle("Currently showing Appearance Effects");

                    appDefaultPreset.Show();

                    ShowPresets(ref appPresets, ref appShowPresets, ref appPresetsArray);

                    appDefaultPreset.ShowCustom();

                    break;
            }

            if (serializedObject.hasModifiedProperties)
                serializedObject.ApplyModifiedProperties();
        }

    }

}
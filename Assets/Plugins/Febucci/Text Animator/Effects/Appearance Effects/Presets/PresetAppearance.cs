using UnityEngine;
namespace Febucci.UI.Core
{
    internal class PresetAppearance : AppearanceBase
    {
        bool enabled;

        //management
        Matrix4x4 matrix;
        Vector3 movementVec;
        Vector3 scaleVec;
        Vector3 offset;
        Vector3 rotationEuler;
        Quaternion rotationQua;

        bool hasTransformEffects;

        //movement
        bool setMovement;
        EffectEvaluator movementX;
        EffectEvaluator movementY;
        EffectEvaluator movementZ;

        //scale
        bool setScale;
        float scaleXDuration;
        float scaleYDuration;
        EffectEvaluator scaleX;
        EffectEvaluator scaleY;

        //rotation
        bool setRotation;
        EffectEvaluator rotX;
        EffectEvaluator rotY;
        EffectEvaluator rotZ;

        bool setColor;
        Color32 color;
        ColorCurve colorCurve;

        public override void SetDefaultValues(AppearanceDefaultValues data)
        {
            int GetPresetIndex()
            {
                for (int i = 0; i < data.presets.Length; i++)
                {
                    if (data.presets[i].effectTag == effectTag)
                        return i;
                }
                return -1;
            }

            showDuration = 0;

            int presetIndex = GetPresetIndex();
            if (presetIndex >= 0) //found preset
            {
                movementVec = Vector3.zero;

                data.presets[presetIndex].Initialize();
                showDuration = data.presets[presetIndex].GetMaxDuration();

                setMovement = data.presets[presetIndex].movementX.enabled || data.presets[presetIndex].movementY.enabled || data.presets[presetIndex].movementZ.enabled;
                if (setMovement)
                {
                    movementX = data.presets[presetIndex].movementX;
                    movementY = data.presets[presetIndex].movementY;
                    movementZ = data.presets[presetIndex].movementZ;
                }

                scaleVec = Vector3.one;
                setScale = data.presets[presetIndex].scaleX.enabled || data.presets[presetIndex].scaleY.enabled;
                if (setScale)
                {
                    scaleX = data.presets[presetIndex].scaleX;
                    scaleY = data.presets[presetIndex].scaleY;

                    scaleVec.z = 1;

                    scaleXDuration = scaleX.GetDuration();
                    scaleYDuration = scaleY.GetDuration();
                }

                setRotation = data.presets[presetIndex].rotX.enabled || data.presets[presetIndex].rotY.enabled || data.presets[presetIndex].rotZ.enabled;
                rotationQua = Quaternion.identity;
                if (setRotation)
                {
                    rotX = data.presets[presetIndex].rotX;
                    rotY = data.presets[presetIndex].rotY;
                    rotZ = data.presets[presetIndex].rotZ;
                }

                hasTransformEffects = setMovement || setRotation || setScale;

                setColor = data.presets[presetIndex].color.enabled;
                if (setColor)
                {
                    colorCurve = data.presets[presetIndex].color;
                    colorCurve.Initialize();
                }

                enabled = hasTransformEffects || setColor;
            }
        }

        public override void ApplyEffect(ref CharacterData data, int charIndex)
        {
            if (!enabled)
                return;

            if (hasTransformEffects)
            {
                offset = (data.vertices[0] + data.vertices[2]) / 2f;

                #region Movement

                if (setMovement)
                {
                    movementVec.x = movementX.Evaluate(data.passedTime, charIndex);
                    movementVec.y = movementY.Evaluate(data.passedTime, charIndex);
                    movementVec.z = movementZ.Evaluate(data.passedTime, charIndex);

                    movementVec *= effectIntensity;

                }
                #endregion

                #region Scale
                if (setScale)
                {
                    scaleVec.x = Mathf.Lerp(scaleX.Evaluate(data.passedTime, charIndex), 1, data.passedTime / scaleXDuration);
                    scaleVec.y = Mathf.Lerp(scaleY.Evaluate(data.passedTime, charIndex), 1, data.passedTime / scaleYDuration);
                }

                #endregion

                #region Rotation
                if (setRotation)
                {

                    rotationEuler.x = rotX.Evaluate(data.passedTime, charIndex);
                    rotationEuler.y = rotY.Evaluate(data.passedTime, charIndex);
                    rotationEuler.z = rotZ.Evaluate(data.passedTime, charIndex);

                    rotationQua.eulerAngles = rotationEuler;
                }
                #endregion

                matrix.SetTRS(movementVec, rotationQua, scaleVec);

                for (byte i = 0; i < data.vertices.Length; i++)
                {
                    data.vertices[i] -= offset;
                    data.vertices[i] = matrix.MultiplyPoint3x4(data.vertices[i]);
                    data.vertices[i] += offset;
                }
            }

            if (setColor)
            {
                color = colorCurve.GetColor(data.passedTime, charIndex);
                data.colors.LerpUnclamped(color, 1 - data.passedTime / showDuration);
            }

        }
    }
}
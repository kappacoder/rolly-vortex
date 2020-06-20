using UnityEngine;
using UnityEditor;

namespace Febucci.UI.Core
{
    [CustomPropertyDrawer(typeof(CharsDisplayTimeAttribute))]
    public class CharsDisplayTimeAttributeDrawer : PropertyDrawer
    {
        const float minWaitTime = 0.01f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect valueRect = new Rect(position);

            valueRect.width = Mathf.Clamp(valueRect.width - 104, 162, valueRect.width); //clamps to avoid trimming the value field

            Rect perSecLabel = new Rect(valueRect);
            perSecLabel.x += perSecLabel.width + 2;
            perSecLabel.width = position.width - valueRect.width - 4;

            switch (property.propertyType)
            {
                case SerializedPropertyType.Float:

                    EditorGUI.PropertyField(valueRect, property, label);

                    if (property.floatValue < minWaitTime)
                        property.floatValue = minWaitTime;

                    float charPerSecond = 1 / property.floatValue;

                    EditorGUI.LabelField(perSecLabel, $"{(charPerSecond).ToString("F1")}/s  {(60 * charPerSecond).ToString("F1")}/m");

                    break;


                default: //unsupported, fallback to the default OnGUI
                    EditorGUI.PropertyField(position, property, label);
                    return;
            }

        }

    }

}
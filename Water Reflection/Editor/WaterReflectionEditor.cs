using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WaterReflection))]
public class WaterReflectionEditor : Editor
{
    private WaterReflection waterReflection;
    private SerializedProperty spriteRenderer;
    private SerializedProperty camera;
    private SerializedProperty waterShader;
    private SerializedProperty waterTexture;

    private void OnEnable()
    {
        waterReflection = target as WaterReflection;

        waterReflection.UpdateCamera();

        spriteRenderer = serializedObject.FindProperty("spriteRenderer");
        camera = serializedObject.FindProperty("camera");
        waterShader = serializedObject.FindProperty("waterShader");
        waterTexture = serializedObject.FindProperty("waterTexture");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();

        GUILayout.Space(10);
        EditorGUILayout.PropertyField(camera, new GUIContent("Camera", "Camera used to capture the reflection scene."));
        if (camera.objectReferenceValue == null)
        {
            EditorGUILayout.HelpBox("You must assign the camera.", MessageType.Error);
        }
        else
        {
            waterReflection.verticalCameraOffset = EditorGUILayout.FloatField(new GUIContent("Vertical Camera Offset", "By default, the camera used to capture the reflection's scene is placed just above the sprite renderer. You can adjust camera height by modifying this offset."), waterReflection.verticalCameraOffset);
        }

        GUILayout.Space(10);
        EditorGUILayout.PropertyField(spriteRenderer, new GUIContent("Sprite Renderer", "Sprite that will receive the water shader."));
        if (spriteRenderer.objectReferenceValue == null)
        {
            EditorGUILayout.HelpBox("You must assign the spriteRenderer.", MessageType.Error);
        }
        else
        {
            waterReflection.pixelsPerUnit = EditorGUILayout.IntField(new GUIContent("Pixels Per Unit", "Resolution of the reflection's texture."), waterReflection.pixelsPerUnit);
            waterReflection.verticalSqueezeRatio = EditorGUILayout.FloatField(new GUIContent("Vertical Squeeze Ratio", "How much is the reflection squeezed vertically. 1 : no squeeze, > 1 : smaller reflection, < 1 : taller reflection."), waterReflection.verticalSqueezeRatio);
        }

        GUILayout.Space(10);
        GUILayout.Label("Shader Parameters", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(waterTexture, new GUIContent("Water Texture", "Water texture used to simulate water."));
        EditorGUILayout.PropertyField(waterShader, new GUIContent("Water Shader", "Shader used to simulate water."));
        waterReflection.color = EditorGUILayout.ColorField(new GUIContent("Water Color", "Water's color."), waterReflection.color);
        waterReflection.turbulencesStrength = EditorGUILayout.FloatField(new GUIContent("Turbulences Strength", "Strength of water's turbulences."), waterReflection.turbulencesStrength);
        waterReflection.waterSpeed = EditorGUILayout.FloatField(new GUIContent("Water Speed", "Water's speed."), waterReflection.waterSpeed);
        waterReflection.refraction = EditorGUILayout.FloatField(new GUIContent("Refraction/Reflection", "How much refraction (> 0) or Reflection(< 0) patterns are visible."), waterReflection.refraction);
        waterReflection.noiseScale = EditorGUILayout.FloatField(new GUIContent("Noise Scale", "Scale of noise. Used to move and distord turbulences in a more realistic way."), waterReflection.noiseScale);
        waterReflection.noisePower = EditorGUILayout.FloatField(new GUIContent("Noise Power", "Power given to noise. Used to move and distord turbulences in a more realistic way."), waterReflection.noisePower);
        waterReflection.waveInversedScale = EditorGUILayout.Vector2Field(new GUIContent("Pattern Size Reduction", "Wave patterns inversed scale."), waterReflection.waveInversedScale);
        EditorGUI.indentLevel--;

        bool updateCamera = GUILayout.Button(new GUIContent("Force Visual Update", "If for some reason parameters are not applied, you can force them by clicking this button."));
        if (updateCamera)
        {
            waterReflection.UpdateCamera();
        }
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(waterReflection);
            waterReflection.UpdateCamera();
        }
        serializedObject.ApplyModifiedProperties();
    }
}

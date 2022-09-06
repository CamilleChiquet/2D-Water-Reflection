using UnityEngine;

public class WaterReflection : MonoBehaviour
{
    public Texture waterTexture;
    // Sprite that will receive the water shader.
    public SpriteRenderer spriteRenderer;
    // Camera used to capture the reflection scene.
    public Camera camera;
    // Resolution of the reflection's texture.
    public int pixelsPerUnit = 32;
    // Water's color.
    public Color color;
    // How much is the reflection squeezed vertically. 1 : no squeeze, > 1 : smaller reflection, < 1 : taller reflection.
    public float verticalSqueezeRatio = 1;
    // By default, the camera used to capture the reflection's scene is placed just above the sprite renderer. You can adjust camera height by modifying this offset.
    public float verticalCameraOffset = 0;
    // ======== Shader parameters ========
    // Shader used to simulate water.
    public Shader waterShader;
    // Strength of water's turbulences.
    public float turbulencesStrength = 0.4f;
    // Water's speed.
    public float waterSpeed = 0.01f;
    // How much refraction (> 0) or Reflection(< 0) patterns are visible.
    public float refraction = 0.5f;
    // Scale of noise. Used to move and distord turbulences in a more realistic way.
    public float noiseScale = 10;
    // Power given to noise. Used to move and distord turbulences in a more realistic way.
    public float noisePower = 0.03f;
    // Wave patterns inversed scale.
    public Vector2 waveInversedScale = Vector2.one;

    private RenderTexture renderTexture;
    private Material waterMaterial;

    public void Awake()
    {
        // Unity modify camera's aspect when game starts (according to your screen dimensions). So we call UpdateCamera here to override it.
        UpdateCamera();
    }

    public void UpdateCamera()
    {
        if (spriteRenderer != null && camera != null && waterShader != null)
        {
            renderTexture = new RenderTexture((int)spriteRenderer.bounds.size.x * pixelsPerUnit, (int)spriteRenderer.bounds.size.y * pixelsPerUnit, 1);
            renderTexture.name = "Render Texture";

            float cameraHeight = spriteRenderer.bounds.size.y * verticalSqueezeRatio;
            camera.aspect = spriteRenderer.bounds.size.x / cameraHeight;
            camera.orthographicSize = verticalSqueezeRatio * spriteRenderer.bounds.size.y / 2;
            camera.targetTexture = renderTexture;
            Vector3 cameraPosition = camera.transform.position;
            cameraPosition.x = spriteRenderer.transform.position.x;
            cameraPosition.y = verticalCameraOffset + spriteRenderer.transform.position.y + spriteRenderer.bounds.size.y / 2 + cameraHeight / 2;
            camera.transform.position = cameraPosition;

            waterMaterial = new Material(waterShader);
            waterMaterial.SetTexture("_WaterTex", waterTexture);
            waterMaterial.SetTexture("_RenderTex", renderTexture);
            waterMaterial.SetColor("_Color", color);
            waterMaterial.SetFloat("_TurbulencesStrength", turbulencesStrength);
            waterMaterial.SetFloat("_WaterSpeed", waterSpeed);
            waterMaterial.SetFloat("_Refraction", refraction);
            waterMaterial.SetFloat("_NoiseScale", noiseScale);
            waterMaterial.SetFloat("_NoisePower", noisePower);
            waterMaterial.SetVector("_PatternSizeReduction", waveInversedScale);

            spriteRenderer.material = waterMaterial;
        }
    }
}

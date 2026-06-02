using UnityEngine;
using UnityEditor;
using System.IO;

public class PrefabToImageTransparent : EditorWindow
{
    private GameObject prefab;
    private int imageWidth = 1024;
    private int imageHeight = 1024;
    private float cameraDistance = 3f;
    private Vector3 cameraAngle = new Vector3(0, 0, 0);
    private string outputFileName = "NPC_Transparent";
    private Color backgroundColor = new Color(0, 0, 0, 0);

    [MenuItem("Tools/Prefab to PNG (Transparent)")]
    public static void ShowWindow()
    {
        GetWindow<PrefabToImageTransparent>("Prefab to PNG Transparent");
    }

    private void OnGUI()
    {
        GUILayout.Label("Render Prefab to Transparent PNG", EditorStyles.boldLabel);
        
        prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false);
        
        imageWidth = EditorGUILayout.IntField("Image Width", imageWidth);
        imageHeight = EditorGUILayout.IntField("Image Height", imageHeight);
        
        cameraDistance = EditorGUILayout.Slider("Camera Distance", cameraDistance, 0.5f, 10f);
        cameraAngle = EditorGUILayout.Vector3Field("Camera Angle", cameraAngle);
        
        outputFileName = EditorGUILayout.TextField("Output File Name", outputFileName);
        
        GUILayout.Space(10);
        
        if (GUILayout.Button("Render to Transparent PNG", GUILayout.Height(40)))
        {
            if (prefab != null)
            {
                RenderPrefabToPNG();
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "Please assign a prefab first!", "OK");
            }
        }
        
        GUILayout.Space(10);
        GUILayout.Label("PNG saved to Assets/RenderedImages/", EditorStyles.helpBox);
    }

    private void RenderPrefabToPNG()
    {
        // Store original scene
        var originalScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        
        // Create new temporary scene
        var tempScene = UnityEditor.SceneManagement.EditorSceneManager.NewScene(
            UnityEditor.SceneManagement.NewSceneSetup.EmptyScene, 
            UnityEditor.SceneManagement.NewSceneMode.Additive
        );
        UnityEditor.SceneManagement.EditorSceneManager.SetActiveScene(tempScene);
        
        // Instantiate the prefab
        GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        
        // Calculate bounds
        Bounds bounds = CalculateBounds(instance);
        Vector3 center = bounds.center;
        
        // Center the model
        instance.transform.position = -center;
        
        // Create camera
        GameObject cameraObj = new GameObject("RenderCamera");
        Camera camera = cameraObj.AddComponent<Camera>();
        
        // IMPORTANT: These settings ensure transparent background
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(0, 0, 0, 0);
        camera.orthographic = false;
        
        // Position camera for front view
        float maxSize = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);
        float distance = maxSize * cameraDistance;
        
        cameraObj.transform.position = new Vector3(0, bounds.size.y * 0.4f, distance);
        cameraObj.transform.rotation = Quaternion.Euler(cameraAngle);
        cameraObj.transform.LookAt(Vector3.zero);
        
        // Add lighting
        GameObject lightObj = new GameObject("Key Light");
        Light keyLight = lightObj.AddComponent<Light>();
        keyLight.type = LightType.Directional;
        keyLight.intensity = 1f;
        keyLight.color = Color.white;
        lightObj.transform.rotation = Quaternion.Euler(50, -30, 0);
        
        GameObject fillLightObj = new GameObject("Fill Light");
        Light fillLight = fillLightObj.AddComponent<Light>();
        fillLight.type = LightType.Directional;
        fillLight.intensity = 0.4f;
        fillLight.color = new Color(0.8f, 0.8f, 1f);
        fillLightObj.transform.rotation = Quaternion.Euler(-20, 150, 0);
        
        // Create RenderTexture with alpha channel
        RenderTexture renderTexture = new RenderTexture(imageWidth, imageHeight, 24, RenderTextureFormat.ARGB32);
        renderTexture.antiAliasing = 8;
        camera.targetTexture = renderTexture;
        
        // Render the camera
        camera.Render();
        
        // Read the pixels
        RenderTexture.active = renderTexture;
        Texture2D screenshot = new Texture2D(imageWidth, imageHeight, TextureFormat.ARGB32, false);
        screenshot.ReadPixels(new Rect(0, 0, imageWidth, imageHeight), 0, 0, false);
        screenshot.Apply();
        RenderTexture.active = null;
        
        // Encode to PNG
        byte[] bytes = screenshot.EncodeToPNG();
        
        // Create directory
        string outputDir = "Assets/RenderedImages";
        if (!Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }
        
        // Save file
        string outputPath = Path.Combine(outputDir, outputFileName + ".png");
        File.WriteAllBytes(outputPath, bytes);
        
        // Cleanup
        DestroyImmediate(screenshot);
        DestroyImmediate(renderTexture);
        
        // Close temporary scene
        UnityEditor.SceneManagement.EditorSceneManager.CloseScene(tempScene, true);
        UnityEditor.SceneManagement.EditorSceneManager.SetActiveScene(originalScene);
        
        // Refresh
        AssetDatabase.Refresh();
        
        // Import settings to ensure alpha is preserved
        TextureImporter importer = AssetImporter.GetAtPath(outputPath) as TextureImporter;
        if (importer != null)
        {
            importer.alphaIsTransparency = true;
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            AssetDatabase.ImportAsset(outputPath, ImportAssetOptions.ForceUpdate);
        }
        
        EditorUtility.DisplayDialog("Success!", 
            $"Transparent PNG saved to:\n{outputPath}\n\nMake sure to check 'Alpha Is Transparency' in import settings!", 
            "OK");
        
        // Ping the file in project window
        Object obj = AssetDatabase.LoadAssetAtPath<Object>(outputPath);
        EditorGUIUtility.PingObject(obj);
        
        Debug.Log($"Transparent render saved: {outputPath}");
    }

    private Bounds CalculateBounds(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        
        if (renderers.Length == 0)
        {
            return new Bounds(obj.transform.position, Vector3.one);
        }
        
        Bounds bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }
        
        return bounds;
    }
}
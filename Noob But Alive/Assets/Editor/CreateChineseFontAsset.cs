// 放在 Assets/Editor/ 下：编辑器工具，不参与打包
// 作用：把思源黑体做成 TMP 的「动态」字体资产，并挂到当前场景所有 TMP 文本上
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.LowLevel;
using TMPro;

public static class CreateChineseFontAsset
{
    // 源字体（由外部放入；首次运行会自动拷进项目）
    const string SourceOtfInWorkspace = @"D:\dshstudy\.tmp\fonts\SourceHanSansSC-Regular.otf";
    const string FontDir = "Assets/Fonts";
    const string OtfAssetPath = "Assets/Fonts/SourceHanSansSC-Regular.otf";
    const string FontAssetPath = "Assets/Fonts/SourceHanSansSC SDF.asset";
    const string TmpSettingsPath = "Assets/TextMesh Pro/Resources/TMP Settings.asset";

    [MenuItem("Tools/大肥鱼/生成中文字体资产并应用")]
    public static void Run()
    {
        // ---------- 1. 确保 otf 在项目里 ----------
        if (!Directory.Exists(FontDir)) Directory.CreateDirectory(FontDir);
        if (!File.Exists(OtfAssetPath))
        {
            if (!File.Exists(SourceOtfInWorkspace))
            {
                Debug.LogError("[字体] 找不到源字体：" + SourceOtfInWorkspace);
                return;
            }
            File.Copy(SourceOtfInWorkspace, OtfAssetPath, true);
            AssetDatabase.Refresh();
            Debug.Log("[字体] 已把 otf 拷入 " + OtfAssetPath);
        }

        AssetDatabase.ImportAsset(OtfAssetPath, ImportAssetOptions.ForceUpdate);
        Font sourceFont = AssetDatabase.LoadAssetAtPath<Font>(OtfAssetPath);
        if (sourceFont == null)
        {
            Debug.LogError("[字体] otf 没导入成 Font 资产，检查 " + OtfAssetPath);
            return;
        }

        // ---------- 2. 生成 / 复用 TMP 字体资产 ----------
        TMP_FontAsset fa = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(FontAssetPath);
        if (fa == null)
        {
            // 90pt 采样、5px padding、SDFAA、1024x1024、Dynamic、支持多图集
            fa = TMP_FontAsset.CreateFontAsset(
                sourceFont, 90, 5, GlyphRenderMode.SDFAA,
                1024, 1024, AtlasPopulationMode.Dynamic, true);

            if (fa == null)
            {
                Debug.LogError("[字体] CreateFontAsset 返回 null");
                return;
            }

            fa.name = "SourceHanSansSC SDF";

            // 借用默认字体的 shader，防止材质变粉红
            if (TMP_Settings.defaultFontAsset != null && TMP_Settings.defaultFontAsset.material != null)
                fa.material.shader = TMP_Settings.defaultFontAsset.material.shader;

            AssetDatabase.CreateAsset(fa, FontAssetPath);

            if (fa.atlasTextures != null)
            {
                foreach (Texture2D tex in fa.atlasTextures)
                {
                    if (tex == null) continue;
                    tex.name = "Atlas";
                    AssetDatabase.AddObjectToAsset(tex, fa);
                }
            }
            if (fa.material != null)
            {
                fa.material.name = "SourceHanSansSC SDF Material";
                AssetDatabase.AddObjectToAsset(fa.material, fa);
            }

            AssetDatabase.SaveAssets();
            Debug.Log("[字体] 已生成 " + FontAssetPath + "（Dynamic + Multi Atlas）");
        }
        else
        {
            Debug.Log("[字体] 复用已有资产 " + FontAssetPath);
        }

        // ---------- 3. 换到当前场景所有 TMP 文本（含未激活） ----------
        int changed = 0;
        Scene dirtyScene = default(Scene);

        TextMeshProUGUI[] uguiTexts = Object.FindObjectsOfType<TextMeshProUGUI>(true);
        for (int i = 0; i < uguiTexts.Length; i++)
        {
            TextMeshProUGUI t = uguiTexts[i];
            if (t == null || t.font == fa) continue;
            Undo.RecordObject(t, "换中文字体");
            t.font = fa;
            EditorUtility.SetDirty(t);
            if (t.gameObject.scene.IsValid()) dirtyScene = t.gameObject.scene;
            changed++;
        }

        TextMeshPro[] worldTexts = Object.FindObjectsOfType<TextMeshPro>(true);
        for (int i = 0; i < worldTexts.Length; i++)
        {
            TextMeshPro t = worldTexts[i];
            if (t == null || t.font == fa) continue;
            Undo.RecordObject(t, "换中文字体");
            t.font = fa;
            EditorUtility.SetDirty(t);
            if (t.gameObject.scene.IsValid()) dirtyScene = t.gameObject.scene;
            changed++;
        }

        if (dirtyScene.IsValid()) EditorSceneManager.MarkSceneDirty(dirtyScene);

        // ---------- 4. 设为 TMP 默认字体（以后新建文本自动是中文） ----------
        TMP_Settings settings = AssetDatabase.LoadAssetAtPath<TMP_Settings>(TmpSettingsPath);
        if (settings != null)
        {
            SerializedObject so = new SerializedObject(settings);
            SerializedProperty p = so.FindProperty("m_defaultFontAsset");
            if (p != null && p.objectReferenceValue != fa)
            {
                p.objectReferenceValue = fa;
                so.ApplyModifiedPropertiesWithoutUndo();
                AssetDatabase.SaveAssets();
                Debug.Log("[字体] 已设为 TMP 默认字体");
            }
        }
        else
        {
            Debug.LogWarning("[字体] 没找到 " + TmpSettingsPath + "，跳过默认字体设置");
        }

        Debug.Log("[字体] 完成：本场景替换 " + changed + " 个文本。请 Ctrl+S 保存场景，再 Play 验证。");
    }
}
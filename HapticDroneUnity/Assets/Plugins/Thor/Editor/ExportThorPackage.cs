using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ExportThorPackage
{
    [MenuItem("Ultraleap/Export Thor Package")]
    public static void Export()
    {
        string path = EditorUtility.OpenFolderPanel("Select a folder where the package will be exported", "", "");
        AssetDatabase.ExportPackage("Assets/Plugins/Thor", $"{path}/Thor.unitypackage", ExportPackageOptions.Recurse | ExportPackageOptions.Interactive);
    }
}

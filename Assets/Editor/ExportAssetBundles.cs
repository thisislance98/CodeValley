// C# Example
// Builds an asset bundle from the selected objects in the project view.
// Once compiled go to "Menu" -> "Assets" and select one of the choices
// to build the Asset Bundle

using UnityEngine;
using UnityEditor;
using Parse;
using System.Threading.Tasks;

public class ExportAssetBundles {
	[MenuItem("Assets/Build AssetBundle From Selection - Track dependencies")]
	static void ExportResource () {
		// Bring up save panel
		string path = EditorUtility.SaveFilePanel ("Save Resource", "", "New Resource", "unity3d");
		if (path.Length != 0) {
			// Build the resource file from the active selection.
			Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
			BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, 
			                               BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets);
			Selection.objects = selection;
		}
	}
	[MenuItem("Assets/Build AssetBundle From Selection - No dependency tracking")]
	static void ExportResourceNoTrack () {

		ParseClient.Initialize("12zbQzuJZSFdRZ2TLTy195huFNPthww1gT0uSsgX","dkpQN45spUUwQ6Z8FWeYgk1BfuOVN2pbhwPtIa5i");
		byte[] data = System.Text.Encoding.UTF8.GetBytes("Working at Parse is great!");
		ParseFile file = new ParseFile("resume.txt", data);

		Task saveTask = file.SaveAsync();

		while (saveTask.IsCompleted == false)
		{
			Debug.Log("saving");
		}

		Debug.Log("done");

		// Bring up save panel
//		string path = EditorUtility.SaveFilePanel ("Save Resource", "", "New Resource", "unity3d");
//		if (path.Length != 0) {
//			// Build the resource file from the active selection.
//			BuildPipeline.BuildAssetBundle(Selection.activeObject, Selection.objects, path);
//		}
	}
}
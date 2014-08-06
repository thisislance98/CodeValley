#pragma strict
#pragma downcast
 
import System;
import System.IO;
import System.Reflection;
import System.CodeDom.Compiler;
 
static class DynamicCodeCompiler {
    //Adds a menu item with a shortcut
    @MenuItem ("Compiler/Compile Selected #C")
    function CompileSelected():void {
        //only compile Script objects
        if (Selection.activeObject && Selection.activeObject.GetType().IsAssignableFrom(MonoScript)) {
            var target:MonoScript = Selection.activeObject as MonoScript;
 
            //reference directory separator
            var sep:String = Path.DirectorySeparatorChar.ToString();
            var path:String = Application.dataPath + sep + "Plugins" + sep + 
                Path.GetFileName(target.name) + ".dll";
 
            GenerateDll(target.text, path);
        }
        else {
            Debug.Log("Selected object is not compilable");
        }
    }
 
    function GenerateDll(source:String, destination:String):void {
        var results:CompilerResults = CreateAssemblyFromSource("C#", source);
 
        //debug compiler errors
        if (results.Errors.HasErrors) {
            for (var error:CompilerError in results.Errors) {
                Debug.LogError("Dynamic Code Compiler ERROR: " + error);
            }
            Debug.Log("Could not generate dll");
        }
        else {
            //Create Plugins directory if it doesn't exist
            Directory.CreateDirectory(Path.GetDirectoryName(destination));
 
            //Move dll to Plugins folder, delete duplicates
            if (File.Exists(destination))
                File.Delete(destination);
            File.Move(results.PathToAssembly, destination);
 
            //notify of completion and import new dll
            Debug.Log("Success! Code was compiled and moved to: " + destination);
            AssetDatabase.Refresh();
        }
    }
 
    function CreateAssemblyFromSource(type:String, source:String):CompilerResults {
        //Create default provider parameters
        var parameters:CompilerParameters = new CompilerParameters();
        parameters.GenerateExecutable = false;
        parameters.OutputAssembly = "temp_build.dll";
 
        //reference ALL assemblies!!
        for (var assembly:Assembly in AppDomain.CurrentDomain.GetAssemblies()) {
            parameters.ReferencedAssemblies.Add(assembly.Location);
        }
 
        //compile and return results
        return CodeDomProvider.CreateProvider(type).CompileAssemblyFromSource(parameters, source);
    }
}
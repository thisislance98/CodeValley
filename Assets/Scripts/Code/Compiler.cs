using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//using Mono.CSharp;
using Microsoft.CSharp;
using System;
using System.Reflection;
using System.CodeDom.Compiler;

// echo 'export PATH=/Applications/Unity/Unity.app/Contents/Frameworks/Mono/bin:$PATH' >> ~/.profile
//

public class Compiler : Photon.MonoBehaviour
{
	public static Compiler Instance;
	public static Dictionary<string,Type> CompiledTypes = new Dictionary<string, Type>();

	int _revision = 0;

	Type _currentType;


	void Awake()
	{
		Instance = this;

	}

	[RPC]
	public void CompilePeer(string code, string className, int castViewId)
	{
		code = code.Replace(className,className + castViewId);
		string typeName = CompileSpell (code,castViewId);

		PhotonNetwork.networkingPeer.GetPhotonView(castViewId).gameObject.SendMessage("SetSpellTypeName",typeName);

	}

	public string CompileSpell(string code, int casterViewId)
	{
		PhotonView casterView = PhotonNetwork.networkingPeer.GetPhotonView(casterViewId);

		Debug.Log("compiling code: " + code);

		bool success = CompileCSharpCode(code, "./test" + _revision + ".dll"); //"using UnityEngine; class Test : MonoBehaviour { public void Start(){ UnityEngine.Debug.Log(\"Test made\"); } public void Update(){ UnityEngine.Debug.Log(\"Test called\"); } }", "./test.dll");
		
		if (success)
		{
			/*AppDomainSetup ads = new AppDomainSetup();
                        ads.ShadowCopyFiles = "true";
                        AppDomain.CurrentDomain.SetShadowCopyFiles();
                        AppDomain newDomain = AppDomain.CreateDomain("newDomain");
                        byte[] rawAssembly = loadFile("/Users/Ferds/test.dll");
                        Assembly assembly = newDomain.Load(rawAssembly, null);*/
			
			
			Assembly assembly = Assembly.LoadFrom("./test" + _revision + ".dll");

		
			_currentType = assembly.GetTypes()[0]; //test.GetType());
			CompiledTypes[_currentType.Name] = _currentType;

			Debug.Log("loaded class: " + _currentType.Name);

			if (casterView != null && casterView.isMine)
			{
				PhotonNetwork.RPC(photonView,"CompilePeer",PhotonTargets.OthersBuffered,code,_currentType.Name,casterViewId);
				ThirdPersonController.MyPlayer.SetSpellClassTypeName(_currentType.Name);
			}

//			test.GetType().GetMethod("TestMethod").Invoke(test, null);
			
			assembly = null;
			
			_revision++;
			//AppDomain.Unload(newDomain);
			//newDomain = null;

			return _currentType.Name;
		}
		return null;
	}

	public Type GetCurrentType()
	{
		return _currentType;
	}

	public static bool CompileCSharpCode(string sourceFile, string exeFile)
	{
		CSharpCodeProvider provider = new CSharpCodeProvider();
		
		// Build the parameters for source compilation.
		CompilerParameters cp = new CompilerParameters();


	
		// Add an assembly reference.

		cp.ReferencedAssemblies.Add( "System.dll" );
		cp.ReferencedAssemblies.Add( "/Applications/Unity/Unity.app/Contents/Frameworks/Managed/UnityEngine.dll" );
		//	cp.ReferencedAssemblies.Add( "UnityEngine.dll" );


		Debug.Log("system location: " +  Assembly.Load("System").Location);

		if (Application.isEditor)
			cp.ReferencedAssemblies.Add( Application.dataPath + "/Plugins/Spell.dll" );
		else
			cp.ReferencedAssemblies.Add( Application.dataPath + "/Data/Managed/Spell.dll" );

		// Generate an executable instead of
		// a class library.
		cp.GenerateExecutable = false;
		
		// Set the assembly file name to generate.
		cp.OutputAssembly = exeFile;
		
		// Save the assembly as a physical file.
		cp.GenerateInMemory = false;
		
		// Invoke compilation.
		CompilerResults cr = provider.CompileAssemblyFromSource(cp, sourceFile);
		
		if (cr.Errors.Count > 0)
		{
			// Display compilation errors.
			Debug.LogError("Errors building " + sourceFile + " into " + cr.PathToAssembly);
			foreach (CompilerError ce in cr.Errors)
			{
				Debug.LogError(ce);
			}
		}
		else
		{
			Debug.Log("Source " + sourceFile + " built into " + cr.PathToAssembly + " successfully.");
		}
		
		// Return the results of compilation.
		if (cr.Errors.Count > 0)
		{
			return false;
		}
		else
		{
			return true;
		}
	}

}
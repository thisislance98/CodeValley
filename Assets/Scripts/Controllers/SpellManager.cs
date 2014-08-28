using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Parse;


public class SpellManager : MonoBehaviour {

	public GameObject SpellPrefab;

	public GameObject[] OnSpellLoadedObservers;
	public GameObject OnSpellsDownloadedObserver;
	public GameObject OnCompileObserver;


	public UIPopupList SpellHitPopup;
	public UIPopupList SpellProjectilePopup;
	public UIInput CodeInput;
	public UIToggle CanHitTerrainToggle;
	
	public UIInput SpellNameInput;

	public List<GameObject> SpellProjectilePrefabs = new List<GameObject>();
	public string ProjectileSpellsDirectory;
	public GameObject SpellsLoadedObserver;


	public List<GameObject> SpellHitPrefabs = new List<GameObject>();
	public string HitSpellsDirectory;

	public static SpellManager Instance;


	GameObject _currentProjectileSpell;
	GameObject _currentHitSpell;

	Dictionary<string,Spell> _spells = new Dictionary<string, Spell>();
	bool _callOnSpellsDownloaded;

	// Use this for initialization
	void Awake () {
		Instance = this;

	}

	void Start()
	{
		DownloadSpells();
	}
	

	public GameObject CastSpell(Vector3 startPos, Vector3 targetPos, string targetName, string classTypeName)
	{
		GameObject target = GameObject.Find(targetName);
		
		if (target.isStatic)
			target = DynamicWorld.Instance.ReplaceWithDynamic(target);
		
		if (target == null)
			return null;

		GameObject spell = (GameObject)Instantiate(SpellPrefab,startPos,Quaternion.identity);
		GameObject spellProjectileEffect = (GameObject)Instantiate(_currentProjectileSpell);
		spellProjectileEffect.transform.parent = spell.transform;
		spellProjectileEffect.transform.localPosition = Vector3.zero;
		spell.transform.forward = (targetPos - startPos).normalized;
		
		spell.GetComponent<SpellMotor>().Initialize(targetPos,target.transform,classTypeName,_currentHitSpell);

		return spell;
	}

	public void SetCurrentHitSpell(GameObject hitSpell)
	{
		_currentHitSpell = hitSpell;
	}

	public void SetCurrentProjectileSpell(GameObject projectileSpell)
	{
		_currentProjectileSpell = projectileSpell;

	}

	public GameObject GetCurrentHitSpell()
	{
		return _currentHitSpell;
	}

	public GameObject GetCurrentProjectileSpell()
	{
		return _currentProjectileSpell;
	}

	public void OnLoadSavedSpell(string spellName)
	{
		foreach (GameObject observer in OnSpellLoadedObservers)
		{
			observer.SendMessage("OnSpellLoaded",_spells[spellName]);

		}

		
		SpellNameInput.value = spellName;
		SpellNameInput.label.text = spellName;
		
		if (PlayerPrefs.HasKey(spellName + "Hit"))
			SpellHitPopup.GetComponent<SpellPopupList>().SetSpell(PlayerPrefs.GetString(spellName + "Hit"));
		
		if (PlayerPrefs.HasKey(spellName + "Projectile"))
			SpellProjectilePopup.GetComponent<SpellPopupList>().SetSpell(PlayerPrefs.GetString(spellName + "Projectile"));
		
		StartCoroutine(CompileSpellRoutine());
	}


	public void CompileSpell()
	{
		StartCoroutine(CompileSpellRoutine());
	}

	IEnumerator CompileSpellRoutine()
	{
		// don't do anything if the player hasn't loaded yet
		while (ThirdPersonController.MyPlayer == null)
		{
			yield return new WaitForSeconds(.1f);
		}


		string code = CodeInput.label.text;;
		string spellName = SpellNameInput.value;
		
		Compiler.Instance.CompileSpell(code,ThirdPersonController.MyPlayer.photonView.viewID);


//		PlayerPrefs.SetString(spellName,code);
//		
//		List<string> savedSpells = new List<string>(PlayerPrefsX.GetStringArray("SavedSpells"));
//		
//		if (savedSpells.Contains(spellName) == false)
//		{
//			savedSpells.Add(spellName);
//			PlayerPrefsX.SetStringArray("SavedSpells",savedSpells.ToArray());
//		}
//		
//		PlayerPrefs.SetString(spellName + "Projectile", SpellProjectilePopup.value);
//		PlayerPrefs.SetString(spellName + "Hit", SpellHitPopup.value);

		// add or update spell in our spells dictionary
		Spell spell;
		if (_spells.ContainsKey(spellName))
		{
			spell = _spells[spellName];
			spell.Initialize(spellName,SpellHitPopup.value,SpellProjectilePopup.value,code,CanHitTerrainToggle.value);
		}
		else
		{
			spell = new Spell();
			spell.Initialize(spellName,SpellHitPopup.value,SpellProjectilePopup.value,code,CanHitTerrainToggle.value);
			_spells.Add(spellName,spell);
		}


		ShareSpell(spell);
	}

	public void ShareSpell(Spell spellObj)
	{
		var query = ParseObject.GetQuery("Spell")
			.WhereEqualTo("Name", spellObj.Name);
		
		Debug.Log("sharing: " + spellObj.Name);
		
		query.FindAsync().ContinueWith( t =>
		                               {
			
			Debug.Log("find complete");
			IEnumerable<ParseObject> results = t.Result;
			
			ParseObject spell=null;
			
			foreach(ParseObject obj in results)
			{
				spell = obj;
				break;
			}
			
			if (spell == null)
				spell = new ParseObject("Spell");
			
			spell["Name"] = spellObj.Name;
			spell["Code"] = spellObj.Code;
			spell["HitEffect"] = spellObj.HitEffect;
			spell["ProjectileEffect"] = spellObj.ProjectileEffect;
			spell["CanHitTerrain"] = spellObj.CanHitTerrain;
			
			
			spell.SaveAsync().ContinueWith( e => 
			                               {
				if (e.Exception != null)
					Debug.LogError("error saving spell: " + e.Exception.Message);
				else
					Debug.Log("success saving spell");
			});
			
		});
		
		
	}

	void DownloadSpells()
	{
		var query = ParseObject.GetQuery("Spell");
		
		query.FindAsync().ContinueWith(t => {
			
			IEnumerable<ParseObject> results = t.Result;



			foreach(ParseObject spellObj in results)
			{
				Spell spell = new Spell();
				
				spell.Name = spellObj.Get<string>("Name");
				spell.HitEffect = spellObj.Get<string>("HitEffect");
				spell.ProjectileEffect = spellObj.Get<string>("ProjectileEffect");
				spell.Code = spellObj.Get<string>("Code");
				spell.CanHitTerrain = spellObj.Get<bool>("CanHitTerrain");


				_spells.Add(spell.Name,spell);
			}

			
			_callOnSpellsDownloaded = true;
		
		});
	}

	void Update()
	{

		// we need to do this dumb workaround because you can only call SendMessage from the main thread
		if (_callOnSpellsDownloaded) 
		{
			List<string> spellNames = new List<string>();
			foreach(Spell spell in _spells.Values)
				spellNames.Add(spell.Name);

			Debug.Log("observer: " + OnSpellsDownloadedObserver.name);
			OnSpellsDownloadedObserver.GetComponent<SavedSpellsPopup>().OnSpellNamesDownloaded(spellNames);// .SendMessage("OnSpellNamesDownloaded",spellNames);
			_callOnSpellsDownloaded = false;
		}
	}


}

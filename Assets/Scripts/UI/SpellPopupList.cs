using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SpellType
{
	Projectile,
	Hit
}

public enum ShiftDirection
{
	up,
	down
}

public class SpellPopupList : MonoBehaviour {

	public SpellType TypeOfSpell;

	public bool IsScrollable = false;
	public float ScrollSpeed = 2;

	List<GameObject> _spells;
	List<string> _spellNames = new List<string>();

	UIPopupList _popup;
	// Use this for initialization
	void Awake () {


		_popup = GetComponent<UIPopupList>();
	
		_spells = (TypeOfSpell == SpellType.Projectile) ? SpellManager.Instance.SpellProjectilePrefabs : SpellManager.Instance.SpellHitPrefabs;


		for (int i=0; i < _spells.Count; i++)
		{
			_spellNames.Add(_spells[i].name);

		}

		_popup.items = _spellNames;
		_popup.value = _spellNames[0];

	}

	void Update()
	{
		if (_popup.isOpen == false)
			return;

		if (IsScrollable)
		{
			float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
			if (scrollDelta < 0 )
				_popup.GetDropDown().position += Mathf.Abs(scrollDelta) * Vector3.up * ScrollSpeed ;
			else if (scrollDelta > 0 )
				_popup.GetDropDown().transform.position += Mathf.Abs(scrollDelta) * Vector3.down * ScrollSpeed;
		}


	}

	void Shift(ShiftDirection dir)
	{
		Debug.Log("shifting " + dir);

		List<string> items = _popup.items;

		if (dir == ShiftDirection.down)
		{
			string start = items[0];
			items.RemoveAt(0);
			items.Add(start);
		}
		else if (dir == ShiftDirection.up)
		{
			string last = items[items.Count-1];
			items.RemoveAt(items.Count-1);

			items.Insert(0,last);

		}

		_popup.SetItems(items);


	}

	public void OnSpellSelected()
	{
		if (_popup == null || _popup.value == null)
			return;

		string spellName = _popup.value;
		SetSpell(spellName);

	}

	public void SetSpell(string spellName)
	{
		_popup.value = spellName;

		Debug.Log("spell selected: " + spellName);
		int index = _spellNames.FindIndex(x => x.Equals(spellName));
		
		if (TypeOfSpell == SpellType.Hit)
			SpellManager.Instance.SetCurrentHitSpell(_spells[index]);
		else
			SpellManager.Instance.SetCurrentProjectileSpell(_spells[index]);

	}
}

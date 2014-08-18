using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIDestroy : MonoBehaviour {
	public bool destroyai;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	if(destroyai){
		AI ai=(AI)GetComponent("AI");
			if(ai){
			int flist=ai.friendly.Count;
				if(ai.friendly.Count>0){
		for (int f = 0; f < flist; f++){
						if (ai.friendly[f] == null)
							continue;

					AI aif=(AI)ai.friendly[f].GetComponent("AI");
						if(aif){
					if(aif.companionleader=null){
						aif.companionleader=null;
						aif.companion=false;
						aif.targetlocateto=null;
						aif.gototarget=false;
						aif.gototargetnostop=false;
						aif.idle=true;
					}
					}
					}
				}
				
				if(ai.enemys.Count>0){
				int elist=ai.enemys.Count;
		for (int i = 0; i < elist; i++){
					AI aie=(AI)ai.enemys[i].GetComponent("AI");
						if(aie){
				if(aie.enemy==transform){
						aie.enemy=null;
						if(aie.attack){
							aie.attack=false;
							if(aie.targetlocateto)aie.gototarget=true;
							else aie.idle=true;
						}
					}
				aie.enemys.Remove(transform);
					}
					}
					
			}
				Destroy(gameObject);
		}
				
			}
		
	}
}

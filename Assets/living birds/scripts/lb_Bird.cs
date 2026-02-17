using UnityEngine;
using System.Collections;

public class lb_Bird : MonoBehaviour {
	enum birdBehaviors{
		sing,
		preen,
		ruffle,
		peck,
		hopForward,
		hopBackward,
		hopLeft,
		hopRight,
	}

	public bool fleeCrows = true;

	Animator anim;
	lb_BirdController controller;

	bool paused = false;
	bool idle = true;
	bool flying = false;
	bool landing = false;
	bool perched = false;
	bool onGround = true;
	bool dead = false;
	BoxCollider birdCollider;
	Vector3 bColCenter;
	Vector3 bColSize;
	SphereCollider solidCollider;
	float distanceToTarget = 0.0f;
	float agitationLevel = .5f;
	float originalAnimSpeed = 1.0f;
	Vector3 originalVelocity = Vector3.zero;

	int idleAnimationHash;
	int flyAnimationHash;
	int hopIntHash;
	int flyingBoolHash;
	int peckBoolHash;
	int ruffleBoolHash;
	int preenBoolHash;
	int landingBoolHash;
	int singTriggerHash;
	int flyingDirectionHash;
	int dieTriggerHash;

	void OnEnable () {
		birdCollider = gameObject.GetComponent<BoxCollider>();
		bColCenter = birdCollider.center;
		bColSize = birdCollider.size;
		solidCollider = gameObject.GetComponent<SphereCollider>();
		anim = gameObject.GetComponent<Animator>();

		idleAnimationHash = Animator.StringToHash("Base Layer.Idle");
		flyAnimationHash = Animator.StringToHash("Base Layer.fly");
		hopIntHash = Animator.StringToHash("hop");
		flyingBoolHash = Animator.StringToHash("flying");
		peckBoolHash = Animator.StringToHash("peck");
		ruffleBoolHash = Animator.StringToHash("ruffle");
		preenBoolHash = Animator.StringToHash("preen");
		landingBoolHash = Animator.StringToHash("landing");
		singTriggerHash = Animator.StringToHash("sing");
		flyingDirectionHash = Animator.StringToHash("flyingDirectionX");
		dieTriggerHash = Animator.StringToHash("die");
		anim.SetFloat("IdleAgitated", agitationLevel);

		if (dead){
			Revive();
		}
	}

	void PauseBird(){
		if(!dead){
			originalAnimSpeed = anim.speed;
			anim.speed = 0;
			if(!GetComponent<Rigidbody>().isKinematic){ originalVelocity = GetComponent<Rigidbody>().linearVelocity; }
			GetComponent<Rigidbody>().isKinematic = true;
			paused = true;
		}
	}

	void UnPauseBird(){
		if(!dead){
			anim.speed = originalAnimSpeed;
			GetComponent<Rigidbody>().isKinematic = false;
			GetComponent<Rigidbody>().linearVelocity = originalVelocity;
			paused = false;
		}
	}

	IEnumerator FlyToTarget(Vector3 target){
		flying = true;
		landing = false;
		onGround = false;
		GetComponent<Rigidbody>().isKinematic = false;
		GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
		GetComponent<Rigidbody>().linearDamping = 0.5f;
		anim.applyRootMotion = false;
		anim.SetBool(flyingBoolHash, true);
		anim.SetBool(landingBoolHash, false);

		while(anim.GetCurrentAnimatorStateInfo(0).nameHash != flyAnimationHash){
			yield return 0;
		}

		GetComponent<Rigidbody>().AddForce((transform.forward * 50.0f*controller.birdScale)+(transform.up * 100.0f*controller.birdScale));
		float t = 0.0f;
		while (t<1.0f){
			if(!paused){
				t+= Time.deltaTime;
				if(t>.2f && !solidCollider.enabled && controller.collideWithObjects){
					solidCollider.enabled = true;
				}
			}
			yield return 0;
		}

		Vector3 vectorDirectionToTarget = (target-transform.position).normalized;
		Quaternion finalRotation = Quaternion.identity;
		Quaternion startingRotation = transform.rotation;
		distanceToTarget = Vector3.Distance(transform.position, target);
		Vector3 forwardStraight;
		RaycastHit hit;
		Vector3 tempTarget = target;
		t = 0.0f;

		if(vectorDirectionToTarget.y>.5f){
			tempTarget = transform.position + (new Vector3(transform.forward.x,.5f,transform.forward.z)*distanceToTarget);

			while(vectorDirectionToTarget.y>.5f){
				vectorDirectionToTarget = (tempTarget-transform.position).normalized;
				finalRotation = Quaternion.LookRotation(vectorDirectionToTarget);
				transform.rotation = Quaternion.Slerp(startingRotation, finalRotation, t);
				anim.SetFloat(flyingDirectionHash, FindBankingAngle(transform.forward, vectorDirectionToTarget));
				t += Time.deltaTime*0.5f;
				GetComponent<Rigidbody>().AddForce(transform.forward * 70.0f*controller.birdScale * Time.deltaTime);
				vectorDirectionToTarget = (target-transform.position).normalized;

				if (Physics.Raycast(transform.position,-Vector3.up,out hit,0.15f*controller.birdScale) && GetComponent<Rigidbody>().linearVelocity.y < 0){
					if(!hit.collider.isTrigger){
						GetComponent<Rigidbody>().linearVelocity = new Vector3(GetComponent<Rigidbody>().linearVelocity.x, 0.0f, GetComponent<Rigidbody>().linearVelocity.z);
					}
				}
				if (Physics.Raycast(transform.position,Vector3.up,out hit,0.15f*controller.birdScale) && GetComponent<Rigidbody>().linearVelocity.y > 0){
					if(!hit.collider.isTrigger){
						GetComponent<Rigidbody>().linearVelocity = new Vector3(GetComponent<Rigidbody>().linearVelocity.x, 0.0f, GetComponent<Rigidbody>().linearVelocity.z);
					}
				}
				if(controller.collideWithObjects){
					forwardStraight = transform.forward;
					forwardStraight.y = 0.0f;
					if (Physics.Raycast(transform.position+(transform.forward*.15f*controller.birdScale),forwardStraight,out hit,.75f*controller.birdScale)){
						if(!hit.collider.isTrigger){
							AbortFlyToTarget();
						}
					}
				}
				yield return null;
			}
		}

		finalRotation = Quaternion.identity;
		startingRotation = transform.rotation;
		distanceToTarget = Vector3.Distance(transform.position, target);

		while(transform.rotation != finalRotation || distanceToTarget >= 1.5f){
			if(!paused){
				distanceToTarget = Vector3.Distance(transform.position, target);
				vectorDirectionToTarget = (target-transform.position).normalized;
				if(vectorDirectionToTarget==Vector3.zero){
					vectorDirectionToTarget = new Vector3(0.0001f,0.00001f,0.00001f);
				}
				finalRotation = Quaternion.LookRotation(vectorDirectionToTarget);
				transform.rotation = Quaternion.Slerp(startingRotation, finalRotation, t);
				anim.SetFloat(flyingDirectionHash, FindBankingAngle(transform.forward, vectorDirectionToTarget));
				t += Time.deltaTime*0.5f;
				GetComponent<Rigidbody>().AddForce(transform.forward * 70.0f*controller.birdScale * Time.deltaTime);
				if (Physics.Raycast(transform.position,-Vector3.up,out hit,0.15f*controller.birdScale) && GetComponent<Rigidbody>().linearVelocity.y < 0){
					if(!hit.collider.isTrigger){
						GetComponent<Rigidbody>().linearVelocity = new Vector3(GetComponent<Rigidbody>().linearVelocity.x, 0.0f, GetComponent<Rigidbody>().linearVelocity.z);
					}
				}
				if (Physics.Raycast(transform.position,Vector3.up,out hit,0.15f*controller.birdScale) && GetComponent<Rigidbody>().linearVelocity.y > 0){
					if(!hit.collider.isTrigger){
						GetComponent<Rigidbody>().linearVelocity = new Vector3(GetComponent<Rigidbody>().linearVelocity.x, 0.0f, GetComponent<Rigidbody>().linearVelocity.z);
					}
				}
				if(controller.collideWithObjects){
					forwardStraight = transform.forward;
					forwardStraight.y = 0.0f;
					if (Physics.Raycast(transform.position+(transform.forward*.15f*controller.birdScale),forwardStraight,out hit,.75f*controller.birdScale)){
						if(!hit.collider.isTrigger){
							AbortFlyToTarget();
						}
					}
				}
			}
			yield return 0;
		}

		float flyingForce = 50.0f*controller.birdScale;
		while(true){
			if(!paused){
				if (Physics.Raycast(transform.position,-Vector3.up,0.15f*controller.birdScale) && GetComponent<Rigidbody>().linearVelocity.y < 0){
					GetComponent<Rigidbody>().linearVelocity = new Vector3(GetComponent<Rigidbody>().linearVelocity.x, 0.0f, GetComponent<Rigidbody>().linearVelocity.z);
				}
				if (Physics.Raycast(transform.position,Vector3.up,out hit,0.15f*controller.birdScale) && GetComponent<Rigidbody>().linearVelocity.y > 0){
					if(!hit.collider.isTrigger){
						GetComponent<Rigidbody>().linearVelocity = new Vector3(GetComponent<Rigidbody>().linearVelocity.x, 0.0f, GetComponent<Rigidbody>().linearVelocity.z);
					}
				}
				if(controller.collideWithObjects){
					forwardStraight = transform.forward;
					forwardStraight.y = 0.0f;
					if (Physics.Raycast(transform.position+(transform.forward*.15f*controller.birdScale),forwardStraight,out hit,.75f*controller.birdScale)){
						if(!hit.collider.isTrigger){
							AbortFlyToTarget();
						}
					}
				}
				vectorDirectionToTarget = (target-transform.position).normalized;
				finalRotation = Quaternion.LookRotation(vectorDirectionToTarget);
				anim.SetFloat(flyingDirectionHash, FindBankingAngle(transform.forward, vectorDirectionToTarget));
				transform.rotation = finalRotation;
				GetComponent<Rigidbody>().AddForce(transform.forward * flyingForce * Time.deltaTime);
				distanceToTarget = Vector3.Distance(transform.position, target);
				if(distanceToTarget <= 1.5f*controller.birdScale){
					solidCollider.enabled = false;
					if(distanceToTarget < 0.5f*controller.birdScale){
						break;
					}else{
						GetComponent<Rigidbody>().linearDamping = 2.0f;
						flyingForce = 50.0f*controller.birdScale;
					}
				}else if(distanceToTarget <= 5.0f*controller.birdScale){
					GetComponent<Rigidbody>().linearDamping = 1.0f;
					flyingForce = 50.0f*controller.birdScale;
				}
			}
			yield return 0;
		}

		anim.SetFloat(flyingDirectionHash, 0);
		Vector3 vel = Vector3.zero;
		flying = false;
		landing = true;
		solidCollider.enabled = false;
		anim.SetBool(landingBoolHash, true);
		anim.SetBool(flyingBoolHash, false);
		t = 0.0f;
		GetComponent<Rigidbody>().linearVelocity = Vector3.zero;

		Collider[] hitColliders = Physics.OverlapSphere(target, 0.05f*controller.birdScale);
		for(int i=0;i<hitColliders.Length;i++){
			if(hitColliders[i].tag == "lb_bird" && hitColliders[i].transform != transform){
				hitColliders[i].SendMessage("FlyAway");
			}
		}

		startingRotation = transform.rotation;
		transform.localEulerAngles = new Vector3(0.0f, transform.localEulerAngles.y, 0.0f);
		finalRotation = transform.rotation;
		transform.rotation = startingRotation;
		while(distanceToTarget>0.05f*controller.birdScale){
			if(!paused){
				transform.rotation = Quaternion.Slerp(startingRotation, finalRotation, t*4.0f);
				transform.position = Vector3.SmoothDamp(transform.position, target, ref vel, 0.5f);
				t += Time.deltaTime;
				distanceToTarget = Vector3.Distance(transform.position, target);
				if(t>2.0f){ break; }
			}
			yield return 0;
		}
		GetComponent<Rigidbody>().linearDamping = .5f;
		GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
		anim.SetBool(landingBoolHash, false);
		landing = false;
		transform.localEulerAngles = new Vector3(0.0f, transform.localEulerAngles.y, 0.0f);
		transform.position = target;
		anim.applyRootMotion = true;
		onGround = true;
	}

	float FindBankingAngle(Vector3 birdForward, Vector3 dirToTarget){
		Vector3 cr = Vector3.Cross(birdForward, dirToTarget);
		float ang = Vector3.Dot(cr, Vector3.up);
		return ang;
	}

	void OnGroundBehaviors(){
		idle = anim.GetCurrentAnimatorStateInfo(0).nameHash == idleAnimationHash;
		if(!GetComponent<Rigidbody>().isKinematic){
			GetComponent<Rigidbody>().isKinematic = true;
		}
		if(idle){
			if(Random.value < Time.deltaTime*.33){
				float rand = Random.value;
				if(rand < .3){
					DisplayBehavior(birdBehaviors.sing);
				}else if(rand < .5){
					DisplayBehavior(birdBehaviors.peck);
				}else if(rand < .6){
					DisplayBehavior(birdBehaviors.preen);
				}else if(!perched && rand<.7){
					DisplayBehavior(birdBehaviors.ruffle);
				}else{
					DisplayBehavior(birdBehaviors.sing);
				}
				anim.SetFloat("IdleAgitated", Random.value);
			}
			if(Random.value < Time.deltaTime*.1){
				FlyAway();
			}
		}
	}

	void DisplayBehavior(birdBehaviors behavior){
		idle = false;
		switch(behavior){
		case birdBehaviors.sing:
			anim.SetTrigger(singTriggerHash);
			break;
		case birdBehaviors.ruffle:
			anim.SetTrigger(ruffleBoolHash);
			break;
		case birdBehaviors.preen:
			anim.SetTrigger(preenBoolHash);
			break;
		case birdBehaviors.peck:
			anim.SetTrigger(peckBoolHash);
			break;
		case birdBehaviors.hopForward:
			anim.SetInteger(hopIntHash, 1);
			break;
		case birdBehaviors.hopLeft:
			anim.SetInteger(hopIntHash, -2);
			break;
		case birdBehaviors.hopRight:
			anim.SetInteger(hopIntHash, 2);
			break;
		case birdBehaviors.hopBackward:
			anim.SetInteger(hopIntHash, -1);
			break;
		}
	}

	void OnTriggerEnter(Collider col){
		if(col.tag == "lb_bird"){
			FlyAway();
		}
	}

	void OnTriggerExit(Collider col){
		if(onGround && (col.tag == "lb_groundTarget" || col.tag == "lb_perchTarget")){
			FlyAway();
		}
	}

	void AbortFlyToTarget(){
		StopCoroutine("FlyToTarget");
		solidCollider.enabled = false;
		anim.SetBool(landingBoolHash, false);
		anim.SetFloat(flyingDirectionHash, 0);
		transform.localEulerAngles = new Vector3(0.0f, transform.localEulerAngles.y, 0.0f);
		FlyAway();
	}

	void FlyAway(){
		if(!dead && controller != null){
			StopCoroutine("FlyToTarget");
			anim.SetBool(landingBoolHash, false);
			controller.SendMessage("BirdFindTarget", gameObject);
		}
	}

	void Flee(){
		if(!dead){
			StopCoroutine("FlyToTarget");
			anim.Play(flyAnimationHash);
			Vector3 farAwayTarget = transform.position;
			farAwayTarget += new Vector3(Random.Range(-100,100)*controller.birdScale, 10*controller.birdScale, Random.Range(-100,100)*controller.birdScale);
			StartCoroutine("FlyToTarget", farAwayTarget);
		}
	}

	void CrowIsClose(){
		if(fleeCrows && !dead){
			Flee();
		}
	}

	public void KillBird(){
		if(!dead){
			if(controller != null) controller.SendMessage("FeatherEmit", transform.position);
			anim.SetTrigger(dieTriggerHash);
			anim.applyRootMotion = false;
			dead = true;
			onGround = false;
			flying = false;
			landing = false;
			idle = false;
			perched = false;
			AbortFlyToTarget();
			StopAllCoroutines();
			GetComponent<Collider>().isTrigger = false;
			birdCollider.center = new Vector3(0.0f, 0.0f, 0.0f);
			birdCollider.size = new Vector3(0.1f, 0.01f, 0.1f)*controller.birdScale;
			GetComponent<Rigidbody>().isKinematic = false;
			GetComponent<Rigidbody>().useGravity = true;
		}
	}

	public void KillBirdWithForce(Vector3 force){
		if(!dead){
			if(controller != null) controller.SendMessage("FeatherEmit", transform.position);
			anim.SetTrigger(dieTriggerHash);
			anim.applyRootMotion = false;
			dead = true;
			onGround = false;
			flying = false;
			landing = false;
			idle = false;
			perched = false;
			AbortFlyToTarget();
			StopAllCoroutines();
			GetComponent<Collider>().isTrigger = false;
			birdCollider.center = new Vector3(0.0f, 0.0f, 0.0f);
			birdCollider.size = new Vector3(0.1f, 0.01f, 0.1f)*controller.birdScale;
			GetComponent<Rigidbody>().isKinematic = false;
			GetComponent<Rigidbody>().useGravity = true;
			GetComponent<Rigidbody>().AddForce(force);
		}
	}

	void Revive(){
		if(dead){
			birdCollider.center = bColCenter;
			birdCollider.size = bColSize;
			GetComponent<Collider>().isTrigger = true;
			dead = false;
			onGround = false;
			flying = false;
			landing = false;
			idle = true;
			perched = false;
			GetComponent<Rigidbody>().isKinematic = false;
			GetComponent<Rigidbody>().useGravity = false;
			anim.Play(idleAnimationHash);
			if(controller != null){
				controller.SendMessage("BirdFindTarget", gameObject);
			}
		}
	}

	void SetController(lb_BirdController cont){
		controller = cont;
	}

	void ResetHopInt(){
		anim.SetInteger(hopIntHash, 0);
	}

	void ResetFlyingLandingVariables(){
		if(flying || landing){
			flying = false;
			landing = false;
		}
	}

	void Update(){
		if(onGround && !paused && !dead){
			OnGroundBehaviors();
		}
	}
}
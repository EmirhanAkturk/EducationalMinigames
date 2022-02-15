using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class BasicCharacterController : MonoBehaviour, ICharacterController
{
	public bool IsActive { get; set; } = true;
	public bool IsCharacterBoosterChecked;

	public IBrainController BrainControler
	{
		get { return brainControler ?? (brainControler = GetComponent<IBrainController>()); }
	}

	public IMovementController MovementController
	{
		get { return movementController ?? (movementController = GetComponent<IMovementController>()); }
	}

	public CharacterBooster CharacterBooster
	{
		get
		{
			if (characterBooster == null && !IsCharacterBoosterChecked)
			{
				characterBooster = GetComponent<CharacterBooster>();
				IsCharacterBoosterChecked = true;
			}

			return characterBooster;
		}
	}

	public Transform CharacterModelTr
	{
		get
		{
			if (characterModelTr == null || characterModelTr.Equals(null) || characterModelTr.gameObject == null || !characterModelTr.gameObject.activeSelf)
			{
				try
				{
					characterModelTr = SkinController.ActiveSkin.transform;
				}
				catch
				{
					characterModelTr = null;
				}
			}
			return characterModelTr;
		}
	}

	public ISkinController SkinController
	{
		get
		{
			if (skinController == null || skinController.Equals(null))
				skinController = GetComponent<ISkinController>();

			return skinController;
		}
	}	
	
	public CarryController CarryController
	{
		get
		{
			if (carryController == null || carryController.Equals(null))
				carryController = GetComponent<CarryController>();

			return carryController;
		}
	}

	public readonly Dictionary<BehavioralEffectType, IBehavioralEffect> BehavioralEffects = new Dictionary<BehavioralEffectType, IBehavioralEffect>();

	[SerializeField] protected CharacterData characterData;

	private Transform characterModelTr;
	private IBrainController brainControler;
	private IMovementController movementController;
	private ISkinController skinController;
	private CharacterBooster characterBooster;
	private CarryController carryController;

	private float moveSpeed;
	private bool isWalking;

	public virtual void OnEnable()
	{
		if(BrainControler != null) BrainControler.Initialize(this);
		SkinController.SetSkin(this);
		if(CarryController != null) CarryController.Initialize();
	}

	protected void OnDisable()
	{
		CharacterBooster?.BoostDictionary.Clear();
	}

	private void FixedUpdate()
	{
		if (!IsActive) return;
        // bool callDo = true;
        // foreach (var item in BehavioralEffects)
        // {
        // 	callDo &= item.Value.Do();
        // }

        // if (callDo) BrainControler.Do();

        if (BrainControler != null) BrainControler.Do();
	}

	public virtual T GetCharacterValue<T>(CharacterDataType characterDataType)
	{
		T value = (T)characterData.GetData(characterDataType);

		if (value is float)
		{
			float valueAsFloat = (float)(object)value;
			if ((int)characterData.GetData(CharacterDataType.TeamID) != 0)
			{
				// var enemyBoosts = ConfigurationService.Configurations.EnemyBoosts;
				// foreach (var item in enemyBoosts)
				// {
				// 	if (item.DataType == characterDataType)
				// 	{
				// 		valueAsFloat = valueAsFloat * item.Boost.Multiply + item.Boost.Addition;
				// 		value = (T)(object)valueAsFloat;
				// 	}
				// }
				/*if(characterDataType == CharacterDataType.AttackDamage
                    || characterDataType == CharacterDataType.Health
                    || characterDataType == CharacterDataType.SpecialDamage
                    )
                {
                    var hardness = LevelManager.Instance.ActiveWorld.WorldHardenMultiplier;
                    if (LevelManager.Instance.ActiveWorld.EnableAdaptiveHardness)
                    {
                        float hardnessMod = 1f;
                        var deck = DeckManager.Instance.GetActiveDeck();
                        foreach (var item in deck)
                        {
                            hardnessMod += ((float)InventoryManager.Instance.GetItem(item).Rank - 1f) * 0.1f;
                        }
                        if (AttackableManager.Instance.IsTeamExists(0))
                        {
                            hardness *= (AttackableManager.Instance.GetAttackablesCount(0) > 10 ? (float)AttackableManager.Instance.GetAttackablesCount(0) * 0.1f : 1f);
                        }
                        hardness *= hardnessMod;
                    }
                    valueAsFloat = valueAsFloat * hardness;
                    value = (T)(object)valueAsFloat;
                }*/
			}
			if (CharacterBooster != null)
				value = (T)(object)CharacterBooster.ApplyBoost(characterDataType, valueAsFloat);
		}
		return value;
	}
	public void SetSkin()
    {
		SkinController.SetSkin(this);
		// if(brainControler is BaseInteractorAIBrainControler)
		// 	(brainControler as BaseInteractorAIBrainControler).CarryController.Initialize();

	}

	// private Vector3 lastKnownPosition;
	// private float stopTime;
	public void Move(Vector3 destination, Action callback = null)
	{
		/*
		if(Vector3.Distance(lastKnownPosition, transform.position) > 0.1f)
		 {
		     lastKnownPosition = transform.position;
		     stopTime = Time.time;
		 }*/
		// if ( /*GameManager.Instance.IsPlaying && */(Time.time - stopTime > 0.5f)) transform.position += (Vector3.back * 0.3f);
		
		SetSpeed(GetCharacterValue<float>(CharacterDataType.MoveSpeed));
		MovementController.Move(destination, this, callback);
	}

	public void MoveWithDirection(Vector2 direction)
	{
		// SetSpeed(GetCharacterValue<float>(CharacterDataType.MoveSpeed));
		if (direction == Vector2.zero) Stop();
		else MovementController.MoveWithDirection(direction, this);
	}

	public void LookTargetObject(Vector3 targetPos)
	{
		CharacterModelTr.LookAt(new Vector3(targetPos.x, CharacterModelTr.position.y, targetPos.z));
	}

	public void Stop()
	{
		MovementController.Stop();
	}

	public void SetSpeed(float newSpeed)
	{
		MovementController.SetSpeed(newSpeed);
		MovementController.SetMovementSetting(GetCharacterValue<float>(CharacterDataType.StoppingDistance));
	}

	public float GetSpeed()
	{
		return movementController.GetSpeed();
	}

	public bool IsCharacterWalking()
    {
		return movementController.IsCharacterWalking();
    }
}

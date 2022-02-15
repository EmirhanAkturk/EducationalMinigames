using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding;

public abstract class BaseMovementController : MonoBehaviour, IMovementController
{
	public AIPath Agent
	{
		get
		{
			if (agent == null) agent = GetComponent<AIPath>();
			return agent;
		}
	}

	public IAnimationController AnimationController
	{
		get
		{
			if (activeAnimationController == null || activeAnimationController.Equals(null) || !activeAnimationController.Animator.isActiveAndEnabled)
			{
				if (animationControllers == null)
					animationControllers = GetComponentsInChildren<IAnimationController>(true).ToList();

				foreach (var controller in animationControllers)
				{
					if (controller.Animator.isActiveAndEnabled)
					{
						activeAnimationController = controller;
						break;
					}
				}
			}

			return activeAnimationController;
		}
	}

	private IAnimationController activeAnimationController;
	private List<IAnimationController> animationControllers;

    private AIPath agent;
	private bool isDoRotating;
    
	public bool canIdle = true;

	public abstract void Move(Vector3 destination, ICharacterController characterController, Action callback);
	public abstract void MoveWithDirection(Vector2 direction, ICharacterController characterController);

	public virtual void Stop()
	{
		if (Agent != null && Agent.enabled)
			Agent.isStopped = true;
		
		if(canIdle) AnimationController?.ChangeAnimation(AnimationType.Idle);
	}

	public virtual void SetSpeed(float newSpeed)
	{
		Agent.maxSpeed = newSpeed;
	}

	public virtual void SetMovementSetting(float stoppingDistance)
	{
		float difference = stoppingDistance - Agent.endReachedDistance;
		Agent.endReachedDistance = stoppingDistance;
	}

	public virtual float GetSpeed()
	{
		return Agent.maxSpeed;
	}

	protected void CheckChildRotation(ICharacterController characterController)
	{
		if (characterController != null && characterController.CharacterModelTr != null)
		{
			if (Vector3.Distance(characterController.CharacterModelTr.transform.localEulerAngles, Vector3.zero) > 1f)
			{
				isDoRotating = true;
				characterController.CharacterModelTr.DOLocalRotate(Vector3.zero, 0.01f)
					.OnComplete(() => isDoRotating = false);
			}

			if (Vector3.Distance(characterController.CharacterModelTr.transform.localPosition, Vector3.zero) > 0.0001f)
			{
				characterController.CharacterModelTr.DOLocalMove(Vector3.zero, 0.01f);
			}
		}
	}

	public bool IsCharacterWalking()
    {
		return !Agent.isStopped;
    }
}
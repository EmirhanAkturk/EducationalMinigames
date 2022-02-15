using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkMoveController : BaseMovementController
{
	private ICharacterController cc;
	private Action callback;
	private Vector3 destination;

    public bool IsStopped => Agent.isStopped;

    public override void Stop()
    {
	    CheckChildRotation(cc);
        base.Stop();
	    callback?.Invoke();
	    callback = null;
    }

    private void OnDrawGizmos()
    {
		if(destination != null)
        {
			Gizmos.color = Color.black;
			Gizmos.DrawWireSphere(destination, 0.4f);
		}
			
	}

    public override void Move(Vector3 _destination, ICharacterController characterController, Action _callback)
	{
		if (_callback == null && callback != null && CheckDestinationReached(characterController))
		{
			callback?.Invoke();
			callback = null;
		}
		
		if (_callback != null) callback = _callback;
		cc = characterController;
		destination = _destination;

		if (CheckDestinationReached( characterController) || characterController.GetCharacterValue<float>(CharacterDataType.MoveSpeed) == 0)
		{
			Stop();
			return;
		}
		
		CheckChildRotation(characterController);

		AnimationController.ChangeAnimation(AnimationType.Walk);

		if (Agent != null && Agent.enabled)
		{
			Agent.isStopped = false;
			Agent.destination = _destination;
		}
	}

    private bool CheckDestinationReached(ICharacterController characterController)
    {
        return GameMath.IsInRange(destination, transform.position, characterController.GetCharacterValue<float>(CharacterDataType.StoppingDistance), false);
    }

    public override void MoveWithDirection(Vector2 direction, ICharacterController characterController)
    {
        CheckChildRotation(characterController);
        cc = characterController;
        AnimationController.ChangeAnimation(AnimationType.Walk);

        Agent.isStopped = false;
        Vector3 finalDirection = direction.normalized * Time.deltaTime * GetSpeed();
        Vector3 toPos = transform.position + new Vector3(finalDirection.x, 0, finalDirection.y);
        Vector3 finalPosition = GetNearestPoint(toPos);
        // Vector3 nearestPoint = GetNearestPoint(finalPosition);
        // if(!GameMath.IsInRange(nearestPoint, finalPosition, 0.01f)) finalPosition = nearestPoint;
        transform.position = finalPosition;

        //normalized for rotation
        Vector2 input = direction.normalized;
        transform.eulerAngles = Vector3.up * Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;
    }


    private Vector3 GetNearestPoint(Vector3 position)
    {
        return ((Vector3)((Pathfinding.GraphNode)AstarPath.active.GetNearest(position, Pathfinding.NNConstraint.Default)).ClosestPointOnNode(position));
    }
}
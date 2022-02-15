using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "CharacterData", menuName = "lib/GameDependent/CharacterData", order = 1)]
public class CharacterData : ScriptableObject
{
    [Header("General Variables")]
    public int TeamID;
    public PoolType PoolType;
    public CharacterType CharacterType;

    [Header("Movement Variables")]
    public float MoveSpeed;
    public float StoppingDistance;

    [Header("Interactable Variables")]
    public float SightRange;
    public float InteractRange;
    public float ManifactureTime;
    public float TransportTime;
    public float InteractionCount;
    public float InteractionRatio;

    public float BallThrowTime;
    public float BallThrowRange;
    public int VacuumRequiredBallAmount;


    // [Header("Carry Points Capacities")]
    // public CarryPointsCapacities CarryPointsCapacities;

	public object GetData(CharacterDataType dataType)
    {
        switch (dataType)
        {
            //General Variables
            case CharacterDataType.TeamID:
                return TeamID;
            case CharacterDataType.PoolType:
                return PoolType;
            case CharacterDataType.CharacterType:
                return CharacterType;

            //Movement Variables
            case CharacterDataType.MoveSpeed:
                return MoveSpeed;
            case CharacterDataType.StoppingDistance:
                return StoppingDistance;

            //Interactable Variables
            case CharacterDataType.InteractRange:
                return InteractRange;
            case CharacterDataType.SightRange:
                return SightRange;
            case CharacterDataType.InteractionCount:
                return InteractionCount;
            case CharacterDataType.InteractionRatio:
                return InteractionRatio;
            case CharacterDataType.TransportTime:
                return TransportTime;
            case CharacterDataType.ManifactureTime:
                return ManifactureTime;
                
            case CharacterDataType.BallThrowTime:
                return BallThrowTime;
            case CharacterDataType.BallThrowRange:
                return BallThrowRange;
            case CharacterDataType.VacuumRequiredBallAmount:
                return VacuumRequiredBallAmount;

            // //Carry Points Capacities
            // case CharacterDataType.CarryPointsCapacities:
            //     return CarryPointsCapacities;


            default:
                return null;
        }
    }

}

public enum CharacterDataType
{
	//General Variables
	TeamID = 0,
	PoolType = 1,
	ModelPath = 2,
	ModelName = 3,
	CharacterType = 4,

	//Movement Variables
	MoveSpeed = 5,
	StoppingDistance = 6,

	//Interactable Variables
	InteractRange = 7,

	//CarryPointsCapacities
	CarryPointsCapacities = 8,

	SightRange = 9,

	ManifactureTime = 10,
	InteractionCount = 11,
	InteractionRatio = 12,
	TransportTime = 13,
    RollToolSpeed = 14,
    VacuumRequiredBallAmount = 15,
    BallThrowTime = 16,
    BallThrowRange = 17,
}

public enum CharacterType
{
    Saler = 1,
    Customer = 2,
    ShoppingStaff = 3,
    CashierStaff = 4,
    Influencer = 5,
}
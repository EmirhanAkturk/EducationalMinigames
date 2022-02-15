using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyCarryPoint : CarryPoint
{
	[Header("Rotations")]
	[SerializeField] private Vector3 representationObjRotation = new Vector3(0, -90, 0);

	[Header("Positions")]
	[SerializeField] private Vector3 firstSlotLocalPosition;
	[SerializeField] private Vector3 slotOffset;

	protected override void OnEnable()
	{
		base.OnEnable();
		AnimationController.ChangeArmsState(CarryPointType.Hands, false);
	}

	protected override void AddRepresentationalObject(PoolType objectType, GameObject go)
	{
		base.AddRepresentationalObject(objectType, go);
		AnimationController.ChangeArmsState(CarryPointType.Hands, true);
	}

	protected override bool RemoveRepresentationalObject(PoolType objectType, GameObject go)
	{
		bool returnValue = base.RemoveRepresentationalObject(objectType, go);
		if (objectType == PoolType.SupplyBox) ChangeCurrencyAmount(-1);
		if(gameObject.activeInHierarchy) StartCoroutine(CheckArmState());
		return returnValue;
	}

	private IEnumerator CheckArmState(float delay = .5f)
	{
		yield return new WaitForSeconds(delay);
		AnimationController.ChangeArmsState(CarryPointType.Hands, GetAllCarriedObjects().Count > 0);
	}

	protected override CollectableObject AdjustCarriedObject(CollectableObject carriedObject)
	{
		var carriedGO = base.AdjustCarriedObject(carriedObject).ActiveGameObject;

		Vector3 nextSlotPosition = GetSlotPosition(GetAllCarriedObjects().Count - 1, false); // the minus one is because we add then get next slot which is one count higher

		carriedGO.transform.localPosition = nextSlotPosition;
		carriedGO.transform.localRotation = Quaternion.Euler(representationObjRotation);
		if (carriedObject.CollectableObjectId == 0) ChangeCurrencyAmount(1);

		return carriedObject;
	}

	public Vector3 GetNextSlotPosition(bool worldSpace = true)
	{
		return GetSlotPosition(GetAllCarriedObjects().Count, worldSpace);
	}

	public Vector3 GetSlotPosition(int slotIndex, bool worldSpace = true)
	{
		Vector3 slotPosition = firstSlotLocalPosition + slotOffset * slotIndex;
		if (worldSpace) return transform.TransformPoint(slotPosition);
		return slotPosition;
	}

    public override Vector3 GetTargetPosition()
    {
        return GetSlotPosition(GetAllCarriedObjects().Count - 1, false);
	}

    private void ChangeCurrencyAmount(int value)
    {
		if(CharacterController.GetCharacterValue<CharacterType>(CharacterDataType.CharacterType) == CharacterType.Saler)
        {
			int xpValue;
			ExchangeService.DoExchange(CurrencyType.Supply,
				value, ExchangeMethod.UnChecked,
				out xpValue);
		}		
	}
}

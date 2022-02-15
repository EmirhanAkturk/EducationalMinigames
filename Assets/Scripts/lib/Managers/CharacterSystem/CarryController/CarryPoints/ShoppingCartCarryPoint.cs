using System;
using UnityEngine;

public class ShoppingCartCarryPoint : CarryPoint
{
	[Header("Rotations")]
	[SerializeField] private Vector3 representationObjRotation = new Vector3(0, -90, 0);
	private Vector3 verticleRepresentationObjRotation = Vector3.zero;

	[Header("Positions")]
	[SerializeField] private Vector3 firstSlotLocalPosition;
	[SerializeField] private Vector3 verticlefirstSlotLocalPosition;
	[SerializeField] private Vector3 slotZOffset;
	[SerializeField] private Vector3 slotYOffset;
	[SerializeField] private Vector3 slotXOffset;

	private int slotZCapacity;
	private int slotXCapacity;

	private void Awake()
	{
		slotXCapacity = ConfigurationService.Configurations.CollectProductXSlotCapacity;
		slotZCapacity = ConfigurationService.Configurations.CollectProductZSlotCapacity;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		AnimationController.ChangeArmsState(CarryPointType.Hands, true);
	}

	protected override CollectableObject AdjustCarriedObject(CollectableObject carriedObject)
	{
		var carriedGO = base.AdjustCarriedObject(carriedObject).ActiveGameObject;

		Vector3 nextSlotPosition = GetSlotPosition(GetAllCarriedObjects().Count - 1, false); // the minus one is because we add then get next slot which is one count higher

		carriedGO.transform.localPosition = nextSlotPosition;
		carriedGO.transform.localRotation = Quaternion.Euler(representationObjRotation);

		return carriedObject;
	}

	public Vector3 GetNextSlotPosition(bool worldSpace = true, int nexItrCount = 1)
	{
		return GetSlotPosition(GetAllCarriedObjects().Count, worldSpace, nexItrCount - 1);
	}

	public override Vector3 GetTargetPosition()
	{
		return GetSlotPosition(GetAllCarriedObjects().Count, false, 0);
	}

	public Vector3 GetSlotPosition(int slotIndex, bool worldSpace = true, int slotIndexOffset = 0)
	{
		var tmpSlotIndex = slotIndex + slotIndexOffset;
		int area = slotXCapacity * slotZCapacity;
		Vector3 yOffset = (tmpSlotIndex / area) * slotYOffset;
		Vector3 zOffset = ((tmpSlotIndex / slotXCapacity) % slotZCapacity) * slotZOffset;
		Vector3 xOffset = ((tmpSlotIndex % slotXCapacity) - slotXCapacity / 2) * slotXOffset;
		Vector3 slotPosition = firstSlotLocalPosition + yOffset + zOffset + xOffset;
		if (worldSpace) return transform.TransformPoint(slotPosition);
		return slotPosition;
	}

	public override void AddCarriedObject(CollectableObject carriedObject)
	{
		base.AddCarriedObject(carriedObject);
		if (carriedObject.CollectableObjectId == 2) ChangeCurrencyAmount(ConfigurationService.Configurations.MoneyPerOneBale);
	}

	private void ChangeCurrencyAmount(int value)
	{
		int xpValue;
		ExchangeService.DoExchange(CurrencyType.Coin,
			value, ExchangeMethod.UnChecked,
			out xpValue);
	}
}

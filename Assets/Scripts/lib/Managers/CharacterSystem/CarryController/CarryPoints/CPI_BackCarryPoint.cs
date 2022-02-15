using System.Collections;
using UnityEngine;

public class CPI_BackCarryPoint : CarryPoint
{
	[SerializeField] private GameObject backpack;

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
		slotXCapacity = ConfigurationService.Configurations.CollectMoneyXSlotCapacity;
		slotZCapacity = ConfigurationService.Configurations.CollectMoneyZSlotCapacity;
	}

	protected override void OnEnable()
	{
		if (backpack != null) backpack.SetActive(false);
		base.OnEnable();
	}

	protected override void AddRepresentationalObject(PoolType objectType, GameObject go)
	{
		if (backpack != null) backpack.SetActive(true);
		base.AddRepresentationalObject(objectType, go);
	}

	protected override bool RemoveRepresentationalObject(PoolType objectType, GameObject go)
	{
		bool returnValue = base.RemoveRepresentationalObject(objectType, go);
		if (backpack != null && this.gameObject.activeInHierarchy) StartCoroutine(CheckBackpackState());
		if (objectType == PoolType.Money) ChangeCurrencyAmount(-ConfigurationService.Configurations.MoneyPerOneBale);

		return returnValue;
	}

	private IEnumerator CheckBackpackState(float delay = .5f)
	{
		yield return new WaitForSeconds(delay);
		backpack.SetActive(GetAllCarriedObjects().Count > 0);
	}

	protected override CollectableObject AdjustCarriedObject(CollectableObject carriedObject)
	{
		var carriedGO = base.AdjustCarriedObject(carriedObject).ActiveGameObject;

		Vector3 nextSlotPosition = GetSlotPosition((GetAllCarriedObjects().Count - 1)/ ConfigurationService.Configurations.MoneyStackOnBack, false); // the minus one is because we add then get next slot which is one count higher
		
		carriedGO.transform.localPosition = nextSlotPosition;
		carriedGO.transform.localRotation = Quaternion.Euler(ConfigurationService.Configurations.IsMoneyHorizontal ? representationObjRotation : verticleRepresentationObjRotation);

		return carriedObject;
	}

	public override Vector3 GetTargetRotation(bool isAdded = false)
	{
		return ConfigurationService.Configurations.IsMoneyHorizontal
					? new Vector3(0, -90, 0)
					: Vector3.zero;
	}

	public Vector3 GetNextSlotPosition(bool worldSpace = true, int nexItrCount = 1)
	{
		return GetSlotPosition(GetAllCarriedObjects().Count / ConfigurationService.Configurations.MoneyStackOnBack, worldSpace, (nexItrCount - 1) / ConfigurationService.Configurations.MoneyStackOnBack);
	}
	
	public Vector3 GetSlotPosition(int slotIndex, bool worldSpace = true, int slotIndexOffset = 0)
	{
		var tmpXOffset = ConfigurationService.Configurations.IsMoneyHorizontal ? slotXOffset : (slotXOffset / 2);
		var tmpZOffset = ConfigurationService.Configurations.IsMoneyHorizontal ? slotZOffset : ( 3 * slotZOffset);
		var tmpFirstLocalPos = ConfigurationService.Configurations.IsMoneyHorizontal ? firstSlotLocalPosition : ( verticlefirstSlotLocalPosition);
		
		var tmpSlotIndex = slotIndex + slotIndexOffset;
		int area = slotXCapacity * slotZCapacity;
		Vector3 yOffset = (tmpSlotIndex / area) * slotYOffset;
		Vector3 zOffset = ( (tmpSlotIndex / slotXCapacity) % slotZCapacity) * tmpZOffset;
		Vector3 xOffset = ((tmpSlotIndex % slotXCapacity) - slotXCapacity/2)  * tmpXOffset;
		Vector3 slotPosition = tmpFirstLocalPos + yOffset + zOffset + xOffset;
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

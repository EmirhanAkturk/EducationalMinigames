using System.Collections;
using UnityEngine;

public class BackCarryPoint : CarryPoint
{
	[SerializeField] private GameObject backpack;

	[Header("Rotations")]
	[SerializeField] private Vector3 representationObjRotation = new Vector3(0, -90, 0);

	[Header("Positions")]
	[SerializeField] private Vector3 firstSlotLocalPosition;
	[SerializeField] private Vector3 slotOffset;

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

		Vector3 nextSlotPosition = GetSlotPosition(GetAllCarriedObjects().Count - 1, false); // the minus one is because we add then get next slot which is one count higher
		
		carriedGO.transform.localPosition = nextSlotPosition;
		carriedGO.transform.localRotation = Quaternion.Euler(representationObjRotation);

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
}

using UnityEngine;

namespace lib.Managers.SpawnSystem.SpawnPoints
{
    public sealed class MainCharacterSpawnPoint : MonoBehaviour, ISpawnPoint
    {
        public SpawnPointType SpawnPointType => SpawnPointType.MainCharacterSpawnPoint;
        
        [SerializeField] private MiniGameType miniGameType;
    
        private void OnEnable()
        {
            SpawnManager.Instance.AddSpawnPoint(miniGameType, SpawnPointType, this);
        }

        private void OnDisable()
        {
            if(SpawnManager.IsAvailable())
                SpawnManager.Instance.RemoveSpawnPoint(miniGameType, SpawnPointType, this);
        }

        public void Spawn()
        {
            Transform parent = transform;
            Transform spawnedCharacterTr = PoolingSystem.Instance.Create<Transform>(PoolType.MainCharacter, parent);
            // var agent = spawnedCharacterTr.GetComponent<AIBase>();
            // agent.enabled = false;
            // spawnedCharacterTr.localPosition = Vector3.zero;
            // spawnedCharacterTr.localRotation = Quaternion.Euler(Vector3.zero);
            // agent.enabled = true;
            // JoystickBrain mainInteractor = spawnedCharacterTr.GetComponent<JoystickBrain>();
        
            // var followedObjectByCamera = mainInteractor.GetComponent<FollowedObjectByCamera>();
            // followedObjectByCamera.enabled = true;
            // followedObjectByCamera.FollowThisObject();
            //
            // var stacks = GetComponentsInChildren<JointStackNode>();
            //
            // foreach (var item in stacks)
            // {
            //     item.gameObject.transform.parent = parent; //TODO: for test
            // }
        
            // var saleswomanCharacterController = spawnedCharacterTr.GetComponent<ICharacterController>();
            // return new List<ICharacterController>(){saleswomanCharacterController};
        }
    }
}

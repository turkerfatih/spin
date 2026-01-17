using System;
using Cysharp.Threading.Tasks;
using Game.Core;
using Game.Event;
using UnityEngine;

namespace Game
{
    public class RoundSetup:MonoBehaviour
    {
        [SerializeField] private GameObject RoundUIPrefab;
        [SerializeField] private HandManager HandManagerPrefab;
        [SerializeField] private HandLayout HandLayoutPrefab;
        [SerializeField] private SlotMachine SlotMachinePrefab;
        [SerializeField] private SlotMachineView SlotMachineViewPrefab;
        [SerializeField] private CardDragger CardDraggerPrefab;
        [SerializeField] private SlotMachine3dModel MachineModelPrefab;

        private void Awake()
        {
            EventBus.OnLightValueChanged?.Invoke(1f);
            EventBus.OnLightRotationChanged?.Invoke(Vector3.zero);
        }

        private void Start()
        {
            var machineModel = Instantiate(MachineModelPrefab);
            var cardDragger=Instantiate(CardDraggerPrefab);
            var roundUI = Instantiate(RoundUIPrefab);
            var handLayout = Instantiate(HandLayoutPrefab);
            var slotMachine = Instantiate(SlotMachinePrefab);
            var slotMachineView = Instantiate(SlotMachineViewPrefab);
            var handManager = Instantiate(HandManagerPrefab);
            EventBus.RoundDataChange?.Invoke(Services.Round);
            Services.Hand = handManager;
            Services.Hand.View=handLayout;
            handLayout.SetDragger(cardDragger);
            slotMachine.Setup();
            slotMachineView.Setup();
            RoundStartAnimation().Forget();
        }

        private async UniTask RoundStartAnimation()
        {
            await Services.RoundStack.AnimateSetup(Services.Round.BeginCoinCount);
            await Services.DebtMeter.AnimateDebtSetup();
            await Services.Darabas.AnimateOpen();
        }
        
        
    }
}
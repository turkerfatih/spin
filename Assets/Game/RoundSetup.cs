using System;
using Game.Core;
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


        private void Start()
        {
            var cardDragger=Instantiate(CardDraggerPrefab);
            var roundUI = Instantiate(RoundUIPrefab);
            var handLayout = Instantiate(HandLayoutPrefab);
            var slotMachine = Instantiate(SlotMachinePrefab);
            var slotMachineView = Instantiate(SlotMachineViewPrefab);
            var handManager = Instantiate(HandManagerPrefab);
            Services.Hand = handManager;
            Services.Hand.View=handLayout;
            handLayout.SetDragger(cardDragger);
            slotMachine.Setup();
            slotMachineView.Setup();
            handManager.StartDraw();
        }
    }
}
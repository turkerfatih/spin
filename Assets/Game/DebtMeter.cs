using System;
using Cysharp.Threading.Tasks;
using Game.Odometer;
using UnityEngine;

namespace Game
{
    public class DebtMeter:MonoBehaviour
    {
        private MeterController meterController;
        private void Awake()
        {
            meterController = GetComponent<MeterController>();
            Services.DebtMeter = this;
        }

        public async UniTask AnimateDebtSetup()
        {
            await meterController.Setup(Services.Round.Debt);
        }

        public async UniTask Reduce(int amount)
        {
            await meterController.Reduce(amount);
        }


    }
}
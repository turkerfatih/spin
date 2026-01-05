using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Core.Animation
{
    public class AnimationSequencer : MonoBehaviour
    {
        private SpringHandle<Vector3, Vector3Spring,Vector3> _posHandle;

        private void Awake()
        {
            var spring = new Vector3Spring { AngularFrequency = 10f, DampingRatio = 0.5f };
            _posHandle = new SpringHandle<Vector3, Vector3Spring,Vector3>(
                this,
                spring, 
                val => transform.position = val
            );
        }

        [ContextMenu("Run Sequence")]
        public async UniTaskVoid RunSquareSequence()
        {
            Vector3 startPos = transform.position;

            // 1. Move Right
            await _posHandle.Play(startPos + Vector3.right * 2);
        
            // 2. Move Up
            await _posHandle.Play(startPos + Vector3.right * 2 + Vector3.up * 2);
        
            // 3. Move Left
            await _posHandle.Play(startPos + Vector3.up * 2);
        
            // 4. Return Home
            await _posHandle.Play(startPos);
        
            Debug.Log("Sequence Complete!");
        }
    }
}
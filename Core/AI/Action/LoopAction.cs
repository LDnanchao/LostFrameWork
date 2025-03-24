using System.Collections;
using UnityEngine;

namespace Lost.AI
{
    public class LoopAction : TaskActionBase
    {
        private Coroutine _coroutine;
        public void AddLoopPosition(Vector3 vector)
        {

        }
        public override void Enter()
        {
            base.Enter();
            Debug.Log("LoopAction Enter");
            _coroutine = GetOwner().StartCoroutine(Loop());
        }
        private IEnumerator Loop()
        {
            yield return new WaitForSeconds(2);
            Finish();
            Debug.Log("LoopAction Finished");
        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log("LoopAction Exit");
            //结束后，清空移动
            GetOwner().StopCoroutine(_coroutine);
        }
    }

}
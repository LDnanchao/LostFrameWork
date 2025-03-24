using System.Collections;
using UnityEngine;

namespace Lost.AI
{
    public class FollowTargetAction : TaskActionBase
    {
        private Coroutine searchCoroutine;
        public override void Enter()
        {
            base.Enter();
            Debug.Log("FollowTargetAction Enter");
            searchCoroutine = GetOwner().StartCoroutine(Search());
        }
        public override void Update()
        {
            base.Update();
        }

        private IEnumerator Search()
        {
            yield return new WaitForSeconds(2);
            Finish();
        }
        public override void Exit()
        {
            base.Exit();
            GetOwner().StopCoroutine(searchCoroutine);
            Debug.Log("FollowTargetAction Exit");
        }
    }

}
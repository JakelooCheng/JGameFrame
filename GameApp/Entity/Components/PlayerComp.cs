using Game.Base.EC;
using Game.Base.LifeCycle;
using GameApp.Cache;
using GameApp.Utils;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;
using UnityEngine.AI;
using static Codice.Client.Common.WebApi.WebApiEndpoints;

namespace GameApp.Entitys
{
    public class PlayerComp : MapUnitComp, IUpdate, IDialogAble
    {
        private PrefabComp prefabComp;

        private GameObjectComp gameObjectComp;

        private Animator playerAnimator;

        public float DialogOffset => 1;

        private NavMeshAgent agent;
        private RaycastHit hitInfo = new RaycastHit();

        /// <summary>
        /// Player 有没有被暂停
        /// </summary>
        public bool IsRunning => agent.enabled || IsSitting;

        public bool IsSitting = false;

        public override void InitAfter()
        {
            base.InitAfter();

            prefabComp = GetComp<PrefabComp>();
            gameObjectComp = GetComp<GameObjectComp>();

            prefabComp.Subscribe(OnPrefabLoaded);
        }

        public void OnUpdate()
        {
            UpdateInput(); // 更新移动
            UpdateRotation();
        }

        private void UpdateInput()
        {
            if (agent == null || !agent.enabled)
            {
                return;
            }

            /// 鼠标点击移动
            if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
                {
                    agent.velocity = Vector3.zero;
                    agent.isStopped = false;
                    agent.SetDestination(hitInfo.point);
                    /// 设置方向
                    var cameraForward = GameCache.Player.CameraMovement.transform.rotation.eulerAngles;
                    var target = Vector3.Dot(cameraForward, agent.transform.position - agent.destination);
                    gameObjectComp.Transform.localScale = new Vector3(target * cameraForward.y > 0 ? 1 : -1, 1, 1);
                }
            }

            /// 键盘控制移动
            bool isControlRunning = false;
            var offset = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            if (Vector3.SqrMagnitude(offset) > 0)
            {
                agent.isStopped = true;
                agent.Move(GameCache.Player.CameraMovement.transform.rotation * offset 
                    * Time.deltaTime * ConstValue.PlayerMoveSpeed * agent.speed);
                isControlRunning = true;
                /// 设置方向
                gameObjectComp.Transform.localScale = new Vector3(offset.x > 0 ? 1 : -1, 1, 1);
            }

            UpdateAnima(isControlRunning);
        }

        private void UpdateAnima(bool isControlRunning)
        {
            /// 有速度时移动
            playerAnimator.SetBool("Running", agent.velocity.sqrMagnitude > agent.speed / 5 || isControlRunning);
        }

        private void UpdateRotation()
        {
            if (agent != null)
            {
                gameObjectComp.Transform.GetChild(0).rotation = Camera.main.transform.rotation;
            }
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Stop()
        {
            agent.enabled = false;
        }

        /// <summary>
        /// 启动
        /// </summary>
        public void Run()
        {
            agent.enabled = true;
        }

        /// <summary>
        /// Player 预制体加载完成
        /// </summary>
        private void OnPrefabLoaded(GameObject target)
        {
            prefabComp.Unsubscribe(OnPrefabLoaded);
            agent = target.GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            playerAnimator = target.GetComponent<Animator>();
        }

        public void OnDialog()
        {
            playerAnimator.SetTrigger("Speak");
        }

        public void OnSit(bool state)
        {
            playerAnimator.SetBool("Sitting", state);
        }
    }
}
using UnityEngine;

namespace BossSystem
{
    public interface IBossRuntimeAbility
    {
        void Init(BossController controller);
        // 사이클마다 1회 호출(예: 20초마다)
        void OnCycle();
        // 매 프레임 업데이트(필요 없으면 빈 구현)
        void Tick(float deltaTime) { }
        // 보스 사망/파괴 시 정리
        void Dispose() { }
    }

    // 선택: 공통 유틸/기본구현을 두고 싶다면
    public abstract class BossRuntimeAbilityBase : IBossRuntimeAbility
    {
        protected BossController Controller { get; private set; }

        public virtual void Init(BossController controller) => Controller = controller;
        public virtual void OnCycle() { }
        public virtual void Tick(float deltaTime) { }
        public virtual void Dispose() { }
    }
}

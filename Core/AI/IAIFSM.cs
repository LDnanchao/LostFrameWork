

namespace Lost.AI
{
    public interface IAIFSM
    {
        public void Start();

        public void Update();
        public void Stop();
        public string GetCurrentState();
        public AIControllerBase GetAIController();
    }
}

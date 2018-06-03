namespace LoESoft.GameServer.networking.handlers
{
    public class ClosedTestHandler
    {
        private Client Client
        { get; set; }

        public ClosedTestHandler(Client Client)
        { this.Client = Client; }

        public bool Validate()
        {
            Client.Player.ApplyConditionEffect(ConditionEffectIndex.Paused);

            Client.Player.SendInfo("The server is validating your request, please wait.");

            return false;
        }

        //public void 
    }
}

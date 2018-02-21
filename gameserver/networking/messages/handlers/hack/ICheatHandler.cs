namespace LoESoft.GameServer.networking.messages.handlers.hack
{
    public interface ICheatHandler
    {
        CheatID ID { get; }
        void Handler();
    }

    public enum CheatID : byte
    {
        DEXTERITY = 0,
        GOD = 1
    }
}
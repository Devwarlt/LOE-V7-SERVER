#region

using gameserver.realm;
using gameserver.realm.entity;

#endregion

namespace gameserver.logic.behaviors
{
    public class SetAltTexture : Behavior
    {
        class TextureState
        {
            public int currentTexture;
            public int remainingTime;
        }

        private readonly int _indexMin;
        private readonly int _indexMax;
        private Cooldown _cooldown;
        private readonly bool _loop;

        public SetAltTexture(
            int minValue,
            int maxValue = -1,
            Cooldown cooldown = new Cooldown(),
            bool loop = false
            )
        {
            _indexMin = minValue;
            _indexMax = maxValue;
            _cooldown = cooldown.Normalize(0);
            _loop = loop;
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            state = new TextureState()
            {
                currentTexture = (host as Enemy).AltTextureIndex,
                remainingTime = _cooldown.Next(Random)
            };
            if ((host as Enemy).AltTextureIndex != _indexMin)
            {
                (host as Enemy).AltTextureIndex = _indexMin;
                (state as TextureState).currentTexture = _indexMin;
            }
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            var textState = state as TextureState;

            if (_indexMax == -1 || (textState.currentTexture == _indexMax && !_loop))
                return;

            if (textState.remainingTime <= 0)
            {
                int newTexture = (textState.currentTexture >= _indexMax) ? _indexMin : textState.currentTexture + 1;
                (host as Enemy).AltTextureIndex = newTexture;
                textState.currentTexture = newTexture;
                textState.remainingTime = _cooldown.Next(Random);
            }
            else
            {
                textState.remainingTime -= time.ElapsedMsDelta;
            }
        }
    }
}
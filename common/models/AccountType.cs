using System.Collections.Generic;

namespace common.config
{
    public enum accountType:int
    {
        FREE_ACCOUNT = 0,
        VIP_ACCOUNT = 1,
        LEGENDS_OF_LOE_ACCOUNT = 2,
        TUTOR_ACCOUNT = 3,
        LOESOFT_ACCOUNT = 4
    }

    public class AccountTypePerks
    {
        private accountType _accountType { get; set; }
        private bool _accessToDrastaCitadel { get; set; }
        private bool _byPassKeysRequirements { get; set; }
        private bool _byPassEggsRequirements { get; set; }
        private bool _priorityToLogin { get; set; }

        public AccountTypePerks(int accountType)
        {
            _accountType = (accountType) accountType;
            _accessToDrastaCitadel = _accountType > config.accountType.VIP_ACCOUNT;
            _byPassKeysRequirements = _accountType > config.accountType.VIP_ACCOUNT;
            _byPassEggsRequirements = _accountType > config.accountType.VIP_ACCOUNT;
            _priorityToLogin = _accountType > config.accountType.VIP_ACCOUNT;
        }

        private List<accountType> Boosted = new List<accountType>
        {
            accountType.VIP_ACCOUNT,
            accountType.LEGENDS_OF_LOE_ACCOUNT
        };

        public int Experience(int level, int experience)
        {
            if (_accountType == accountType.VIP_ACCOUNT)
                return level < 20 ? (int) (experience * 1.5) : (int) (experience * 1.05);
            if (_accountType == accountType.LEGENDS_OF_LOE_ACCOUNT)
                return level < 20 ? (experience * 2) : (int) (experience * 1.1);
            return experience;
        }

        public int StatsBoost(int stat, int boost) => stat / (_accountType == accountType.VIP_ACCOUNT ? 10 : _accountType == accountType.LEGENDS_OF_LOE_ACCOUNT ? 20 / 3 : 1) + boost * (!Boosted.Contains(_accountType) ? 0 : 1);

        public bool AccessToDrastaCitadel() => _accessToDrastaCitadel;

        public bool ByPassKeysRequirements() => _byPassKeysRequirements;

        public bool ByPassEggsRequirements() => _byPassEggsRequirements;

        public bool PriorityToLogin() => _priorityToLogin;

        public ConditionEffect SetAccountTypeIcon()
        {            
            ConditionEffect icon = new ConditionEffect();
            icon.DurationMS = -1;

            switch (_accountType)
            {
                case accountType.FREE_ACCOUNT:
                    icon.Effect = ConditionEffectIndex.FreeAccount;
                    break;
                case accountType.VIP_ACCOUNT:
                    icon.Effect = ConditionEffectIndex.VipAccount;
                    break;
                case accountType.LEGENDS_OF_LOE_ACCOUNT:
                    icon.Effect = ConditionEffectIndex.LegendsofLoEAccount;
                    break;
                case accountType.TUTOR_ACCOUNT:
                    icon.Effect = ConditionEffectIndex.TutorAccount;
                    break;
                case accountType.LOESOFT_ACCOUNT:
                    icon.Effect = ConditionEffectIndex.LoESoftAccount;
                    break;
            }

            return icon;
        }

        public int MerchantDiscount() => _accountType == accountType.VIP_ACCOUNT ? 9 / 10 : _accountType == accountType.LEGENDS_OF_LOE_ACCOUNT ? 8 / 10 : 1;
    }
}
namespace LoESoft.Core.config
{
    public enum AccountType : int
    {
        FREE_ACCOUNT = 0,
        VIP_ACCOUNT = 1,
        LEGENDS_OF_LOE_ACCOUNT = 2,
        TUTOR_ACCOUNT = 3,
        LOESOFT_ACCOUNT = 4
    }

    public class AccountTypePerks
    {
        private AccountType _accountType { get; set; }
        private bool _accessToDrastaCitadel { get; set; }
        private bool _byPassKeysRequirements { get; set; }
        private bool _byPassEggsRequirements { get; set; }
        private bool _priorityToLogin { get; set; }

        public AccountTypePerks(int accountType)
        {
            _accountType = (AccountType)accountType;
            _accessToDrastaCitadel = _accountType > config.AccountType.VIP_ACCOUNT;
            _byPassKeysRequirements = _accountType > config.AccountType.VIP_ACCOUNT;
            _byPassEggsRequirements = _accountType > config.AccountType.VIP_ACCOUNT;
            _priorityToLogin = _accountType > config.AccountType.VIP_ACCOUNT;
        }

        public int Experience(int level, int experience)
        {
            if (_accountType == AccountType.VIP_ACCOUNT)
                return level < 20 ? (int)(experience * 1.5) : (int)(experience * 1.05);
            if (_accountType == AccountType.LEGENDS_OF_LOE_ACCOUNT)
                return level < 20 ? (experience * 2) : (int)(experience * 1.1);
            return experience;
        }

        public bool AccessToDrastaCitadel() => _accessToDrastaCitadel;

        public bool ByPassKeysRequirements() => _byPassKeysRequirements;

        public bool ByPassEggsRequirements() => _byPassEggsRequirements;

        public bool PriorityToLogin() => _priorityToLogin;

        public ConditionEffect SetAccountTypeIcon()
        {
            ConditionEffect icon = new ConditionEffect
            {
                DurationMS = -1
            };

            switch (_accountType)
            {
                case AccountType.FREE_ACCOUNT:
                    icon.Effect = ConditionEffectIndex.FreeAccount;
                    break;
                case AccountType.VIP_ACCOUNT:
                    icon.Effect = ConditionEffectIndex.VipAccount;
                    break;
                case AccountType.LEGENDS_OF_LOE_ACCOUNT:
                    icon.Effect = ConditionEffectIndex.LegendsofLoEAccount;
                    break;
                case AccountType.TUTOR_ACCOUNT:
                    icon.Effect = ConditionEffectIndex.TutorAccount;
                    break;
                case AccountType.LOESOFT_ACCOUNT:
                    icon.Effect = ConditionEffectIndex.LoESoftAccount;
                    break;
            }

            return icon;
        }

        public double MerchantDiscount() => _accountType == AccountType.VIP_ACCOUNT ? 0.9 : _accountType == AccountType.LEGENDS_OF_LOE_ACCOUNT ? 0.8 : 1;
    }
}
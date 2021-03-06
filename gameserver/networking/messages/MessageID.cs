namespace LoESoft.GameServer
{
    public enum MessageID : int
    {
        FAILURE = 0,
        SERVERPLAYERSHOOT = 1,
        CTT_SEND = 2,
        USEITEM = 3,
        QUESTOBJID = 4,
        TELEPORT = 5,
        OTHERHIT = 6,
        AOE = 7,
        PING = 8,
        PLAYERTEXT = 9,
        SHOOTACK = 10,
        CREATEGUILD = 11,
        DEATH = 12,
        RESKIN_UNLOCK = 13,
        INVITEDTOGUILD = 14,
        ACCEPT_ARENA_DEATH = 15,
        ESCAPE = 16,
        PLAYSOUND = 17,
        INVRESULT = 18,
        KEY_INFO_RESPONSE = 19,
        NOTIFICATION = 20,
        PETYARDUPDATE = 21,
        CANCELTRADE = 22,
        USEPORTAL = 23,
        MOVE = 24,
        CHOOSENAME = 25,
        ACCEPTTRADE = 26,
        CHECKCREDITS = 27,
        MAPINFO = 28,
        CTT_RECEIVE = 29,
        HATCH_PET = 30,
        NEWTICK = 31,
        URL = 32,
        FILE = 33,
        TEXT = 34,
        TRADEDONE = 35,
        SETCONDITION = 36,
        PLAYERHIT = 37,
        TRADECHANGED = 38,
        ACTIVEPETUPDATE = 39,
        GLOBAL_NOTIFICATION = 40,
        PLAYERSHOOT = 41,
        PET_CHANGE_FORM_MSG = 42,

        // Unregistered MessageID '43',
        UPDATE = 44,

        ENTER_ARENA = 45,
        RESKIN = 46,
        ACTIVE_PET_UPDATE_REQUEST = 47,
        CREATE = 48,
        ALLYSHOOT = 49,
        DELETE_PET = 50,
        TRADEREQUESTED = 51,
        DAMAGE = 52,
        ACCOUNTLIST = 53,

        // Unregistered MessageID '54',
        ARENA_DEATH = 55,

        BUYRESULT = 56,
        CLIENTSTAT = 57,
        CREATE_SUCCESS = 58,
        SQUAREHIT = 59,

        // Unregistered MessageID '60',
        // Unregistered MessageID '61',
        NAMERESULT = 62,

        LOAD = 63,
        INVSWAP = 64,
        IMMINENT_ARENA_WAVE = 65,
        KEY_INFO_REQUEST = 66,
        JOINGUILD = 67,
        RECONNECT = 68,
        EVOLVE_PET = 69,

        // Unregistered MessageID '70',
        // Unregistered MessageID '71',
        // Unregistered MessageID '72',
        // Unregistered MessageID '73',
        TRADESTART = 74,

        GUILDREMOVE = 75,
        NEW_ABILITY = 76,
        BUY = 77,
        SHOWEFFECT = 78,
        PETUPGRADEREQUEST = 79,
        VERIFY_EMAIL = 80,
        CHANGEGUILDRANK = 81,
        REQUESTTRADE = 82,
        PONG = 83,
        GROUNDDAMAGE = 84,
        GUILDINVITE = 85,
        HELLO = 86,
        EDITACCOUNTLIST = 87,
        PIC = 88,
        AOEACK = 89,
        ENEMYSHOOT = 90,
        QUEST_FETCH_ASK = 91,
        GOTO = 92,
        QUEST_REDEEM_RESPONSE = 93,
        ENEMYHIT = 94,
        GUILDRESULT = 95,
        UPDATEACK = 96,
        INVDROP = 97,

        // Unregistered MessageID '98',
        GOTOACK = 99,

        TRADEACCEPTED = 100,
        CHANGETRADE = 101,

        // Unregistered MessageID '102',
        // Unregistered MessageID '103',
        // Unregistered MessageID '104',
        // Unregistered MessageID '105',
        // Unregistered MessageID '106',
        // Unregistered MessageID '107',
        // Unregistered MessageID '108',
        // Unregistered MessageID '109',
        // Unregistered MessageID '110',
        // Unregistered MessageID '111',
        // Unregistered MessageID '112',
        // Unregistered MessageID '113',
        // Unregistered MessageID '114',
        // Unregistered MessageID '115',
        // Unregistered MessageID '116',
        // Unregistered MessageID '117',
        // Unregistered MessageID '118',
        // Unregistered MessageID '119',
        // Unregistered MessageID '120',
        // Unregistered MessageID '121',
        // Unregistered MessageID '122',
        // Unregistered MessageID '123',
        // Unregistered MessageID '124',
        // Unregistered MessageID '125',
        // Unregistered MessageID '126',
        // Unregistered MessageID '127',
        // Unregistered MessageID '128',
        // Unregistered MessageID '129',
        // Unregistered MessageID '130',
        // Unregistered MessageID '131',
        // Unregistered MessageID '132',
        // Unregistered MessageID '133',
        // Unregistered MessageID '134',
        // Unregistered MessageID '135',
        // Unregistered MessageID '136',
        // Unregistered MessageID '137',
        // Unregistered MessageID '138',
        // Unregistered MessageID '139',
        // Unregistered MessageID '140',
        // Unregistered MessageID '141',
        // Unregistered MessageID '142',
        // Unregistered MessageID '143',
        // Unregistered MessageID '144',
        // Unregistered MessageID '145',
        // Unregistered MessageID '146',
        // Unregistered MessageID '147',
        // Unregistered MessageID '148',
        // Unregistered MessageID '149',
        SWITCH_MUSIC = 150,

        CLAIM_LOGIN_REWARD_MSG = 151,
        LOGIN_REWARD_MSG = 152,
        // Unregistered MessageID '153',
        // Unregistered MessageID '154',
        // Unregistered MessageID '155',
        // Unregistered MessageID '156',
        // Unregistered MessageID '157',
        // Unregistered MessageID '158',
        // Unregistered MessageID '159',
        // Unregistered MessageID '160',
        // Unregistered MessageID '161',
        // Unregistered MessageID '162',
        // Unregistered MessageID '163',
        // Unregistered MessageID '164',
        // Unregistered MessageID '165',
        // Unregistered MessageID '166',
        // Unregistered MessageID '167',
        // Unregistered MessageID '168',
        // Unregistered MessageID '169',
        // Unregistered MessageID '170',
        // Unregistered MessageID '171',
        // Unregistered MessageID '172',
        // Unregistered MessageID '173',
        // Unregistered MessageID '174',
        // Unregistered MessageID '175',
        // Unregistered MessageID '176',
        // Unregistered MessageID '177',
        // Unregistered MessageID '178',
        // Unregistered MessageID '179',
        // Unregistered MessageID '180',
        // Unregistered MessageID '181',
        // Unregistered MessageID '182',
        // Unregistered MessageID '183',
        // Unregistered MessageID '184',
        // Unregistered MessageID '185',
        // Unregistered MessageID '186',
        // Unregistered MessageID '187',
        // Unregistered MessageID '188',
        // Unregistered MessageID '189',
        // Unregistered MessageID '190',
        // Unregistered MessageID '191',
        // Unregistered MessageID '192',
        // Unregistered MessageID '193',
        // Unregistered MessageID '194',
        // Unregistered MessageID '195',
        // Unregistered MessageID '196',
        // Unregistered MessageID '197',
        // Unregistered MessageID '198',
        // Unregistered MessageID '199',
        // Unregistered MessageID '200',
        // Unregistered MessageID '201',
        // Unregistered MessageID '202',
        // Unregistered MessageID '203',
        // Unregistered MessageID '204',
        // Unregistered MessageID '205',
        // Unregistered MessageID '206',
        // Unregistered MessageID '207',
        // Unregistered MessageID '208',
        // Unregistered MessageID '209',
        // Unregistered MessageID '210',
        // Unregistered MessageID '211',
        // Unregistered MessageID '212',
        // Unregistered MessageID '213',
        // Unregistered MessageID '214',
        // Unregistered MessageID '215',
        // Unregistered MessageID '216',
        // Unregistered MessageID '217',
        // Unregistered MessageID '218',
        // Unregistered MessageID '219',
        // Unregistered MessageID '220',
        // Unregistered MessageID '221',
        // Unregistered MessageID '222',
        // Unregistered MessageID '223',
        // Unregistered MessageID '224',
        // Unregistered MessageID '225',
        // Unregistered MessageID '226',
        // Unregistered MessageID '227',
        // Unregistered MessageID '228',
        // Unregistered MessageID '229',
        // Unregistered MessageID '230',
        // Unregistered MessageID '231',
        // Unregistered MessageID '232',
        // Unregistered MessageID '233',
        // Unregistered MessageID '234',
        // Unregistered MessageID '235',
        // Unregistered MessageID '236',
        // Unregistered MessageID '237',
        // Unregistered MessageID '238',
        // Unregistered MessageID '239',
        // Unregistered MessageID '240',
        // Unregistered MessageID '241',
        // Unregistered MessageID '242',
        // Unregistered MessageID '243',
        // Unregistered MessageID '244',
        // Unregistered MessageID '245',
        // Unregistered MessageID '246',
        // Unregistered MessageID '247',
        // Unregistered MessageID '248',
        // Unregistered MessageID '249',
        // Unregistered MessageID '250',
        // Unregistered MessageID '251',
        // Unregistered MessageID '252',
        // Unregistered MessageID '253',
        // Unregistered MessageID '254',
        // Unregistered MessageID '255'
    }
}
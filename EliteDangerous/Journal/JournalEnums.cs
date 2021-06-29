﻿/*
 * Copyright © 2016-2019 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 * 
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */

namespace EliteDangerousCore
{
    public enum JournalTypeEnum
    {
        Unknown = 0,

        AfmuRepairs = 3,
        ApproachBody = 4,
        ApproachSettlement = 5,
        AppliedToSquadron = 6,
        AsteroidCracked = 7,
        Backpack = 4000,
        BackpackChange = 4030,
        BookDropship = 4001,
        BookTaxi = 4002,
        Bounty = 10,
        BuyAmmo = 20,
        BuyDrones = 30,
        BuyExplorationData = 40,
        BuyMicroResources = 4003,
        BuySuit = 4004,
        BuyWeapon = 4005,
        BuyTradeData = 50,
        CancelDropship = 4006,
        CancelTaxi = 4007,
        CapShipBond = 60,
        Cargo = 63,
        CargoDepot = 64,
        CargoTransfer = 1370,
        CarrierBankTransfer = 1300,
        CarrierBuy = 1305,
        CarrierCancelDecommission = 1310,
        CarrierCrewServices = 1315,
        CarrierDecommission = 1320,
        CarrierDepositFuel = 1325,
        CarrierDockingPermission = 1330,
        CarrierFinance = 1335,
        CarrierJumpRequest = 1340,
        CarrierJump = 1345,
        CarrierModulePack = 1350,
        CarrierShipPack = 1355,
        CarrierStats = 1360,
        CarrierTradeOrder = 1365,
        CarrierNameChange = 1366,
        CarrierJumpCancelled = 1367,
        ChangeCrewRole = 65,
        ClearSavedGame = 70,
        CockpitBreached = 80,
        CodexEntry = 85,
        CollectCargo = 90,
        CollectItems = 4008,
        Commander = 95,
        CommitCrime = 100,
        CommunityGoal = 109,
        CommunityGoalJoin = 110,
        CommunityGoalReward = 120,
        CommunityGoalDiscard = 1040,
        Continued = 125,
        CreateSuitLoadout = 4009,
        CrewAssign = 126,
        CrewFire = 127,
        CrewHire = 128,
        CrewLaunchFighter = 1268,
        CrewMemberJoins = 1270,
        CrewMemberQuits = 1280,
        CrewMemberRoleChange = 1285,
        CrimeVictim = 129,
        DataScanned = 1030,
        DatalinkScan = 130,
        DatalinkVoucher = 1020,
        DeleteSuitLoadout = 4010,
        Died = 140,
        DiscoveryScan = 141,
        DisbandedSquadron = 142,
        Disembark = 4011,
        Docked = 145,
        DockFighter = 150,
        DockingCancelled = 160,
        DockingDenied = 170,
        DockingGranted = 180,
        DockingRequested = 190,
        DockingTimeout = 200,
        DockSRV = 210,
        DropItems = 4012,
        DropshipDeploy = 4050,
        EjectCargo = 220,
        EndCrewSession = 225,
        EngineerApply = 230,
        EngineerContribution = 235,
        EngineerCraft = 240,
        EngineerLegacyConvert = 241,
        EngineerProgress = 250,
        Embark = 4013,
        EscapeInterdiction = 260,
        FactionKillBond = 270,
        FetchRemoteModule = 1000,
        FSDJump = 280,
        FSDTarget = 281,
        FSSAllBodiesFound = 285,
        FuelScoop = 290,
        Fileheader = 300,
        FighterDestroyed = 303,
        FighterRebuilt = 304,
        Friends = 305,
        FSSDiscoveryScan = 306,
        FSSSignalDiscovered = 307,
        HeatDamage = 310,
        HeatWarning = 320,
        HullDamage = 330,
        Interdicted = 340,
        Interdiction = 350,
        InvitedToSquadron = 351,
        JoinedSquadron = 353,
        JetConeBoost = 354,
        JetConeDamage = 355,
        JoinACrew = 356,
        KickCrewMember = 357,
        KickedFromSquadron = 358,
        LaunchDrone = 359,
        LaunchFighter = 360,
        LaunchSRV = 370,
        LeftSquadron = 371,
        LeaveBody = 375,
        Liftoff = 380,
        LoadGame = 390,
        Loadout = 395,
        LoadoutEquipModule = 4014,
        LoadoutRemoveModule = 4015,
        Location = 400,
        MassModuleStore = 1010,
        Market = 405,
        MarketBuy = 410,
        MarketSell = 420,
        MaterialCollected = 430,
        MaterialDiscarded = 440,
        MaterialDiscovered = 450,
        MaterialTrade = 451,
        Materials = 455,
        MiningRefined = 460,
        Missions = 465,
        MissionAbandoned = 470,
        MissionAccepted = 480,
        MissionCompleted = 490,
        MissionFailed = 500,
        MissionRedirected = 505,
        ModuleInfo = 508,
        ModuleBuy = 510,
        ModuleRetrieve = 515,
        ModuleSell = 520,
        ModuleSellRemote = 990,
        ModuleStore = 525,
        ModuleSwap = 530,
        MultiSellExplorationData = 533,
        Music = 535,
        NavBeaconScan = 538,
        NavRoute = 1400,
        NewCommander = 540,
        NpcCrewPaidWage = 541,
        NpcCrewRank = 542,
        Outfitting = 543,
        Passengers = 545,
        PayFines = 550,
        PayBounties = 551,
        PayLegacyFines = 560,
        Powerplay = 565,
        PowerplayCollect = 570,
        PowerplayDefect = 580,
        PowerplayDeliver = 590,
        PowerplayFastTrack = 600,
        PowerplayJoin = 610,
        PowerplayLeave = 620,
        PowerplaySalary = 630,
        PowerplayVote = 640,
        PowerplayVoucher = 650,
        Progress = 660,
        Promotion = 670,
        ProspectedAsteroid = 673,
        PVPKill = 675,
        QuitACrew = 677,
        Rank = 680,
        RebootRepair = 690,
        ReceiveText = 700,
        RedeemVoucher = 710,
        RefuelAll = 720,
        RefuelPartial = 730,
        RenameSuitLoadout = 4016,
        Repair = 740,
        RepairAll = 745,
        RepairDrone = 747,
        Reputation = 748,
        RestockVehicle = 750,
        Resurrect = 760,
        ReservoirReplenished = 763,
        SAAScanComplete = 765,
        SAASignalsFound = 766,
        Scan = 770,
        ScanOrganic = 4017,
        Scanned = 772,
        ScientificResearch = 775,
        Screenshot = 780,
        SearchAndRescue = 785,
        SelfDestruct = 790,
        SellDrones = 800,
        SellExplorationData = 810,
        SellMicroResources = 4018,
        SellOrganicData = 4019,
        SellShipOnRebuy = 815,
        SellSuit = 4020,
        SellWeapon = 4021,
        SendText = 820,
        SetUserShipName = 825,
        SharedBookmarkToSquadron = 826,
        ShieldState = 830,
        Shipyard = 837,
        ShipyardBuy = 840,
        ShipyardNew = 850,
        ShipyardSell = 860,
        ShipyardSwap = 870,
        ShipyardTransfer = 880,
        ShipTargeted = 881,
        ShipLocker = 4031,
        Shutdown = 882,
        SRVDestroyed = 883,
        StartJump = 885,
        Statistics = 888,
        StoredModules = 886,
        StoredShips = 887,
        SupercruiseEntry = 890,
        SquadronCreated = 891,
        SquadronDemotion = 892,
        SquadronPromotion = 893,
        SquadronStartup = 894,
        SupercruiseExit = 900,
        SwitchSuitLoadout = 4023,
        SuitLoadout = 4029,
        Synthesis = 910,
        SystemsShutdown = 915,
        TechnologyBroker = 918,
        Touchdown = 920,
        TradeMicroResources = 4024,
        UnderAttack = 925,
        Undocked = 930,
        UpgradeSuit = 4025,
        UpgradeWeapon = 4026,
        UseConsumable = 4027,
        USSDrop = 940,
        VehicleSwitch = 950,
        WingAdd = 960,
        WingInvite = 965,
        WingJoin = 970,
        WingLeave = 980,
        WonATrophyForSquadron = 985,

        // EDD Entries

        EDDItemSet = 2000,
        EDDCommodityPrices = 2010,

        // below are not events currently supported or icon rename events

        ObsoleteOrIcons = 10000,

        // removed events
        TransferMicroResources = 11000,
        BackPack = 11001,
        ShipLockerMaterials = 11002,
        BackPackMaterials = 11003,

        // Specials Event IDs for ICON selection - alternate Icons for these events
        RestockVehicle_SRV = 10750,
        RestockVehicle_Fighter = 10751,
        ShieldState_ShieldsUp = 10830,
        ShieldState_ShieldsDown = 10831,
        VehicleSwitch_Mothership = 10950,
        VehicleSwitch_Fighter = 10951,
        DisembarkSRV = 10960,
        EmbarkSRV = 10961,
    }
}


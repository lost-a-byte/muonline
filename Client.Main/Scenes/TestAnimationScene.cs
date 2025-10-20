using System.Collections.Immutable;
using Client.Main.Controls.UI;
using Client.Main.Controls.UI.Game;
using Client.Main.Controls.UI.Game.Inventory;
using Client.Main.Core.Utilities;
using Client.Main.Models;
using Client.Main.Objects.Vehicle;
using Client.Main.Worlds;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using MUnique.OpenMU.Network.Packets;


namespace Client.Main.Scenes;

public class TestAnimationScene : BaseScene
{
    enum TestAnimationUiState
    {
        Loading,
        EditCharacter,
        TestAction,
    }
    // Fields
    TestAnimationUiState _uiState = TestAnimationUiState.Loading;
    private TestAnimationUiState UiState
    {
        get => _uiState;
        set
        {
            if (_uiState == value)
            {
                return;
            }
            _uiState = value;
            RefreshUI();
            // TODO: refresh
        }
    }


    private int wing;
    public int Wing
    {
        get => wing;
        set => wing = value;
    }

    private int leftHand;
    public int LeftHand
    {
        get => leftHand;
        set => leftHand = value;
    }
    private int rightHand;
    public int RightHand
    {
        get => rightHand;
        set => rightHand = value;
    }
    private int pet;
    public int Pet
    {
        get => pet;
        set => pet = value;
    }
    private int armorSet;
    public int ArmorSet
    {
        get => armorSet;
        set => armorSet = value;
    }
    private int ride;
    public int Ride
    {
        get => ride;
        set => ride = value;
    }

    // CONTROLS
    private SelectWorld _selectWorld;

    private readonly ILogger<TestAnimationScene> _logger;
    private LoadingScreenControl _loadingScreen;
    // DIALOGS


    private readonly SelectOptionControl _selectWingOptionControl;

    private readonly SelectOptionControl _selectArmorOptionControl;
    private readonly SelectOptionControl _selectLeftHandOptionControl;
    private readonly SelectOptionControl _selectRightHandOptionControl;
    private readonly SelectOptionControl _selectCharacterClassOptionControl;
    private readonly SelectOptionControl _selectPetOptionControl;
    private readonly SelectOptionControl _selectVehicleOptionControl;
    private readonly OptionPickerControl _selectAnimationOptionControl;
    private readonly LabelButton _testAnimationButton;
    private readonly LabelButton _appearanceConfigButton;


    private bool _initialLoadComplete = false;

    private IEnumerable<KeyValuePair<string, int>> CharacterClasses;
    private IEnumerable<ItemDefinition> Wings;
    private IEnumerable<ItemDefinition> Armors;
    private IEnumerable<ItemDefinition> Pets;
    private IEnumerable<ItemDefinition> Weapons;
    private IEnumerable<VehicleDefinition> Vehicles;
    private List<KeyValuePair<string, int>> Actions; 

    private (string Name, PlayerClass Class, ushort Level, AppearanceConfig Appearance)? character
    {
        get
        {
            if (_selectCharacterClassOptionControl.Value == null)
            {
                return null;
            }
            int armorSetIndex = _selectArmorOptionControl.Value.HasValue ? Armors.ElementAt(_selectArmorOptionControl.Value.Value.Value).Id : 0xFFFF;
            ItemDefinition leftHand = _selectLeftHandOptionControl.Value.HasValue ? Weapons.ElementAt(_selectLeftHandOptionControl.Value.Value.Value) : null;
            int leftHandItemIndex = leftHand?.Id ?? 0xff;
            int leftHandItemGroupIndex = leftHand?.Group ?? 0xff;
            ItemDefinition rightHand = _selectRightHandOptionControl.Value.HasValue ? Weapons.ElementAt(_selectRightHandOptionControl.Value.Value.Value) : null;
            int rightHandItemIndex = rightHand?.Id ?? 0xff;
            int rightHandItemGroupIndex = rightHand?.Group ?? 0xff;
            ItemDefinition wing = _selectWingOptionControl.Value.HasValue ? Wings.ElementAt(_selectWingOptionControl.Value.Value.Value) : null;
            short wingIndex = -1;
            if (wing != null)
            {
                wingIndex = (short)wing.Id;
            }
            short vehicleIndex = -1;
            VehicleDefinition vehicle = _selectVehicleOptionControl.Value.HasValue ? Vehicles.ElementAt(_selectVehicleOptionControl.Value.Value.Value) : null;
            if (vehicle != null)
            {
                vehicleIndex = (short)vehicle.Id;
            }
            return (
                _selectCharacterClassOptionControl.Value.Value.Key ?? "",
                (PlayerClass)_selectCharacterClassOptionControl.Value.Value.Value,
                1,
                new AppearanceConfig()
                {
                    PlayerClass = (PlayerClass)_selectCharacterClassOptionControl.Value.Value.Value,
                    HelmItemIndex = armorSetIndex,
                    HelmItemLevel = 1,
                    ArmorItemIndex = armorSetIndex,
                    ArmorItemLevel = 1,
                    PantsItemIndex = armorSetIndex,
                    PantsItemLevel = 1,
                    GlovesItemIndex = armorSetIndex,
                    GlovesItemLevel = 1,
                    BootsItemIndex = armorSetIndex,
                    BootsItemLevel = 1,
                    LeftHandItemIndex = (byte)leftHandItemIndex,
                    LeftHandItemGroup = (byte)leftHandItemGroupIndex,
                    RightHandItemIndex = (byte)rightHandItemIndex,
                    RightHandItemGroup = (byte)rightHandItemGroupIndex,
                    WingInfo = new WingAppearance(0, 0, wingIndex),
                    RidingVehicle = vehicleIndex,
                }
            );
        }
    }

    // Constructors
    public TestAnimationScene()
    {

        _logger = MuGame.AppLoggerFactory.CreateLogger<TestAnimationScene>();

        CharacterClasses = new List<KeyValuePair<string, int>>([
            new(PlayerClass.DarkWizard.ToString(), (int)PlayerClass.DarkWizard),
            new(PlayerClass.SoulMaster.ToString(), (int)PlayerClass.SoulMaster),
            new(PlayerClass.GrandMaster.ToString(), (int)PlayerClass.GrandMaster),
            new(PlayerClass.SoulWizard.ToString(), (int)PlayerClass.SoulWizard),
            new(PlayerClass.DarkKnight.ToString(), (int)PlayerClass.DarkKnight),
            new(PlayerClass.BladeKnight.ToString(), (int)PlayerClass.BladeKnight),
            new(PlayerClass.BladeMaster.ToString(), (int)PlayerClass.BladeMaster),
            new(PlayerClass.DragonKnight.ToString(), (int)PlayerClass.DragonKnight),
            new(PlayerClass.FairyElf.ToString(), (int)PlayerClass.FairyElf),
            new(PlayerClass.MuseElf.ToString(), (int)PlayerClass.MuseElf),
            new(PlayerClass.HighElf.ToString(), (int)PlayerClass.HighElf),
            new(PlayerClass.NobleElf.ToString(), (int)PlayerClass.NobleElf),
            new(PlayerClass.MagicGladiator.ToString(), (int)PlayerClass.MagicGladiator),
            new(PlayerClass.DuelMaster.ToString(), (int)PlayerClass.DuelMaster),
            new(PlayerClass.MagicKnight.ToString(), (int)PlayerClass.MagicKnight),
            new(PlayerClass.DarkLord.ToString(), (int)PlayerClass.DarkLord),
            new(PlayerClass.LordEmperor.ToString(), (int)PlayerClass.LordEmperor),
            new(PlayerClass.EmpireLord.ToString(), (int)PlayerClass.EmpireLord),
            new(PlayerClass.Summoner.ToString(), (int)PlayerClass.Summoner),
            new(PlayerClass.BloodySummoner.ToString(), (int)PlayerClass.BloodySummoner),
            new(PlayerClass.DimensionMaster.ToString(), (int)PlayerClass.DimensionMaster),
            new(PlayerClass.DimensionSummoner.ToString(), (int)PlayerClass.DimensionSummoner),
            new(PlayerClass.RageFighter.ToString(), (int)PlayerClass.RageFighter),
            new(PlayerClass.FistMaster.ToString(), (int)PlayerClass.FistMaster),
            new(PlayerClass.FistBlazer.ToString(), (int)PlayerClass.FistBlazer),
            new(PlayerClass.GlowLancer.ToString(), (int)PlayerClass.GlowLancer),
            new(PlayerClass.MirageLancer.ToString(), (int)PlayerClass.MirageLancer),
            new(PlayerClass.ShiningLancer.ToString(), (int)PlayerClass.ShiningLancer),
            new(PlayerClass.RuneMage.ToString(), (int)PlayerClass.RuneMage),
            new(PlayerClass.RuneSpellMaster.ToString(), (int)PlayerClass.RuneSpellMaster),
            new(PlayerClass.GradRuneMaster.ToString(), (int)PlayerClass.GradRuneMaster),
            new(PlayerClass.MajesticRuneWizard.ToString(), (int)PlayerClass.MajesticRuneWizard),
            new(PlayerClass.Slayer.ToString(), (int)PlayerClass.Slayer),
            new(PlayerClass.RoyalSlayer.ToString(), (int)PlayerClass.RoyalSlayer),
            new(PlayerClass.MasterSlayer.ToString(), (int)PlayerClass.MasterSlayer),
            new(PlayerClass.Slaughterer.ToString(), (int)PlayerClass.Slaughterer),
            new(PlayerClass.GunCrusher.ToString(), (int)PlayerClass.GunCrusher),
            new(PlayerClass.GunBreaker.ToString(), (int)PlayerClass.GunBreaker),
            new(PlayerClass.MasterGunBreaker.ToString(), (int)PlayerClass.MasterGunBreaker),
            new(PlayerClass.HeistGunCrasher.ToString(), (int)PlayerClass.HeistGunCrasher),
            new(PlayerClass.WhiteWizard.ToString(), (int)PlayerClass.WhiteWizard),
            new(PlayerClass.LightMaster.ToString(), (int)PlayerClass.LightMaster),
            new(PlayerClass.ShineWizard.ToString(), (int)PlayerClass.ShineWizard),
            new(PlayerClass.ShineMaster.ToString(), (int)PlayerClass.ShineMaster),
            new(PlayerClass.Mage.ToString(), (int)PlayerClass.Mage),
            new(PlayerClass.WoMage.ToString(), (int)PlayerClass.WoMage),
            new(PlayerClass.ArchMage.ToString(), (int)PlayerClass.ArchMage),
            new(PlayerClass.MysticMage.ToString(), (int)PlayerClass.MysticMage),
            new(PlayerClass.IllusionKnight.ToString(), (int)PlayerClass.IllusionKnight),
            new(PlayerClass.MirageKnight.ToString(), (int)PlayerClass.MirageKnight),
            new(PlayerClass.IllusionMaster.ToString(), (int)PlayerClass.IllusionMaster),
            new(PlayerClass.MysticKnight.ToString(), (int)PlayerClass.MysticKnight),
            new(PlayerClass.Alchemist.ToString(), (int)PlayerClass.Alchemist),
            new(PlayerClass.AlchemicMaster.ToString(), (int)PlayerClass.AlchemicMaster),
            new(PlayerClass.AlchemicForce.ToString(), (int)PlayerClass.AlchemicForce),
            new(PlayerClass.Creator.ToString(), (int)PlayerClass.Creator),
        ]);
        Armors = ItemDatabase.GetArmors();
        Weapons = ItemDatabase.GetWeapons();
        Wings = ItemDatabase.GetWings();
        Vehicles = VehicleDatabase.Riders.Values.ToList();

        _loadingScreen = new LoadingScreenControl { Visible = true, Message = "Loading Scene" };
        Controls.Add(_loadingScreen);

        Controls.Add(_selectCharacterClassOptionControl = new SelectOptionControl()
        {
            Text = "Select Class",
            X = 10 + 5,
            Y = 10,
        });
        _selectCharacterClassOptionControl.ValueChanged += HandleChangeCharacterClass;
        _selectCharacterClassOptionControl.OptionPickerVisibleChanged += HandleChangeCharacterClassOptionPickerVisible;

        Controls.Add(_selectArmorOptionControl = new SelectOptionControl()
        {
            Text = "Select Armor",
            X = 180 + 30 + 10 + 5,
            Y = 10,
        });
        _selectArmorOptionControl.ValueChanged += HandleChangeArmorSet;

        Controls.Add(_selectLeftHandOptionControl = new SelectOptionControl()
        {
            Text = "Select Weapon Left",
            X = (180 + 30) * 2 + 10 + 5,
            Y = 10,
        });
        _selectLeftHandOptionControl.ValueChanged += HandleChangeLeftHand;

        Controls.Add(_selectRightHandOptionControl = new SelectOptionControl()
        {
            Text = "Select Weapon Right",
            X = (180 + 30) * 3 + 10 + 5,
            Y = 10,
        });
        _selectRightHandOptionControl.ValueChanged += HandleChangeRightHand;

        Controls.Add(_selectWingOptionControl = new SelectOptionControl()
        {
            Text = "Select Wing",
            X = (180 + 30) * 4 + 10 + 5,
            Y = 10,
        });
        _selectWingOptionControl.ValueChanged += HandleChangeWing;

        Controls.Add(_selectPetOptionControl = new SelectOptionControl()
        {
            Text = "Select Pet",
            X = (180 + 30) * 5 + 10 + 5,
            Y = 10,
        });
        _selectPetOptionControl.ValueChanged += HandleChangePet;

        Controls.Add(_selectVehicleOptionControl = new SelectOptionControl()
        {
            Text = "Select Vehicle",
            ButtonAlign = ControlAlign.Top,
            Y = 10 + 29 + 10,
            X = 15,
        });
        _selectVehicleOptionControl.ValueChanged += HandleChangeRide;


        Controls.Add(_appearanceConfigButton = new LabelButton
        {
            Label = new LabelControl
            {
                Text = "Appearance Config",
                X = 8,
                Align = ControlAlign.VerticalCenter,
            },
            Align = ControlAlign.Bottom,
            Margin = new Margin { Bottom = 10 },
            X = 25,
        });
        _appearanceConfigButton.Click += HandleGoToAppearanceConfigButtonClick;

        Controls.Add(_testAnimationButton = new LabelButton
        {
            Label = new LabelControl
            {
                Text = "Test Actions",
                X = 8,
                Align = ControlAlign.VerticalCenter,
            },
            Align = ControlAlign.Bottom,
            Margin = new Margin { Bottom = 10 },
            X = 180 + 25 + 30,
        });
        _testAnimationButton.Click += HandleGoToTestAnimationButtonClick;
        Controls.Add(_selectAnimationOptionControl = new OptionPickerControl
        {
            Align = ControlAlign.Right,
            Options = new(),
            ItemsVisible = 21,
        });
        _selectAnimationOptionControl.ValueChanged += HandleChangeAnimation;
        Actions = [
            new(PlayerAction.Set.ToString(), (int)PlayerAction.Set),
            new(PlayerAction.PlayerStopMale.ToString(), (int)PlayerAction.PlayerStopMale),
            new(PlayerAction.PlayerStopFemale.ToString(), (int)PlayerAction.PlayerStopFemale),
            new(PlayerAction.PlayerStopSummoner.ToString(), (int)PlayerAction.PlayerStopSummoner),
            new(PlayerAction.PlayerStopSword.ToString(), (int)PlayerAction.PlayerStopSword),
            new(PlayerAction.PlayerStopTwoHandSword.ToString(), (int)PlayerAction.PlayerStopTwoHandSword),
            new(PlayerAction.PlayerStopSpear.ToString(), (int)PlayerAction.PlayerStopSpear),
            new(PlayerAction.PlayerStopScythe.ToString(), (int)PlayerAction.PlayerStopScythe),
            new(PlayerAction.PlayerStopBow.ToString(), (int)PlayerAction.PlayerStopBow),
            new(PlayerAction.PlayerStopCrossbow.ToString(), (int)PlayerAction.PlayerStopCrossbow),
            new(PlayerAction.PlayerStopWand.ToString(), (int)PlayerAction.PlayerStopWand),
            new(PlayerAction.PlayerStopFly.ToString(), (int)PlayerAction.PlayerStopFly),
            new(PlayerAction.PlayerStopFlyCrossbow.ToString(), (int)PlayerAction.PlayerStopFlyCrossbow),
            new(PlayerAction.PlayerStopRide.ToString(), (int)PlayerAction.PlayerStopRide),
            new(PlayerAction.PlayerStopRideWeapon.ToString(), (int)PlayerAction.PlayerStopRideWeapon),
            new(PlayerAction.PlayerWalkMale.ToString(), (int)PlayerAction.PlayerWalkMale),
            new(PlayerAction.PlayerWalkFemale.ToString(), (int)PlayerAction.PlayerWalkFemale),
            new(PlayerAction.PlayerWalkSword.ToString(), (int)PlayerAction.PlayerWalkSword),
            new(PlayerAction.PlayerWalkTwoHandSword.ToString(), (int)PlayerAction.PlayerWalkTwoHandSword),
            new(PlayerAction.PlayerWalkSpear.ToString(), (int)PlayerAction.PlayerWalkSpear),
            new(PlayerAction.PlayerWalkScythe.ToString(), (int)PlayerAction.PlayerWalkScythe),
            new(PlayerAction.PlayerWalkBow.ToString(), (int)PlayerAction.PlayerWalkBow),
            new(PlayerAction.PlayerWalkCrossbow.ToString(), (int)PlayerAction.PlayerWalkCrossbow),
            new(PlayerAction.PlayerWalkWand.ToString(), (int)PlayerAction.PlayerWalkWand),
            new(PlayerAction.PlayerWalkSwim.ToString(), (int)PlayerAction.PlayerWalkSwim),
            new(PlayerAction.PlayerRun.ToString(), (int)PlayerAction.PlayerRun),
            new(PlayerAction.PlayerRunSword.ToString(), (int)PlayerAction.PlayerRunSword),
            new(PlayerAction.PlayerRunTwoSword.ToString(), (int)PlayerAction.PlayerRunTwoSword),
            new(PlayerAction.PlayerRunTwoHandSword.ToString(), (int)PlayerAction.PlayerRunTwoHandSword),
            new(PlayerAction.PlayerRunSpear.ToString(), (int)PlayerAction.PlayerRunSpear),
            new(PlayerAction.PlayerRunBow.ToString(), (int)PlayerAction.PlayerRunBow),
            new(PlayerAction.PlayerRunCrossbow.ToString(), (int)PlayerAction.PlayerRunCrossbow),
            new(PlayerAction.PlayerRunWand.ToString(), (int)PlayerAction.PlayerRunWand),
            new(PlayerAction.PlayerRunSwim.ToString(), (int)PlayerAction.PlayerRunSwim),
            new(PlayerAction.PlayerFly.ToString(), (int)PlayerAction.PlayerFly),
            new(PlayerAction.PlayerFlyCrossbow.ToString(), (int)PlayerAction.PlayerFlyCrossbow),
            new(PlayerAction.PlayerRunRide.ToString(), (int)PlayerAction.PlayerRunRide),
            new(PlayerAction.PlayerRunRideWeapon.ToString(), (int)PlayerAction.PlayerRunRideWeapon),
            new(PlayerAction.PlayerAttackFist.ToString(), (int)PlayerAction.PlayerAttackFist),
            new(PlayerAction.PlayerAttackSwordRight1.ToString(), (int)PlayerAction.PlayerAttackSwordRight1),
            new(PlayerAction.PlayerAttackSwordRight2.ToString(), (int)PlayerAction.PlayerAttackSwordRight2),
            new(PlayerAction.PlayerAttackSwordLeft1.ToString(), (int)PlayerAction.PlayerAttackSwordLeft1),
            new(PlayerAction.PlayerAttackSwordLeft2.ToString(), (int)PlayerAction.PlayerAttackSwordLeft2),
            new(PlayerAction.PlayerAttackTwoHandSword1.ToString(), (int)PlayerAction.PlayerAttackTwoHandSword1),
            new(PlayerAction.PlayerAttackTwoHandSword2.ToString(), (int)PlayerAction.PlayerAttackTwoHandSword2),
            new(PlayerAction.PlayerAttackTwoHandSword3.ToString(), (int)PlayerAction.PlayerAttackTwoHandSword3),
            new(PlayerAction.PlayerAttackSpear1.ToString(), (int)PlayerAction.PlayerAttackSpear1),
            new(PlayerAction.PlayerAttackScythe1.ToString(), (int)PlayerAction.PlayerAttackScythe1),
            new(PlayerAction.PlayerAttackScythe2.ToString(), (int)PlayerAction.PlayerAttackScythe2),
            new(PlayerAction.PlayerAttackScythe3.ToString(), (int)PlayerAction.PlayerAttackScythe3),
            new(PlayerAction.PlayerAttackBow.ToString(), (int)PlayerAction.PlayerAttackBow),
            new(PlayerAction.PlayerAttackCrossbow.ToString(), (int)PlayerAction.PlayerAttackCrossbow),
            new(PlayerAction.PlayerAttackFlyBow.ToString(), (int)PlayerAction.PlayerAttackFlyBow),
            new(PlayerAction.PlayerAttackFlyCrossbow.ToString(), (int)PlayerAction.PlayerAttackFlyCrossbow),
            new(PlayerAction.PlayerAttackRideSword.ToString(), (int)PlayerAction.PlayerAttackRideSword),
            new(PlayerAction.PlayerAttackRideTwoHandSword.ToString(), (int)PlayerAction.PlayerAttackRideTwoHandSword),
            new(PlayerAction.PlayerAttackRideSpear.ToString(), (int)PlayerAction.PlayerAttackRideSpear),
            new(PlayerAction.PlayerAttackRideScythe.ToString(), (int)PlayerAction.PlayerAttackRideScythe),
            new(PlayerAction.PlayerAttackRideBow.ToString(), (int)PlayerAction.PlayerAttackRideBow),
            new(PlayerAction.PlayerAttackRideCrossbow.ToString(), (int)PlayerAction.PlayerAttackRideCrossbow),
            new(PlayerAction.PlayerAttackSkillSword1.ToString(), (int)PlayerAction.PlayerAttackSkillSword1),
            new(PlayerAction.PlayerAttackSkillSword2.ToString(), (int)PlayerAction.PlayerAttackSkillSword2),
            new(PlayerAction.PlayerAttackSkillSword3.ToString(), (int)PlayerAction.PlayerAttackSkillSword3),
            new(PlayerAction.PlayerAttackSkillSword4.ToString(), (int)PlayerAction.PlayerAttackSkillSword4),
            new(PlayerAction.PlayerAttackSkillSword5.ToString(), (int)PlayerAction.PlayerAttackSkillSword5),
            new(PlayerAction.PlayerAttackSkillWheel.ToString(), (int)PlayerAction.PlayerAttackSkillWheel),
            new(PlayerAction.PlayerAttackSkillFuryStrike.ToString(), (int)PlayerAction.PlayerAttackSkillFuryStrike),
            new(PlayerAction.PlayerSkillVitality.ToString(), (int)PlayerAction.PlayerSkillVitality),
            new(PlayerAction.PlayerSkillRider.ToString(), (int)PlayerAction.PlayerSkillRider),
            new(PlayerAction.PlayerSkillRiderFly.ToString(), (int)PlayerAction.PlayerSkillRiderFly),
            new(PlayerAction.PlayerAttackSkillSpear.ToString(), (int)PlayerAction.PlayerAttackSkillSpear),
            new(PlayerAction.PlayerAttackDeathstab.ToString(), (int)PlayerAction.PlayerAttackDeathstab),
            new(PlayerAction.PlayerSkillHellBegin.ToString(), (int)PlayerAction.PlayerSkillHellBegin),
            new(PlayerAction.PlayerSkillHellStart.ToString(), (int)PlayerAction.PlayerSkillHellStart),
            new(PlayerAction.PlayerAttackEnd.ToString(), (int)PlayerAction.PlayerAttackEnd),
            new(PlayerAction.PlayerFlyRide.ToString(), (int)PlayerAction.PlayerFlyRide),
            new(PlayerAction.PlayerFlyRideWeapon.ToString(), (int)PlayerAction.PlayerFlyRideWeapon),
            new(PlayerAction.PlayerDarklordStand.ToString(), (int)PlayerAction.PlayerDarklordStand),
            new(PlayerAction.PlayerDarklordWalk.ToString(), (int)PlayerAction.PlayerDarklordWalk),
            new(PlayerAction.PlayerStopRideHorse.ToString(), (int)PlayerAction.PlayerStopRideHorse),
            new(PlayerAction.PlayerRunRideHorse.ToString(), (int)PlayerAction.PlayerRunRideHorse),
            new(PlayerAction.PlayerAttackStrike.ToString(), (int)PlayerAction.PlayerAttackStrike),
            new(PlayerAction.PlayerAttackTeleport.ToString(), (int)PlayerAction.PlayerAttackTeleport),
            new(PlayerAction.PlayerAttackRideStrike.ToString(), (int)PlayerAction.PlayerAttackRideStrike),
            new(PlayerAction.PlayerAttackRideTeleport.ToString(), (int)PlayerAction.PlayerAttackRideTeleport),
            new(PlayerAction.PlayerAttackRideHorseSword.ToString(), (int)PlayerAction.PlayerAttackRideHorseSword),
            new(PlayerAction.PlayerAttackRideAttackFlash.ToString(), (int)PlayerAction.PlayerAttackRideAttackFlash),
            new(PlayerAction.PlayerAttackRideAttackMagic.ToString(), (int)PlayerAction.PlayerAttackRideAttackMagic),
            new(PlayerAction.PlayerAttackDarkhorse.ToString(), (int)PlayerAction.PlayerAttackDarkhorse),
            new(PlayerAction.PlayerIdle1Darkhorse.ToString(), (int)PlayerAction.PlayerIdle1Darkhorse),
            new(PlayerAction.PlayerIdle2Darkhorse.ToString(), (int)PlayerAction.PlayerIdle2Darkhorse),
            new(PlayerAction.PlayerFenrirAttack.ToString(), (int)PlayerAction.PlayerFenrirAttack),
            new(PlayerAction.PlayerFenrirAttackDarklordAqua.ToString(), (int)PlayerAction.PlayerFenrirAttackDarklordAqua),
            new(PlayerAction.PlayerFenrirAttackDarklordStrike.ToString(), (int)PlayerAction.PlayerFenrirAttackDarklordStrike),
            new(PlayerAction.PlayerFenrirAttackDarklordSword.ToString(), (int)PlayerAction.PlayerFenrirAttackDarklordSword),
            new(PlayerAction.PlayerFenrirAttackDarklordTeleport.ToString(), (int)PlayerAction.PlayerFenrirAttackDarklordTeleport),
            new(PlayerAction.PlayerFenrirAttackDarklordFlash.ToString(), (int)PlayerAction.PlayerFenrirAttackDarklordFlash),
            new(PlayerAction.PlayerFenrirAttackTwoSword.ToString(), (int)PlayerAction.PlayerFenrirAttackTwoSword),
            new(PlayerAction.PlayerFenrirAttackMagic.ToString(), (int)PlayerAction.PlayerFenrirAttackMagic),
            new(PlayerAction.PlayerFenrirAttackCrossbow.ToString(), (int)PlayerAction.PlayerFenrirAttackCrossbow),
            new(PlayerAction.PlayerFenrirAttackSpear.ToString(), (int)PlayerAction.PlayerFenrirAttackSpear),
            new(PlayerAction.PlayerFenrirAttackOneSword.ToString(), (int)PlayerAction.PlayerFenrirAttackOneSword),
            new(PlayerAction.PlayerFenrirAttackBow.ToString(), (int)PlayerAction.PlayerFenrirAttackBow),
            new(PlayerAction.PlayerFenrirSkill.ToString(), (int)PlayerAction.PlayerFenrirSkill),
            new(PlayerAction.PlayerFenrirSkillTwoSword.ToString(), (int)PlayerAction.PlayerFenrirSkillTwoSword),
            new(PlayerAction.PlayerFenrirSkillOneRight.ToString(), (int)PlayerAction.PlayerFenrirSkillOneRight),
            new(PlayerAction.PlayerFenrirSkillOneLeft.ToString(), (int)PlayerAction.PlayerFenrirSkillOneLeft),
            new(PlayerAction.PlayerFenrirDamage.ToString(), (int)PlayerAction.PlayerFenrirDamage),
            new(PlayerAction.PlayerFenrirDamageTwoSword.ToString(), (int)PlayerAction.PlayerFenrirDamageTwoSword),
            new(PlayerAction.PlayerFenrirDamageOneRight.ToString(), (int)PlayerAction.PlayerFenrirDamageOneRight),
            new(PlayerAction.PlayerFenrirDamageOneLeft.ToString(), (int)PlayerAction.PlayerFenrirDamageOneLeft),
            new(PlayerAction.PlayerFenrirRun.ToString(), (int)PlayerAction.PlayerFenrirRun),
            new(PlayerAction.PlayerFenrirRunTwoSword.ToString(), (int)PlayerAction.PlayerFenrirRunTwoSword),
            new(PlayerAction.PlayerFenrirRunOneRight.ToString(), (int)PlayerAction.PlayerFenrirRunOneRight),
            new(PlayerAction.PlayerFenrirRunOneLeft.ToString(), (int)PlayerAction.PlayerFenrirRunOneLeft),
            new(PlayerAction.PlayerFenrirRunMagom.ToString(), (int)PlayerAction.PlayerFenrirRunMagom),
            new(PlayerAction.PlayerFenrirRunTwoSwordMagom.ToString(), (int)PlayerAction.PlayerFenrirRunTwoSwordMagom),
            new(PlayerAction.PlayerFenrirRunOneRightMagom.ToString(), (int)PlayerAction.PlayerFenrirRunOneRightMagom),
            new(PlayerAction.PlayerFenrirRunOneLeftMagom.ToString(), (int)PlayerAction.PlayerFenrirRunOneLeftMagom),
            new(PlayerAction.PlayerFenrirRunElf.ToString(), (int)PlayerAction.PlayerFenrirRunElf),
            new(PlayerAction.PlayerFenrirRunTwoSwordElf.ToString(), (int)PlayerAction.PlayerFenrirRunTwoSwordElf),
            new(PlayerAction.PlayerFenrirRunOneRightElf.ToString(), (int)PlayerAction.PlayerFenrirRunOneRightElf),
            new(PlayerAction.PlayerFenrirRunOneLeftElf.ToString(), (int)PlayerAction.PlayerFenrirRunOneLeftElf),
            new(PlayerAction.PlayerFenrirStand.ToString(), (int)PlayerAction.PlayerFenrirStand),
            new(PlayerAction.PlayerFenrirStandTwoSword.ToString(), (int)PlayerAction.PlayerFenrirStandTwoSword),
            new(PlayerAction.PlayerFenrirStandOneRight.ToString(), (int)PlayerAction.PlayerFenrirStandOneRight),
            new(PlayerAction.PlayerFenrirStandOneLeft.ToString(), (int)PlayerAction.PlayerFenrirStandOneLeft),
            new(PlayerAction.PlayerFenrirWalk.ToString(), (int)PlayerAction.PlayerFenrirWalk),
            new(PlayerAction.PlayerFenrirWalkTwoSword.ToString(), (int)PlayerAction.PlayerFenrirWalkTwoSword),
            new(PlayerAction.PlayerFenrirWalkOneRight.ToString(), (int)PlayerAction.PlayerFenrirWalkOneRight),
            new(PlayerAction.PlayerFenrirWalkOneLeft.ToString(), (int)PlayerAction.PlayerFenrirWalkOneLeft),
            new(PlayerAction.PlayerAttackBowUp.ToString(), (int)PlayerAction.PlayerAttackBowUp),
            new(PlayerAction.PlayerAttackCrossbowUp.ToString(), (int)PlayerAction.PlayerAttackCrossbowUp),
            new(PlayerAction.PlayerAttackFlyBowUp.ToString(), (int)PlayerAction.PlayerAttackFlyBowUp),
            new(PlayerAction.PlayerAttackFlyCrossbowUp.ToString(), (int)PlayerAction.PlayerAttackFlyCrossbowUp),
            new(PlayerAction.PlayerAttackRideBowUp.ToString(), (int)PlayerAction.PlayerAttackRideBowUp),
            new(PlayerAction.PlayerAttackRideCrossbowUp.ToString(), (int)PlayerAction.PlayerAttackRideCrossbowUp),
            new(PlayerAction.PlayerAttackOneFlash.ToString(), (int)PlayerAction.PlayerAttackOneFlash),
            new(PlayerAction.PlayerAttackRush.ToString(), (int)PlayerAction.PlayerAttackRush),
            new(PlayerAction.PlayerAttackDeathCannon.ToString(), (int)PlayerAction.PlayerAttackDeathCannon),
            new(PlayerAction.PlayerAttackRemoval.ToString(), (int)PlayerAction.PlayerAttackRemoval),
            new(PlayerAction.PlayerAttackStun.ToString(), (int)PlayerAction.PlayerAttackStun),
            new(PlayerAction.PlayerShock.ToString(), (int)PlayerAction.PlayerShock),
            new(PlayerAction.PlayerStopTwoHandSwordTwo.ToString(), (int)PlayerAction.PlayerStopTwoHandSwordTwo),
            new(PlayerAction.PlayerWalkTwoHandSwordTwo.ToString(), (int)PlayerAction.PlayerWalkTwoHandSwordTwo),
            new(PlayerAction.PlayerRunTwoHandSwordTwo.ToString(), (int)PlayerAction.PlayerRunTwoHandSwordTwo),
            new(PlayerAction.PlayerAttackTwoHandSwordTwo.ToString(), (int)PlayerAction.PlayerAttackTwoHandSwordTwo),
            new(PlayerAction.PlayerSkillHand1.ToString(), (int)PlayerAction.PlayerSkillHand1),
            new(PlayerAction.PlayerSkillHand2.ToString(), (int)PlayerAction.PlayerSkillHand2),
            new(PlayerAction.PlayerSkillWeapon1.ToString(), (int)PlayerAction.PlayerSkillWeapon1),
            new(PlayerAction.PlayerSkillWeapon2.ToString(), (int)PlayerAction.PlayerSkillWeapon2),
            new(PlayerAction.PlayerSkillElf1.ToString(), (int)PlayerAction.PlayerSkillElf1),
            new(PlayerAction.PlayerSkillTeleport.ToString(), (int)PlayerAction.PlayerSkillTeleport),
            new(PlayerAction.PlayerSkillFlash.ToString(), (int)PlayerAction.PlayerSkillFlash),
            new(PlayerAction.PlayerSkillInferno.ToString(), (int)PlayerAction.PlayerSkillInferno),
            new(PlayerAction.PlayerSkillHell.ToString(), (int)PlayerAction.PlayerSkillHell),
            new(PlayerAction.PlayerRideSkill.ToString(), (int)PlayerAction.PlayerRideSkill),
            new(PlayerAction.PlayerSkillSleep.ToString(), (int)PlayerAction.PlayerSkillSleep),
            new(PlayerAction.PlayerSkillSleepUni.ToString(), (int)PlayerAction.PlayerSkillSleepUni),
            new(PlayerAction.PlayerSkillSleepDino.ToString(), (int)PlayerAction.PlayerSkillSleepDino),
            new(PlayerAction.PlayerSkillSleepFenrir.ToString(), (int)PlayerAction.PlayerSkillSleepFenrir),
            new(PlayerAction.PlayerSkillChainLightning.ToString(), (int)PlayerAction.PlayerSkillChainLightning),
            new(PlayerAction.PlayerSkillChainLightningUni.ToString(), (int)PlayerAction.PlayerSkillChainLightningUni),
            new(PlayerAction.PlayerSkillChainLightningDino.ToString(), (int)PlayerAction.PlayerSkillChainLightningDino),
            new(PlayerAction.PlayerSkillChainLightningFenrir.ToString(), (int)PlayerAction.PlayerSkillChainLightningFenrir),
            new(PlayerAction.PlayerSkillLightningOrb.ToString(), (int)PlayerAction.PlayerSkillLightningOrb),
            new(PlayerAction.PlayerSkillLightningOrbUni.ToString(), (int)PlayerAction.PlayerSkillLightningOrbUni),
            new(PlayerAction.PlayerSkillLightningOrbDino.ToString(), (int)PlayerAction.PlayerSkillLightningOrbDino),
            new(PlayerAction.PlayerSkillLightningOrbFenrir.ToString(), (int)PlayerAction.PlayerSkillLightningOrbFenrir),
            new(PlayerAction.PlayerSkillDrainLife.ToString(), (int)PlayerAction.PlayerSkillDrainLife),
            new(PlayerAction.PlayerSkillDrainLifeUni.ToString(), (int)PlayerAction.PlayerSkillDrainLifeUni),
            new(PlayerAction.PlayerSkillDrainLifeDino.ToString(), (int)PlayerAction.PlayerSkillDrainLifeDino),
            new(PlayerAction.PlayerSkillDrainLifeFenrir.ToString(), (int)PlayerAction.PlayerSkillDrainLifeFenrir),
            new(PlayerAction.PlayerSkillSummon.ToString(), (int)PlayerAction.PlayerSkillSummon),
            new(PlayerAction.PlayerSkillSummonUni.ToString(), (int)PlayerAction.PlayerSkillSummonUni),
            new(PlayerAction.PlayerSkillSummonDino.ToString(), (int)PlayerAction.PlayerSkillSummonDino),
            new(PlayerAction.PlayerSkillSummonFenrir.ToString(), (int)PlayerAction.PlayerSkillSummonFenrir),
            new(PlayerAction.PlayerSkillBlowOfDestruction.ToString(), (int)PlayerAction.PlayerSkillBlowOfDestruction),
            new(PlayerAction.PlayerSkillSwellOfMp.ToString(), (int)PlayerAction.PlayerSkillSwellOfMp),
            new(PlayerAction.PlayerSkillMultishotBowStand.ToString(), (int)PlayerAction.PlayerSkillMultishotBowStand),
            new(PlayerAction.PlayerSkillMultishotBowFlying.ToString(), (int)PlayerAction.PlayerSkillMultishotBowFlying),
            new(PlayerAction.PlayerSkillMultishotCrossbowStand.ToString(), (int)PlayerAction.PlayerSkillMultishotCrossbowStand),
            new(PlayerAction.PlayerSkillMultishotCrossbowFlying.ToString(), (int)PlayerAction.PlayerSkillMultishotCrossbowFlying),
            new(PlayerAction.PlayerSkillRecovery.ToString(), (int)PlayerAction.PlayerSkillRecovery),
            new(PlayerAction.PlayerSkillGiganticstorm.ToString(), (int)PlayerAction.PlayerSkillGiganticstorm),
            new(PlayerAction.PlayerSkillFlamestrike.ToString(), (int)PlayerAction.PlayerSkillFlamestrike),
            new(PlayerAction.PlayerSkillLightningShock.ToString(), (int)PlayerAction.PlayerSkillLightningShock),
            new(PlayerAction.PlayerDefense1.ToString(), (int)PlayerAction.PlayerDefense1),
            new(PlayerAction.PlayerGreeting1.ToString(), (int)PlayerAction.PlayerGreeting1),
            new(PlayerAction.PlayerGreetingFemale1.ToString(), (int)PlayerAction.PlayerGreetingFemale1),
            new(PlayerAction.PlayerGoodbye1.ToString(), (int)PlayerAction.PlayerGoodbye1),
            new(PlayerAction.PlayerGoodbyeFemale1.ToString(), (int)PlayerAction.PlayerGoodbyeFemale1),
            new(PlayerAction.PlayerClap1.ToString(), (int)PlayerAction.PlayerClap1),
            new(PlayerAction.PlayerClapFemale1.ToString(), (int)PlayerAction.PlayerClapFemale1),
            new(PlayerAction.PlayerCheer1.ToString(), (int)PlayerAction.PlayerCheer1),
            new(PlayerAction.PlayerCheerFemale1.ToString(), (int)PlayerAction.PlayerCheerFemale1),
            new(PlayerAction.PlayerDirection1.ToString(), (int)PlayerAction.PlayerDirection1),
            new(PlayerAction.PlayerDirectionFemale1.ToString(), (int)PlayerAction.PlayerDirectionFemale1),
            new(PlayerAction.PlayerGesture1.ToString(), (int)PlayerAction.PlayerGesture1),
            new(PlayerAction.PlayerGestureFemale1.ToString(), (int)PlayerAction.PlayerGestureFemale1),
            new(PlayerAction.PlayerUnknown1.ToString(), (int)PlayerAction.PlayerUnknown1),
            new(PlayerAction.PlayerUnknownFemale1.ToString(), (int)PlayerAction.PlayerUnknownFemale1),
            new(PlayerAction.PlayerCry1.ToString(), (int)PlayerAction.PlayerCry1),
            new(PlayerAction.PlayerCryFemale1.ToString(), (int)PlayerAction.PlayerCryFemale1),
            new(PlayerAction.PlayerAwkward1.ToString(), (int)PlayerAction.PlayerAwkward1),
            new(PlayerAction.PlayerAwkwardFemale1.ToString(), (int)PlayerAction.PlayerAwkwardFemale1),
            new(PlayerAction.PlayerSee1.ToString(), (int)PlayerAction.PlayerSee1),
            new(PlayerAction.PlayerSeeFemale1.ToString(), (int)PlayerAction.PlayerSeeFemale1),
            new(PlayerAction.PlayerWin1.ToString(), (int)PlayerAction.PlayerWin1),
            new(PlayerAction.PlayerWinFemale1.ToString(), (int)PlayerAction.PlayerWinFemale1),
            new(PlayerAction.PlayerSmile1.ToString(), (int)PlayerAction.PlayerSmile1),
            new(PlayerAction.PlayerSmileFemale1.ToString(), (int)PlayerAction.PlayerSmileFemale1),
            new(PlayerAction.PlayerSleep1.ToString(), (int)PlayerAction.PlayerSleep1),
            new(PlayerAction.PlayerSleepFemale1.ToString(), (int)PlayerAction.PlayerSleepFemale1),
            new(PlayerAction.PlayerCold1.ToString(), (int)PlayerAction.PlayerCold1),
            new(PlayerAction.PlayerColdFemale1.ToString(), (int)PlayerAction.PlayerColdFemale1),
            new(PlayerAction.PlayerAgain1.ToString(), (int)PlayerAction.PlayerAgain1),
            new(PlayerAction.PlayerAgainFemale1.ToString(), (int)PlayerAction.PlayerAgainFemale1),
            new(PlayerAction.PlayerRespect1.ToString(), (int)PlayerAction.PlayerRespect1),
            new(PlayerAction.PlayerSalute1.ToString(), (int)PlayerAction.PlayerSalute1),
            new(PlayerAction.PlayerScissors.ToString(), (int)PlayerAction.PlayerScissors),
            new(PlayerAction.PlayerRock.ToString(), (int)PlayerAction.PlayerRock),
            new(PlayerAction.PlayerPaper.ToString(), (int)PlayerAction.PlayerPaper),
            new(PlayerAction.PlayerHustle.ToString(), (int)PlayerAction.PlayerHustle),
            new(PlayerAction.PlayerProvocation.ToString(), (int)PlayerAction.PlayerProvocation),
            new(PlayerAction.PlayerLookAround.ToString(), (int)PlayerAction.PlayerLookAround),
            new(PlayerAction.PlayerCheers.ToString(), (int)PlayerAction.PlayerCheers),
            new(PlayerAction.PlayerKoreaHandclap.ToString(), (int)PlayerAction.PlayerKoreaHandclap),
            new(PlayerAction.PlayerPointDance.ToString(), (int)PlayerAction.PlayerPointDance),
            new(PlayerAction.PlayerRush1.ToString(), (int)PlayerAction.PlayerRush1),
            new(PlayerAction.PlayerComeUp.ToString(), (int)PlayerAction.PlayerComeUp),
            new(PlayerAction.ActionUnknown230.ToString(), (int)PlayerAction.ActionUnknown230),
            new(PlayerAction.PlayerDie1.ToString(), (int)PlayerAction.PlayerDie1),
            new(PlayerAction.PlayerDie2.ToString(), (int)PlayerAction.PlayerDie2),
            new(PlayerAction.PlayerSit1.ToString(), (int)PlayerAction.PlayerSit1),
            new(PlayerAction.PlayerSit2.ToString(), (int)PlayerAction.PlayerSit2),
            new(PlayerAction.PlayerSitFemale1.ToString(), (int)PlayerAction.PlayerSitFemale1),
            new(PlayerAction.PlayerSitFemale2.ToString(), (int)PlayerAction.PlayerSitFemale2),
            new(PlayerAction.PlayerHealing1.ToString(), (int)PlayerAction.PlayerHealing1),
            new(PlayerAction.PlayerHealingFemale1.ToString(), (int)PlayerAction.PlayerHealingFemale1),
            new(PlayerAction.PlayerPoseMale1.ToString(), (int)PlayerAction.PlayerPoseMale1),
            new(PlayerAction.PlayerPoseFemale1.ToString(), (int)PlayerAction.PlayerPoseFemale1),
            new(PlayerAction.PlayerJack1.ToString(), (int)PlayerAction.PlayerJack1),
            new(PlayerAction.PlayerJack2.ToString(), (int)PlayerAction.PlayerJack2),
            new(PlayerAction.PlayerSanta1.ToString(), (int)PlayerAction.PlayerSanta1),
            new(PlayerAction.PlayerSanta2.ToString(), (int)PlayerAction.PlayerSanta2),
            new(PlayerAction.PlayerChangeUp.ToString(), (int)PlayerAction.PlayerChangeUp),
            new(PlayerAction.PlayerRecoverSkill.ToString(), (int)PlayerAction.PlayerRecoverSkill),
            new(PlayerAction.PlayerSkillThrust.ToString(), (int)PlayerAction.PlayerSkillThrust),
            new(PlayerAction.PlayerSkillStamp.ToString(), (int)PlayerAction.PlayerSkillStamp),
            new(PlayerAction.PlayerSkillGiantswing.ToString(), (int)PlayerAction.PlayerSkillGiantswing),
            new(PlayerAction.PlayerSkillDarksideReady.ToString(), (int)PlayerAction.PlayerSkillDarksideReady),
            new(PlayerAction.PlayerSkillDarksideAttack.ToString(), (int)PlayerAction.PlayerSkillDarksideAttack),
            new(PlayerAction.PlayerSkillDragonkick.ToString(), (int)PlayerAction.PlayerSkillDragonkick),
            new(PlayerAction.PlayerSkillDragonlore.ToString(), (int)PlayerAction.PlayerSkillDragonlore),
            new(PlayerAction.PlayerSkillPhoenixShot.ToString(), (int)PlayerAction.PlayerSkillPhoenixShot),
            new(PlayerAction.PlayerSkillAttUpOurforces.ToString(), (int)PlayerAction.PlayerSkillAttUpOurforces),
            new(PlayerAction.PlayerSkillHpUpOurforces.ToString(), (int)PlayerAction.PlayerSkillHpUpOurforces),
            new(PlayerAction.PlayerRageUniAttack.ToString(), (int)PlayerAction.PlayerRageUniAttack),
            new(PlayerAction.PlayerRageUniAttackOneRight.ToString(), (int)PlayerAction.PlayerRageUniAttackOneRight),
            new(PlayerAction.PlayerRageUniRun.ToString(), (int)PlayerAction.PlayerRageUniRun),
            new(PlayerAction.PlayerRageUniRunOneRight.ToString(), (int)PlayerAction.PlayerRageUniRunOneRight),
            new(PlayerAction.PlayerRageUniStopOneRight.ToString(), (int)PlayerAction.PlayerRageUniStopOneRight),
            new(PlayerAction.PlayerRageFenrir.ToString(), (int)PlayerAction.PlayerRageFenrir),
            new(PlayerAction.PlayerRageFenrirTwoSword.ToString(), (int)PlayerAction.PlayerRageFenrirTwoSword),
            new(PlayerAction.PlayerRageFenrirOneRight.ToString(), (int)PlayerAction.PlayerRageFenrirOneRight),
            new(PlayerAction.PlayerRageFenrirOneLeft.ToString(), (int)PlayerAction.PlayerRageFenrirOneLeft),
            new(PlayerAction.PlayerRageFenrirWalk.ToString(), (int)PlayerAction.PlayerRageFenrirWalk),
            new(PlayerAction.PlayerRageFenrirWalkOneRight.ToString(), (int)PlayerAction.PlayerRageFenrirWalkOneRight),
            new(PlayerAction.PlayerRageFenrirWalkOneLeft.ToString(), (int)PlayerAction.PlayerRageFenrirWalkOneLeft),
            new(PlayerAction.PlayerRageFenrirWalkTwoSword.ToString(), (int)PlayerAction.PlayerRageFenrirWalkTwoSword),
            new(PlayerAction.PlayerRageFenrirRun.ToString(), (int)PlayerAction.PlayerRageFenrirRun),
            new(PlayerAction.PlayerRageFenrirRunTwoSword.ToString(), (int)PlayerAction.PlayerRageFenrirRunTwoSword),
            new(PlayerAction.PlayerRageFenrirRunOneRight.ToString(), (int)PlayerAction.PlayerRageFenrirRunOneRight),
            new(PlayerAction.PlayerRageFenrirRunOneLeft.ToString(), (int)PlayerAction.PlayerRageFenrirRunOneLeft),
            new(PlayerAction.PlayerRageFenrirStand.ToString(), (int)PlayerAction.PlayerRageFenrirStand),
            new(PlayerAction.PlayerRageFenrirStandTwoSword.ToString(), (int)PlayerAction.PlayerRageFenrirStandTwoSword),
            new(PlayerAction.PlayerRageFenrirStandOneRight.ToString(), (int)PlayerAction.PlayerRageFenrirStandOneRight),
            new(PlayerAction.PlayerRageFenrirStandOneLeft.ToString(), (int)PlayerAction.PlayerRageFenrirStandOneLeft),
            new(PlayerAction.PlayerRageFenrirDamage.ToString(), (int)PlayerAction.PlayerRageFenrirDamage),
            new(PlayerAction.PlayerRageFenrirDamageTwoSword.ToString(), (int)PlayerAction.PlayerRageFenrirDamageTwoSword),
            new(PlayerAction.PlayerRageFenrirDamageOneRight.ToString(), (int)PlayerAction.PlayerRageFenrirDamageOneRight),
            new(PlayerAction.PlayerRageFenrirDamageOneLeft.ToString(), (int)PlayerAction.PlayerRageFenrirDamageOneLeft),
            new(PlayerAction.PlayerRageFenrirAttackRight.ToString(), (int)PlayerAction.PlayerRageFenrirAttackRight),
            new(PlayerAction.PlayerStopRagefighter.ToString(), (int)PlayerAction.PlayerStopRagefighter),
            new(PlayerAction.MaxPlayerAction.ToString(), (int)PlayerAction.MaxPlayerAction),
        ];

        _loadingScreen.BringToFront();
    }


    private void RefreshUI()
    {
        switch (UiState)
        {
            case TestAnimationUiState.Loading:
                {
                    break;
                }
            case TestAnimationUiState.EditCharacter:
                {
                    if (_loadingScreen != null)
                    _loadingScreen.Visible = false;
                    _selectCharacterClassOptionControl.Options = CharacterClasses.ToList();
                    _selectCharacterClassOptionControl.Visible = true;
                    _selectArmorOptionControl.Options = Armors.Select((p, i) => new KeyValuePair<string, int>(p.Name, i)).ToList();
                    _selectArmorOptionControl.Visible = true;
                    _selectLeftHandOptionControl.Options = Weapons.Select((p, i) => new KeyValuePair<string, int>(p.Name, i)).ToList();
                    _selectLeftHandOptionControl.Visible = true;
                    _selectRightHandOptionControl.Options = Weapons.Select((p, i) => new KeyValuePair<string, int>(p.Name, i)).ToList();
                    _selectRightHandOptionControl.Visible = true;
                    _selectWingOptionControl.Options = Wings.Select((p, i) => new KeyValuePair<string, int>(p.Name, i)).ToList();
                    _selectWingOptionControl.Visible = true;
                    _selectPetOptionControl.Visible = true;
                    _selectVehicleOptionControl.Options = Vehicles.Select((p, i) => new KeyValuePair<string, int>(p.Name, i)).ToList();
                    _selectVehicleOptionControl.Visible = true;
                    _selectAnimationOptionControl.Visible = false;
                    break;
                }
            case TestAnimationUiState.TestAction:
                {
                    if (_loadingScreen != null)
                    _loadingScreen.Visible = false;
                    _selectCharacterClassOptionControl.Visible = false;
                    _selectArmorOptionControl.Visible = false;
                    _selectLeftHandOptionControl.Visible = false;
                    _selectRightHandOptionControl.Visible = false;
                    _selectWingOptionControl.Visible = false;
                    _selectPetOptionControl.Visible = false;
                    _selectVehicleOptionControl.Visible = false;
                    _selectAnimationOptionControl.Options = Actions.Select((p, i) => new KeyValuePair<string, int>(p.Key, p.Value)).ToList();
                    _selectAnimationOptionControl.Visible = true;
                    break;
                }
        }
    }

    private void UpdateLoadProgress(string message, float progress)
    {
        MuGame.ScheduleOnMainThread(() =>
        {
            if (_loadingScreen != null && _loadingScreen.Visible)
            {
                _loadingScreen.Message = message;
                _loadingScreen.Progress = progress;
            }
        });
    }




    protected override async Task LoadSceneContentWithProgress(Action<string, float> progressCallback)
    {
        UpdateLoadProgress("Initializing Character Selection...", 0.0f);
        _logger.LogInformation(">>> TestAnimationScene LoadSceneContentWithProgress starting...");

        try
        {
            UpdateLoadProgress("Creating Select World...", 0.05f);
            _selectWorld = new SelectWorld();
            Controls.Add(_selectWorld);

            UpdateLoadProgress("Initializing Select World (Graphics)...", 0.1f);
            await _selectWorld.Initialize();
            World = _selectWorld;
            UpdateLoadProgress("Select World Initialized.", 0.35f); // Zwiększony postęp po inicjalizacji świata
            _logger.LogInformation("--- TestAnimationScene: SelectWorld initialized and set.");

            if (_selectWorld.Terrain != null)
            {
                _selectWorld.Terrain.AmbientLight = 0.6f;
            }
            UiState = TestAnimationUiState.EditCharacter;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "!!! TestAnimationScene: Error during world initialization or character creation.");
            UpdateLoadProgress("Error loading character selection.", 1.0f);
        }
        finally
        {
            _initialLoadComplete = true;
            UpdateLoadProgress("Character Selection Ready.", 1.0f);
            _logger.LogInformation("<<< TestAnimationScene LoadSceneContentWithProgress finished.");
        }
    }

    public override void AfterLoad()
    {
        base.AfterLoad();
        _logger.LogInformation("TestAnimationScene.AfterLoad() called.");
        if (_loadingScreen != null)
        {
            MuGame.ScheduleOnMainThread(() =>
            {
                if (_loadingScreen != null)
                {
                    Controls.Remove(_loadingScreen);
                    _loadingScreen.Dispose();
                    _loadingScreen = null;
                    Cursor?.BringToFront();
                    DebugPanel?.BringToFront();
                }
            });
        }
    }

    protected override void OnScreenSizeChanged()
    {
        base.OnScreenSizeChanged();
    }

    public override async Task Load()
    {
        if (Status == GameControlStatus.Initializing)
        {
            await LoadSceneContentWithProgress(UpdateLoadProgress);
        }
        else
        {
            _logger.LogDebug("TestAnimationScene.Load() called outside of InitializeWithProgressReporting flow. Re-routing to progressive load.");
            await LoadSceneContentWithProgress(UpdateLoadProgress);
        }
    }


    public override void Dispose()
    {
        _logger.LogDebug("Disposing TestAnimationScene.");
        if (_loadingScreen != null)
        {
            Controls.Remove(_loadingScreen);
            _loadingScreen.Dispose();
            _loadingScreen = null;
        }
        base.Dispose();
    }




    public override void Update(GameTime gameTime)
    {
        if (_loadingScreen != null && _loadingScreen.Visible)
        {
            _loadingScreen.Update(gameTime);
            Cursor?.Update(gameTime);
            DebugPanel?.Update(gameTime);
            return;
        }
        if (!_initialLoadComplete && Status == GameControlStatus.Initializing)
        {
            Cursor?.Update(gameTime);
            DebugPanel?.Update(gameTime);
            return;
        }
        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
    }

    private void RefreshCharacter()
    {
        if (!character.HasValue)
        {
            _ = _selectWorld.CreateCharacterObjects(new List<(string Name, PlayerClass Class, ushort Level, AppearanceConfig Appearance)>());
            return;
        }
        _ = _selectWorld.CreateCharacterObjects([character.Value]);
    }

    public void HandleChangeCharacterClass(object sender, KeyValuePair<string, int> newCharacter)
    {
        _selectArmorOptionControl.Value = null;

        // _selectArmorOptionControl.Options = 
        // Rebuild the class
        RefreshCharacter();

    }
    public void HandleChangeCharacterClassOptionPickerVisible(object sender, bool isShowPicker)
    {
        _selectVehicleOptionControl.Visible = !isShowPicker;
    }
    public void HandleChangeArmorSet(object sender, KeyValuePair<string, int> armor)
    {
        RefreshCharacter();
    }
    public void HandleChangeWing(object sender, KeyValuePair<string, int> wing)
    {
        RefreshCharacter();
    }
    public void HandleChangeLeftHand(object sender, KeyValuePair<string, int> weaponL)
    {
        RefreshCharacter();
    }
    public void HandleChangeRightHand(object sender, KeyValuePair<string, int> weaponR)
    {
        RefreshCharacter();
    }
    public void HandleChangePet(object sender, KeyValuePair<string, int> newPet)
    {
        Pet = newPet.Value;
    }
    public void HandleChangeRide(object sender, KeyValuePair<string, int> newRidingPet)
    {
        RefreshCharacter();
    }

    private void HandleGoToTestAnimationButtonClick(object sender, EventArgs e)
    {
        UiState = TestAnimationUiState.TestAction;
    }

    private void HandleGoToAppearanceConfigButtonClick(object sender, EventArgs e)
    {
        UiState = TestAnimationUiState.EditCharacter;
    }

    private void HandleChangeAnimation(object sender, KeyValuePair<string, int>? newAnimation)
    {
        if (!newAnimation.HasValue) return; 
        _selectWorld.PlayEmoteAnimation((PlayerAction)newAnimation.Value.Value);
    }
    
}

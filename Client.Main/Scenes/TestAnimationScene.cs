using Client.Main.Controls.UI;
using Client.Main.Controls.UI.Game;
using Client.Main.Models;
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


    private CharacterClassNumber characterClass = CharacterClassNumber.DarkWizard;

    public CharacterClassNumber CharacterClass
    {
        get => characterClass;
        set
        {
            if (characterClass == value) return;
            characterClass = value;
            // TODO: Raise character class change
        }
    }
    private int wing;
    public int Wing
    {
        get => wing;
        set => wing = value;
    }

    private int weaponLeft;
    public int WeaponLeft
    {
        get => weaponLeft;
        set => weaponLeft = value;
    }
    private int weaponRight;
    public int WeaponRight
    {
        get => weaponRight;
        set => weaponRight = value;
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
    private readonly SelectOptionControl _selectWeaponLeftOptionControl;
    private readonly SelectOptionControl _selectWeaponRightOptionControl;
    private readonly SelectOptionControl _selectCharacterClassOptionControl;
    private readonly SelectOptionControl _selectPetOptionControl;
    private readonly SelectOptionControl _selectRideOptionControl;


    private bool _initialLoadComplete = false;

    // Constructors
    public TestAnimationScene()
    {

        _logger = MuGame.AppLoggerFactory.CreateLogger<TestAnimationScene>();


        _loadingScreen = new LoadingScreenControl { Visible = true, Message = "Loading Scene" };
        Controls.Add(_loadingScreen);

        Controls.Add(_selectCharacterClassOptionControl = new SelectOptionControl()
        {
            Text = "Select Class",
            X = 10 + 5,
            Y = 10,
        });
        _selectCharacterClassOptionControl.ValueChanged += HandleChangeCharacterClass;
        
        Controls.Add(_selectArmorOptionControl = new SelectOptionControl()
        {
            Text = "Select Armor",
            X = 180 + 30 + 10 + 5,
            Y = 10,
        });
        _selectArmorOptionControl.ValueChanged += HandleChangeArmorSet;

        Controls.Add(_selectWeaponLeftOptionControl = new SelectOptionControl()
        {
            Text = "Select Weapon Left",
            X = (180 + 30) * 2 + 10 + 5,
            Y = 10,
        });
        _selectWeaponLeftOptionControl.ValueChanged += HandleChangeWeaponLeft;
        
        Controls.Add(_selectWeaponRightOptionControl = new SelectOptionControl()
        {
            Text = "Select Weapon Right",
            X = (180 + 30) * 3 + 10 + 5,
            Y = 10,
        });
        _selectWeaponRightOptionControl.ValueChanged += HandleChangeWeaponRight;
        
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
        
        Controls.Add(_selectRideOptionControl = new SelectOptionControl()
        {
            Text = "Select Riding Pet",
            ButtonAlign = ControlAlign.Bottom,
            Y = ViewSize.Y - 30 - 10,
        });
        _selectRideOptionControl.ValueChanged += HandleChangeRide;


        _loadingScreen.BringToFront();
    }
    

    private void RefreshUI()
    {
        switch (_uiState)
        {
            case TestAnimationUiState.Loading:
                {
                    break;
                }
            case TestAnimationUiState.EditCharacter:
                {
                    _loadingScreen.Visible = false;
                    _selectCharacterClassOptionControl.Options =
                    [
                        new(CharacterClassNumber.DarkWizard.ToString(), (int)CharacterClassNumber.DarkWizard),
                        new(CharacterClassNumber.SoulMaster.ToString(), (int)CharacterClassNumber.SoulMaster),
                        new(CharacterClassNumber.GrandMaster.ToString(), (int)CharacterClassNumber.GrandMaster),
                        new(CharacterClassNumber.DarkKnight.ToString(), (int)CharacterClassNumber.DarkKnight),
                        new(CharacterClassNumber.BladeKnight.ToString(), (int)CharacterClassNumber.BladeKnight),
                        new(CharacterClassNumber.BladeMaster.ToString(), (int)CharacterClassNumber.BladeMaster),
                        new(CharacterClassNumber.FairyElf.ToString(), (int)CharacterClassNumber.FairyElf),
                        new(CharacterClassNumber.MuseElf.ToString(), (int)CharacterClassNumber.MuseElf),
                        new(CharacterClassNumber.HighElf.ToString(), (int)CharacterClassNumber.HighElf),
                        new(CharacterClassNumber.MagicGladiator.ToString(), (int)CharacterClassNumber.MagicGladiator),
                        new(CharacterClassNumber.DuelMaster.ToString(), (int)CharacterClassNumber.DuelMaster),
                        new(CharacterClassNumber.DarkLord.ToString(), (int)CharacterClassNumber.DarkLord),
                        new(CharacterClassNumber.LordEmperor.ToString(), (int)CharacterClassNumber.LordEmperor),
                        new(CharacterClassNumber.Summoner.ToString(), (int)CharacterClassNumber.Summoner),
                        new(CharacterClassNumber.BloodySummoner.ToString(), (int)CharacterClassNumber.BloodySummoner),
                        new(CharacterClassNumber.DimensionMaster.ToString(), (int)CharacterClassNumber.DimensionMaster),
                        new(CharacterClassNumber.RageFighter.ToString(), (int)CharacterClassNumber.RageFighter),
                        new(CharacterClassNumber.FistMaster.ToString(), (int)CharacterClassNumber.FistMaster),
                    ];
                    _selectCharacterClassOptionControl.Visible = true;
                    _selectArmorOptionControl.Options =
                    [
                        new("Set 0", 0),
                        new("Set 1", 1),
                        new("Set 2", 2),
                        new("Set 3", 3),
                        new("Set 4", 4),
                        new("Set 5", 5),
                        new("Set 6", 6),
                        new("Set 7", 7),
                        new("Set 8", 8),
                        new("Set 9", 9),
                        new("Set 10", 10),
                    ];
                    _selectArmorOptionControl.Visible = true;
                    _selectWeaponLeftOptionControl.Visible = true;
                    _selectWeaponRightOptionControl.Visible = true;
                    _selectWingOptionControl.Visible = true;
                    _selectPetOptionControl.Visible = true;
                    _selectRideOptionControl.Visible = true;
                    break;
                }
            case TestAnimationUiState.TestAction:
                {
                    _loadingScreen.Visible = false;
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

    public void HandleChangeCharacterClass(object sender, KeyValuePair<string, int> character)
    {
        CharacterClass = (CharacterClassNumber)character.Value;
    }
    public void HandleChangeArmorSet(object sender, KeyValuePair<string, int> armor)
    {
        ArmorSet = armor.Value;
    }
    public void HandleChangeWing(object sender, KeyValuePair<string, int> wing)
    {
        Wing = wing.Value;
    }
    public void HandleChangeWeaponLeft(object sender, KeyValuePair<string, int> weaponL)
    {
        WeaponLeft = weaponL.Value;
    }
    public void HandleChangeWeaponRight(object sender, KeyValuePair<string, int> weaponR)
    {
        WeaponRight = weaponR.Value;
    }
    public void HandleChangePet(object sender, KeyValuePair<string, int> newPet)
    {
        Pet = newPet.Value;
    }
    public void HandleChangeRide(object sender, KeyValuePair<string, int> newRidingPet)
    {
        Ride = newRidingPet.Value;
    }
}

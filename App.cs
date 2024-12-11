using Microsoft.Xna.Framework;
using Sokoban.Controller;
using Sokoban.Models;
using Sokoban.Models.Levels;
using Sokoban.Models.Menu;
using Sokoban.Models.Profiles;
using Sokoban.View;
using static Sokoban.Shared;


namespace Sokoban
{
    public class App : Game
    {
        private const int MAX_WIDTH_IN_TILES = 22;
        private const int MAX_HEIGHT_IN_TILES = 12;
        private readonly IAppView view;
        private readonly IGameController controller;

        private Models.AppState state;

        private readonly AppStateContainer appStateContainer;

        public readonly GameModel GameModel;

        public readonly IProfileManager ProfileManager;

        public readonly ILevelLoader LevelLoader;

        public readonly IMenuModel MenuModel;

        public readonly ILevelEditor LevelEditor;

        public readonly ILevelCreator LevelCreator;

        public readonly ProfileStatistics ProfileStatistics;

        public bool StateWasChanged { get; private set; } = true;

        public string CurrentProfileName => ProfileManager.CurrentProfile.Name;

        public App()
        {
            controller = new KeyboardGameController(this);
            view = new MVPAppView(this);

            ProfileManager = new ProfileManager();
            ProfileStatistics = new ProfileStatistics(ProfileManager);
            LevelLoader = new LevelLoader(this);
            GameModel = new GameModel(this);
            MenuModel = new MenuModel(this);
            LevelEditor = new LevelEditor(LevelLoader);
            LevelCreator = new LevelCreator(MAX_WIDTH_IN_TILES, MAX_HEIGHT_IN_TILES, 3, 3);

            appStateContainer = new AppStateContainer(this, SetState);
            state = appStateContainer.InMenu;
        }

        public ViewListData GetListViewData() => state.GetListViewData();

        public ViewMapData GetMapViewData() => state.GetMapViewData();

        public Shared.ExternalAppState GetState()
        {
            StateWasChanged = false;
            return state.GetAppState();
        }

        public void ProcessCommand(InputCommand command)
        {
            StateWasChanged = true;
            state.ProcessCommand(command);
        }

        public void SaveAndExit()
        {
            ProfileManager.SaveProfilesInFile();
            LevelLoader.SaveAllInFile();
            Exit();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            view.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            controller.ProcessInput();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (StateWasChanged)
            {
                view.Draw();
            }
            base.Draw(gameTime);
        }

        private void SetState(Models.AppState appState)
        {
            state = appState;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Sokoban.Models.Levels;
using static Sokoban.Shared;

namespace Sokoban.Models.Menu
{

    public interface IMenuModel
    {
        public void AddMenuItem(int newLevelNumber, string packageName);
    }

    public class MenuModel : AppState, IMenuModel
    {
        private readonly App app;

        private readonly Stack<IMenu> menus;

        private readonly Stack<int> savedIndexes;

        private int currentIndex;

        private bool EditModeOn = false;

        private IReadOnlyList<IMenuItem> Items => menus.Peek().Items;

        public IMenuItem CurrentItem => Items[currentIndex];

        public MenuModel(App app)
        {
            this.app = app;
            menus = new Stack<IMenu>();
            savedIndexes = new Stack<int>();
            Initialize();
        }

        public override ViewListData GetListViewData()
        {
            var list = Items.Select(i => i.Name).ToList();
            return new ViewListData(list, currentIndex);
        }

        public void AddMenuItem(int levelNumber, string packageName)
        {
            var currentMenu = menus.Peek();
            if (currentMenu.Name == packageName)
            {
                var newLevelMenuItem = currentMenu.Items[^1];
                currentMenu.Remove("+");
                currentMenu.Add(new MenuItem(levelNumber.ToString(), () =>
                {
                    Container.SetState(Container.InGame);
                    app.GameModel.Start(app.LevelLoader.LoadLevel(levelNumber, packageName));
                }));
                currentMenu.Add(newLevelMenuItem);
            }
        }

        public override Shared.ExternalAppState GetAppState()
        {
            if (menus.Peek().Name == "Play")
            {
                return Shared.ExternalAppState.Packages;
            }
            if (menus.Peek().Name == "Profiles")
            {
                return Shared.ExternalAppState.Profiles;
            }
            return Shared.ExternalAppState.Menu;
        }

        public override void ProcessCommand(InputCommand command)
        {
            Action action;
            action = command switch
            {
                InputCommand.Left => MovePrevious,
                InputCommand.Right => MoveNext,
                InputCommand.Up => MovePrevious,
                InputCommand.Down => MoveNext,
                InputCommand.Enter => Enter,
                InputCommand.Back => Back,
                _ => () => { }
                ,
            };
            action();
        }

        private void Initialize()
        {
            var mainMenu = new Menu("Main");
            mainMenu.Add(GetPlayMenu());
            mainMenu.Add(GetProfileSelectMenu());
            mainMenu.Add(new MenuItem("Statistics", () =>
            {
                Container.SetState(Container.InStatistics);
            }));
            mainMenu.Add(GetSettingsMenu());
            mainMenu.Add(new MenuItem("Exit", () =>
            {
                app.SaveAndExit();
            }));
            menus.Push(mainMenu);
        }

        private IMenu GetPlayMenu()
        {
            var playMenu = new Menu("Play");
            var packageToLevels = app.LevelLoader.GetPackageNameToLevelNumbers();
            foreach (var packageName in packageToLevels.Keys)
            {
                var packageMenu = new Menu(packageName);
                var levelNumbers = packageToLevels[packageName];
                foreach (var levelNumber in levelNumbers)
                {
                    packageMenu.Add(new MenuItem(levelNumber.ToString(), () =>
                    {
                        Container.SetState(Container.InGame);
                        app.GameModel.Start(app.LevelLoader.LoadLevel(levelNumber, packageName));
                    }));
                }
                packageMenu.Add(new MenuItem("+", () =>
                {
                    (Container.InCreator as ILevelCreator).CreateLevel(menus.Peek().Name, app.ProfileManager.CurrentProfile.Name);
                    Container.SetState(Container.InCreator);
                }));
                playMenu.Add(packageMenu);
            }
            return playMenu;
        }

        private IMenu GetProfileSelectMenu()
        {
            var profilesMenu = new Menu("Profiles");
            foreach (var profile in app.ProfileManager.Profiles)
            {
                profilesMenu.Add(new MenuItem(profile.Name, () =>
                {
                    app.ProfileManager.SelectProfile(profile);
                    Back();
                }));
            }
            return profilesMenu;
        }

        private IMenu GetSettingsMenu()
        {
            var settingsMenu = new Menu("Settings");
            settingsMenu.Add(new MenuItem("Sound", () => { }));
            settingsMenu.Add(new MenuItem("Home", Back));
            return settingsMenu;
        }

        private void MoveNext()
        {
            currentIndex = currentIndex == Items.Count - 1 ? 0 : ++currentIndex;
        }

        private void MovePrevious()
        {
            currentIndex = currentIndex == 0 ? Items.Count - 1 : --currentIndex;
        }

        private void Enter()
        {
            if (CurrentItem is IMenu)
            {
                OpenNextMenu();
            }
            else
            {
                CurrentItem.Enter();
            }
        }

        private void Back()
        {
            if (menus.Count <= 1)
            {
                app.SaveAndExit();
            }
            else
            {
                OpenPreviousMenu();
            }
        }

        private void OpenNextMenu()
        {
            menus.Push(CurrentItem as IMenu);
            savedIndexes.Push(currentIndex);
            currentIndex = 0;
        }

        private void OpenPreviousMenu()
        {
            menus.Pop();
            currentIndex = savedIndexes.Pop();
        }
    }
}

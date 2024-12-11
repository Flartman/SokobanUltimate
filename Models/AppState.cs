using System;
using static Sokoban.Shared;

namespace Sokoban.Models
{
    public abstract class AppState
    {
        public abstract void ProcessCommand(InputCommand command);

        public virtual ViewListData GetListViewData()
        {
            return null;
        }

        public virtual ViewMapData GetMapViewData()
        {
            return null;
        }

        public abstract Shared.ExternalAppState GetAppState();

        protected AppStateContainer Container { get; private set; }

        public void SetContainer(AppStateContainer appStateContainer)
        {
            Container = appStateContainer;
        }
    }


    public class AppStateContainer
    {
        public AppState InGame { get; }

        public AppState InMenu { get; }

        public AppState InEditor { get; }

        public AppState InCreator { get; }

        public AppState InStatistics { get;  }

        public Action<AppState> SetState;

        public AppStateContainer(App app, Action<AppState> setState)
        {
            SetState = setState;
            InGame = (AppState)app.GameModel;
            InGame.SetContainer(this);
            InCreator = (AppState)app.LevelCreator;
            InCreator.SetContainer(this);
            InEditor = (AppState)app.LevelEditor;
            InEditor.SetContainer(this);
            InMenu = (AppState)app.MenuModel;
            InMenu.SetContainer(this);
            InStatistics = (AppState)app.ProfileStatistics;
            InStatistics.SetContainer(this);
        }
    }
}

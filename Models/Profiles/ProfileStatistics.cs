using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Sokoban.Shared;

namespace Sokoban.Models.Profiles
{
    public class ProfileStatistics : AppState
    {
        private int levelsCompleted => profileManager.CurrentProfile.Results.Count;

        private Dictionary<string, int> levelCounts;

        private readonly IProfileManager profileManager;

        public ProfileStatistics(IProfileManager profileManager)
        {
             this.profileManager = profileManager;
        }

        public override Shared.ExternalAppState GetAppState()
        {
            return Shared.ExternalAppState.Statistics;
        }

        public override void ProcessCommand(InputCommand command)
        {
            Back();
        }

        public override ViewListData GetListViewData()
        {
            var list = new List<string>();
            list.Add("Levels completed: " + levelsCompleted.ToString());
            return new ViewListData(list, -1);
        }

        private void Back()
        {
            Container.SetState(Container.InMenu);
        }
    }
}

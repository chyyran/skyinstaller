using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrailsHelper.ViewModels
{
    public class InstallViewModel : ViewModelBase
    {
        public InstallViewModel(GameDisplayViewModel gameModel)
        {
            this.GameModel = gameModel;
        }

        public GameDisplayViewModel GameModel { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace Chicken4WP8.ViewModels.Home
{
    public class HomePageViewModel : Conductor<Screen>.Collection.OneActive
    {
        private IndexViewModel index;

        public HomePageViewModel(
            IndexViewModel index
            )
        {
            this.index = index;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Items.Add(index);

            ActivateItem(index);
        }
    }
}

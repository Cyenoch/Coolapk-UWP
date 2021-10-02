using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coolapk.ViewModels.Home
{
    public partial class HomeViewModel : ReactiveObject
    {
        [Reactive]
        public string Title { get; set; }

        public ReactiveCommand<Unit, Unit> ChangeTitleCommand { get; }

        public HomeViewModel()
        {
            Title = "WTF";
            ChangeTitleCommand = ReactiveCommand.CreateFromObservable(ExceuteChangeTitleCommand);
        }

        private IObservable<Unit> ExceuteChangeTitleCommand()
        {
            Title = "WWWWT";
            return Observable.Return(Unit.Default);
        }
    }
}

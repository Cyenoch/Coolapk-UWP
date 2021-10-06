using Coolapk.Apis;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coolapk.ViewModels
{
    public abstract class ViewModelBase : ReactiveObject
    {
        public ICoolapkApis ApiService =>
                Locator.Current.GetService<ICoolapkApis>();
    }

    public class ApiRequestViewModelBase : ViewModelBase
    {
        [Reactive]
        public bool IsInitializeLoading { get; set; }

        [Reactive]
        public bool IsRequested { get; set; }

        [Reactive]
        public bool IsFailed { get; set; }

        [Reactive]
        public string ErrorText { get; set; }
    }

    public class DeltaRequestViewModelBase : ApiRequestViewModelBase
    {
        [Reactive]
        public bool IsDeltaLoading { get; set; }

    }

    public class DataListViewModelBase : ViewModelBase
    {

    }

    public interface IApiRequestViewModel
    {
        Task InitializeRequestAsync();
    }

    public interface IDeltaRequestViewModel : IApiRequestViewModel
    {
        Task DeltaRequestDataAsync();
    }
}

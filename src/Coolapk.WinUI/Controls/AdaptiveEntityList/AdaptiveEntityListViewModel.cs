using Coolapk.Models;
using Coolapk.ViewModels;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;

namespace Coolapk.WinUI.Controls.AdaptiveEntityList
{
    public class EntityDataWrapper
    {
        public Entity Unwrapped { get; set; }
        public AdaptiveEntityListViewModel ViewModel { get; protected set; }
        public EntityDataWrapper(Entity entity, AdaptiveEntityListViewModel vm)
        {
            ViewModel = vm;
            Unwrapped = entity.AutoCast();
        }
    }

    public class IncrementalEntityDataCollection : ObservableCollection<EntityDataWrapper>, ISupportIncrementalLoading
    {
        private AdaptiveEntityListViewModel ViewModel { get; set; }

        public IncrementalEntityDataCollection(AdaptiveEntityListViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return AsyncInfo.Run(async cancelToken =>
            {
                
                var countBeforeLoad = ViewModel.Data.Count;
                await ViewModel.NextPage();
                var countAfterLoaded = ViewModel.Data.Count;
                var loadCount = countAfterLoaded - countBeforeLoad;
                return new LoadMoreItemsResult()
                {
                    Count = (uint)loadCount
                };
            });
        }

        public bool HasMoreItems => !ViewModel.IsFinished;
    }

    public class AdaptiveEntityListViewModel : ApiRequestViewModelBase, IDeltaRequestViewModel
    {
        [Reactive]
        public uint Page { get; private set; }

        [Reactive]
        public bool IsFinished { get; private set; }

        [Reactive]
        public IncrementalEntityDataCollection Data { get; private set; }

        public DataFetcher Fetcher { get; private set; }

        public AdaptiveEntityListViewModel(DataFetcher fetcher)
        {
            Data = new IncrementalEntityDataCollection(this);
            Page = 1;
            IsFinished = false;
            IsFailed = false;
            IsRequested = false;
            IsInitializeLoading = true;
            Fetcher = fetcher;
        }

        public Task NextPage()
        {
            Page++;
            return DeltaRequestDataAsync();
        }

        public async Task DeltaRequestDataAsync()
        {
            if (IsFinished || IsFailed) return;
            IsRequested = false;
            try
            {
                var resp = await Fetcher(this);
                if (resp.Count == 0)
                    IsFinished = true;
                resp.ToList().ForEach(item => Data.Add(new EntityDataWrapper(item, this)));
            }
            catch (Exception ex)
            {
                IsFailed = true;
                ErrorText = ex.Message;
            }
            finally
            {
                IsRequested = true;
            }
        }

        public async Task InitializeRequestAsync()
        {
            IsFinished = false;
            ErrorText = null;
            IsFailed = false;
            IsInitializeLoading = true;
            IsRequested = false;
            Page = 1;
            Data.Clear();
            try
            {
                var resp = await Fetcher(this);
                if (resp == null) return;
                if (resp.Count == 0)
                    IsFinished = true;
                resp.ToList().ForEach(item => Data.Add(new EntityDataWrapper(item, this)));
            }
            catch (Exception ex)
            {
                IsFailed = true;
                ErrorText = ex.Message;
            }
            finally
            {
                IsRequested = true;
                IsInitializeLoading = false;
            }
        }

        public delegate Task<IList<Entity>> DataFetcher(AdaptiveEntityListViewModel viewModel);
    }


}

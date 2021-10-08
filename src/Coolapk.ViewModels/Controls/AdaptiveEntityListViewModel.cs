using Coolapk.Models;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coolapk.ViewModels.Controls
{
    public class AdaptiveEntityListViewModel : ApiRequestViewModelBase, IDeltaRequestViewModel
    {
        [Reactive]
        public uint Page { get; private set; }

        [Reactive]
        public bool IsFinished { get; private set; }

        [Reactive]
        public ObservableCollection<Entity> Data { get; private set; }

        public DataFetcher Fetcher { get; private set; }

        public AdaptiveEntityListViewModel(DataFetcher fetcher)
        {
            Page = 1;
            Data = new ObservableCollection<Entity>();
            IsFinished = false;
            IsFailed = false;
            IsRequested = false;
            IsInitializeLoading = true;
            this.Fetcher = fetcher;
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
                resp.ToList().ForEach(item => Data.Add(item.AutoCast()));
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
                resp.ToList().ForEach(item => Data.Add(item.AutoCast()));
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

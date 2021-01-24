using Coolapk_UWP.Models;
using Coolapk_UWP.Network;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Coolapk_UWP.ViewModels {
    public class BaseViewModel : INotifyPropertyChanged {
        public AppViewModel AppViewModel { get { return App.AppViewModel; } }
        public ICoolapkApis CoolapkApis { get { return App.AppViewModel.CoolapkApis; } }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyChanged([CallerMemberName] string propertyName = null) {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


    public interface IAsyncLoadViewModel<T> {
        Task<RespBase<T>> OnLoadAsync();
        string[] NotifyChangedProperties();
    }

    public abstract class AsyncLoadViewModel<T> : BaseViewModel, IAsyncLoadViewModel<T> {
        public bool _busy = false;
        public bool Busy {
            get { return _busy; }
            set { _busy = value; NotifyChanged(); }
        }

        public string _errMsg;
        public string ErrorMessage {
            get { return _errMsg; }
            set { _errMsg = value; NotifyChanged(); }
        }

        public T _data;
        public T Data {
            get { return _data; }
            set {
                _data = value; NotifyChanged(); foreach (var prop in NotifyChangedProperties()) {
                    NotifyChanged(prop);
                }
            }
        }

        //public AsyncLoadViewModel() {
        //    _doOnLoad();
        //}

        private async void _doOnLoad() {
            Busy = true;
            Data = default;
            ErrorMessage = null;
            try {
                Data = (await OnLoadAsync()).Data;
            } catch (Exception exception) {
                ErrorMessage = exception.Message;
            } finally {
                Busy = false;
            }
        }

        public virtual void Reload() {
            _doOnLoad();
        }

        public abstract Task<RespBase<T>> OnLoadAsync();

        // Data字段更新时，同时有哪些字段也要通知改变
        virtual public string[] NotifyChangedProperties() {
            return new string[] { };
        }
    }
}

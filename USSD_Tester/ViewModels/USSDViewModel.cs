using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using USSD.App.Models;
using USSD.App.TCP;

namespace USSD_Tester.ViewModels
{
    public class USSDViewModel
    {
        private USSDData _data;
        private readonly ITCPClient _client;

        public USSDViewModel(ITCPClient client)
        {
            _data = new USSDData();
            _client = client;
        }

        public USSDData Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }
        public async Task SendData(string data)
        {
            await _client.SendData(data);
        }
        private ICommand mUpdater;
        public ICommand UpdateCommand
        {
            get
            {
                if (mUpdater == null)
                {
                    Action action = async () =>
                    {
                        var req = JsonConvert.SerializeObject(new AdapterRequest
                        {
                            PhoneNumber = "0881286653",
                            ScreenID = 1,
                            ServiceProvider = "tnm",
                            SessionID = "12345",
                            Stage = -1,
                            Tag = "0",                            
                            USSDString = _data.Request
                        });
                        await SendData(req + "\n");

                    };
                    mUpdater = new Updater(action);
                }
                return mUpdater;
            }
            set
            {
                mUpdater = value;
            }
        }

        private class Updater : ICommand
        {
            #region ICommand Members  

            private readonly Action _action;

            public Updater(Action action)
            {
                _action = action;
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }


            public async void Execute(object parameter)
            {
                
                await Task.Run(() => _action());
            }

            #endregion
        }
    }
}

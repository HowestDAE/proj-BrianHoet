using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ValorantAPI.Model;
using ValorantAPI.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ValorantAPI.Repository;

namespace ValorantAPI.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        public string CommandText
        {
            get
            {
                if (CurrentPage is OverViewPage)
                {
                    return "SHOW DETAILS";
                }
                else
                {
                    return "GO BACK";
                }
            }
        }

        public OverViewPage MainPage { get; } = new OverViewPage();
        public DetailPage AgentPage { get; } = new DetailPage();
        public Page CurrentPage { get; set; }

        public RelayCommand SwitchPageCommand { get; private set; }

        public void SwitchPage()
        {
            if (CurrentPage is OverViewPage)
            {
                Agent selectedAgent = (MainPage.DataContext as OverViewVM).SelectedAgent;
                if (selectedAgent == null) return;

                (AgentPage.DataContext as DetailPageVM).CurrentAgent = selectedAgent;

                CurrentPage = AgentPage;
            }
            else
            {
                CurrentPage = MainPage;
            }

            OnPropertyChanged(nameof(CurrentPage));
            OnPropertyChanged(nameof(CommandText));
        }

        public MainViewModel()
        {
            CurrentPage = MainPage;
            SwitchPageCommand = new RelayCommand(SwitchPage);
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using ValorantAPI.Model;

namespace ValorantAPI.ViewModel
{
    internal class DetailPageVM : ObservableObject
    {
        private Agent _currentAgent = new Agent()
        {
            Id = "6f2a04ca-43e0-be17-7f36-b3908627744d",
            Name = "Skye",
            RoleIcon = "https://media.valorant-api.com/agents/roles/1b47567f-8f7b-444b-aae3-b0c634622d10/displayicon.png",
            RoleName = "Initiator"
        };
        public Agent CurrentAgent
        {
            get => _currentAgent;
            set
            {
                _currentAgent = value;
                OnPropertyChanged(nameof(CurrentAgent));
            }
        }
    }
}

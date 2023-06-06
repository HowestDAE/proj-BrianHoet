using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json.Linq;
using ValorantAPI.Model;
using ValorantAPI.Repository;
using ValorantAPI.View;

namespace ValorantAPI.ViewModel
{
    internal class OverViewVM : ObservableObject
    {
        private bool _RepositoryState { get; set; } = true;
        private SolidColorBrush _buttonBackground;

        public List<Agent> Agents { get; set; }
        public List<string> AgentRoles { get; set; }

        AgentRepository _agentRepository;
        private Agent _selectedAgent;
        private string _selectedRole;

        public RelayCommand RepositoryCommand { get; private set; }

        public string SelectedRole
        {
            get { return _selectedRole; }
            set
            {
                if (_RepositoryState)
                {
                    FilterAgentsByRole(value);
                }
                else
                {
                    Agents = AgentRepositoryLocal.GetAgentsLocal(value);
                    OnPropertyChanged(nameof(Agents));

                    _selectedRole = value;
                }
            }
        }

        public Agent SelectedAgent
        {
            get { return _selectedAgent; }
            set { _selectedAgent = value; }
        }

        public string CommandText
        {
            get
            {
                if (_RepositoryState)
                {
                    return "Online";
                }
                else
                {
                    return "Offline";
                }
            }
        }

        public SolidColorBrush ButtonBackground
        {
            get { return _buttonBackground; }
            set
            {
                _buttonBackground = value;
                OnPropertyChanged(nameof(ButtonBackground));
            }
        }

        public OverViewVM()
        {
            RepositoryCommand = new RelayCommand(SwitchRepositoryState);
            SetButtonBackground();

            if (_RepositoryState)
            {
                _agentRepository = new AgentRepository();
                
                var taskRes = Task.Run(() =>
                {
                    var agentList = _agentRepository.GetAgentsAsync();
                    return agentList;
                });
                taskRes.ConfigureAwait(true).GetAwaiter().OnCompleted(
                () =>
                {
                    Agents = taskRes.Result;
                
                    OnPropertyChanged(nameof(Agents));
                });
                InitializeAgentRolesAsync();

                
            }
            else
            {
                Agents = AgentRepositoryLocal.GetAgentsLocal();
                AgentRoles = AgentRepositoryLocal.GetAgentsTypesLocal();
            }
        }

        private async Task InitializeAgentRolesAsync()
        {
            try
            {
                AgentRoles = await _agentRepository.GetAgentRolesAsync();
                OnPropertyChanged(nameof(AgentRoles));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to initialize agent roles: " + ex.Message);
            }
        }

        private void FilterAgentsByRole(string value)
        {
            var taskRes = Task.Run(() =>
            {
                var agentList = _agentRepository.GetAgentsAsync(value);
                return agentList;
            });
            taskRes.ConfigureAwait(true).GetAwaiter().OnCompleted(
                () =>
                {
                    Agents = taskRes.Result;
                    _selectedRole = value;
                    OnPropertyChanged(nameof(Agents));
                });
        }

        public void SwitchRepositoryState()
        {
            _RepositoryState = !_RepositoryState;
            OnPropertyChanged(nameof(CommandText));
            SetButtonBackground();
        }

        private void SetButtonBackground()
        {
            if (_RepositoryState)
            {
                ButtonBackground = new SolidColorBrush(Colors.CadetBlue);
            }
            else
            {
                ButtonBackground = new SolidColorBrush(Colors.Red); // Change to the desired color
            }
        }
    }
}

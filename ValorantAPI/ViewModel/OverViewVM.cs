using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json.Linq;
using ValorantAPI.Model;
using ValorantAPI.Repository;

namespace ValorantAPI.ViewModel
{
    internal class OverViewVM : ObservableObject
    {
        public List<Agent> Agents { get; set; }
        public List<string> AgentRoles { get; set; }

        AgentRepository _agentRepository;
        private Agent _selectedAgent;
        private string _selectedRole;

        public string SelectedRole
        {
            get { return _selectedRole; }
            set
            {
                FilterAgentsByRole(value);
            }
        }

        public Agent SelectedAgent
        {
            get { return _selectedAgent; }
            set { _selectedAgent = value; }
        }

        public OverViewVM()
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
    }
}

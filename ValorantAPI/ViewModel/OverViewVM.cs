using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
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
        _selectedRole = value;
        OnPropertyChanged(nameof(SelectedRole));

        // Filter the agents based on the selected role
        FilterAgentsByRole(_selectedRole);
    }
}

private void FilterAgentsByRole(string selectedRole)
{
    try
    {
        // Filter the agents based on the selected role
        List<Agent> filteredAgents = Agents.Where(agent => agent.RoleName == selectedRole).ToList();

        // Update the Agents property with the filtered agents
        Agents = filteredAgents;
        OnPropertyChanged(nameof(Agents));
    }
    catch (Exception)
    {
        Console.WriteLine("Failed to filter agents by role");
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
    }
}

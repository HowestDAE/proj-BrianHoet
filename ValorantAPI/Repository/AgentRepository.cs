using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ValorantAPI.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ValorantAPI.Repository
{
    internal class AgentRepository
    {
        private static List<Agent> _agents;

        public async Task<List<Agent>> GetAgentsAsync()
        {
            string url = "https://valorant-api.com/v1/agents";

            _agents = new List<Agent>();

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                        throw new HttpRequestException(response.ReasonPhrase);

                    // Read JSON string from API asynchronously (await result)
                    string json = await response.Content.ReadAsStringAsync();

                    // Deserialize JSON
                    var data = JsonConvert.DeserializeObject<JObject>(json)["data"];
                    if (data == null || !data.HasValues)
                        throw new Exception("No agents found");

                    // Iterate over each agent in the "data" array
                    foreach (var agent in data)
                    {
                        var check = agent.Value<string>("isPlayableCharacter");

                        if (check == "True")
                        {
                            Agent newAgent = new Agent();

                            newAgent.Name = agent.Value<string>("displayName");
                            newAgent.Description = agent.Value<string>("description");
                            newAgent.LittleIcon = agent.Value<string>("displayIconSmall");
                            newAgent.BackGround = agent.Value<string>("background");
                            newAgent.FullPortrait = agent.Value<string>("fullPortrait");

                            // Deserialize the "role" information
                            var roleData = agent.Value<JObject>("role");
                            if (roleData != null)
                            {
                                newAgent.RoleName = roleData.Value<string>("displayName");
                                newAgent.RoleIcon = roleData.Value<string>("displayIcon");
                            }

                            var abilitiesData = agent.Value<JArray>("abilities");
                            if (abilitiesData != null)
                            {
                                newAgent.AbilitiesName = new List<string>();
                                newAgent.AbilitiesIconName = new List<string>();

                                foreach (var ability in abilitiesData)
                                {
                                    newAgent.AbilitiesName.Add(ability.Value<string>("displayName"));
                                    newAgent.AbilitiesIconName.Add(ability.Value<string>("displayIcon"));
                                }
                            }

                            _agents.Add(newAgent);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error retrieving agents: " + ex.Message);
                }
            }

            return _agents;
        }

        public async Task<List<string>> GetAgentRolesAsync()
        {
            var url = "https://valorant-api.com/v1/agents";
        
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(url);
        
                    if (!response.IsSuccessStatusCode)
                        throw new HttpRequestException(response.ReasonPhrase);
        
                    // Read the JSON response
                    var json = await response.Content.ReadAsStringAsync();
        
                    // Deserialize the JSON response
                    var agentsResponse = JsonConvert.DeserializeObject<JObject>(json);
        
                    // Get the "data" array from the response
                    var agentsArray = agentsResponse["data"];
        
                    // Create a list to store the role display names
                    var roleDisplayNames = new List<string>();
        
                    // Iterate through each agent and extract the role display name
                    foreach (var agent in agentsArray)
                    {
                        var roleName = agent.Value<JObject>("role")?.Value<string>("displayName");
                        if (!string.IsNullOrEmpty(roleName))
                        {
                            roleDisplayNames.Add(roleName);
                        }
                    }
        
                    // Remove duplicates from the list
                    roleDisplayNames = roleDisplayNames.Distinct().ToList();
        
                    // Add a default value to the list
                    roleDisplayNames.Insert(0, "All Roles");
        
                    return roleDisplayNames;
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to retrieve agent roles");
                    throw;
                }
            }
        }


    }
}

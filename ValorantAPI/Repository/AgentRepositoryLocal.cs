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
    internal class AgentRepositoryLocal
    {
        private static List<Agent> _agents;

        public static List<Agent> GetAgentsLocal()
        {
            if (_agents != null)
            {
                return _agents;
            }

            _agents = new List<Agent>();

            string jsonString = File.ReadAllText("../../Resources/Data/ValorantAgents.json");

            // Deserialize JSON
            var data = JsonConvert.DeserializeObject<JObject>(jsonString)["data"];
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

            return _agents;
        }

        public static List<string> GetAgentsTypesLocal()
        {
            if (_agents == null)
            {
                GetAgentsLocal();
            }

            List<string> types = _agents.Select(agent => agent.RoleName).Distinct().ToList();
            types.Add("All types");

            return types;
        }


        public List<Agent> GetAgentLocal(string type)
        {
            if (_agents == null)
            {
                GetAgentsLocal();
            }

            if (type.Equals("All types"))
            {
                return GetAgentsLocal();
            }

            return _agents.Where(agent => agent.RoleName.Equals(type)).ToList();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValorantAPI.Model
{
    public class Agent
    {
        //Names
        public string Name { get; set; }
        public string Description { get; set; }
        public string RoleName { get; set; }
        
        //Images
        public string LittleIcon { get; set; }
        public string RoleIcon { get; set; }
        public string BackGround { get; set; }

        public string FullPortrait { get; set; }

        public List<string> AbilitiesName { get; set; }
        public List<string> AbilitiesIconName { get; set; }
    }

}

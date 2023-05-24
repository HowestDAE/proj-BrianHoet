using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValorantAPI.Model
{
    public class Agent
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string RoleName { get; set; }
        public string RoleIcon { get; set; }

        //public int Weight { get; set; }
        //public int Height { get; set; }
        //
        public string LittleIcon { get; set; }
        public string BackGround { get; set; }
    }

}

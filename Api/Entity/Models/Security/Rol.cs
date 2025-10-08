using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models.Security
{
    public class Rol : GenericModel
    {
        public string Description { get; set; }
<<<<<<< HEAD
        public virtual ICollection<RolFormPermission> RolFormPermission { get; set; } = new List<RolFormPermission>();

=======
>>>>>>> 6ca5cd43fe20de25915aa779da030276258b25d7

    }
}

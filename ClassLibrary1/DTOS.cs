using SistemaAcademico.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaAcademico.DTOS
{
    public class MenuOptionsDTO
    {
        public MenuOption OpcionDeMenu { get; set; }
        public List<MenuOption> Childs { get; set; }
    }
}

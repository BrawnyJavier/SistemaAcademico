using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaAcademico.SchemaTypes
{

    public enum UserTypes
    {
        Student,
        Teacher,
        Administrative
    }
    public enum revisionStatus
    {
        EnEspera,
        Procede,
        NoProcede,
        Cancelada
    }
    public enum StudentMajorStatus
    {
        Apartado,
        Cursando,
        Egresado
    }
    public enum AsignatureTypes
    {
        Laboratory,
        Threory
    }
    public enum RequisitoTypes
    {
        Creditos,
        Asignatura
    }
    public enum PeriodStatus
    {
        Completado,
        En_Curso
    }
    public enum HistorialStatus
    {
        En_Curso,
        Aprobada,
        Reprobada,
        Retirada,
        Cancelada
    }
}

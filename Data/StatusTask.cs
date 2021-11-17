using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ReportServiceWeb02.Data
{
    public class StatusTask : IStatus
    {
        public List<Reportes> Reportes;

        public StatusTask()
        {
            Reportes = new List<Reportes>();

            Reportes.Add(new Data.Reportes { Name = "Reporte Diario", Ids = "RepD", Status = "Habilitado", Command="Deshabilitar" });
            Reportes.Add(new Data.Reportes { Name = "Reporte EDAC", Ids = "RepE", Status = "Habilitado", Command = "Deshabilitar" });
            Reportes.Add(new Data.Reportes { Name = "Reporte RTU", Ids = "RepR", Status = "Habilitado", Command = "Deshabilitar" });
        }

        public string Check(string id)
        {
            var rep = Reportes.Where(w => w.Ids == id).FirstOrDefault();

            return rep.Status;
        }

        public void Deshabilitar(string id)
        {
            var rep = Reportes.Where(w => w.Ids == id).FirstOrDefault();

            rep.Status = "Deshabilitado";
            rep.Command = "Habilitar";
        }

        public void Habilitar(string id)
        {
            var rep = Reportes.Where(w => w.Ids == id).FirstOrDefault();

            rep.Status = "Habilitado";
            rep.Command = "Deshabilitar";
        }
    }
}

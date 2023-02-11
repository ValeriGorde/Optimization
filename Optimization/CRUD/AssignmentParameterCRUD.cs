using Optimization.DB_EF;
using Optimization.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimization.CRUD
{
    internal class AssignmentParameterCRUD : ICRUD<AssignmentParameter>
    {
        public List<AssignmentParameter> _AssignParam { get; set; }
        ApplicationContext context;

        public AssignmentParameterCRUD() 
        {
            context = new ApplicationContext();
            _AssignParam = context.AssignmentParameters.ToList();       
        }
        public void Create(AssignmentParameter item)
        {
            context.Add(item);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var assignParam = _AssignParam.FirstOrDefault(a => a.Id == id);
            context.Remove(assignParam);
            context.SaveChanges();
        }

        public bool Read(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(AssignmentParameter item)
        {
            var assignParam = _AssignParam.FirstOrDefault(a => a.ParameterId == item.ParameterId && a.AssignmentId == item.AssignmentId);
            assignParam.Value = item.Value;
            assignParam.ParameterId = item.ParameterId;
            assignParam.AssignmentId = item.AssignmentId;

            context.Update(assignParam);
            context.SaveChanges();

        }

        public void Delete(AssignmentParameter item)
        {
            context.Remove(item);
            context.SaveChanges();
        }
    }
}

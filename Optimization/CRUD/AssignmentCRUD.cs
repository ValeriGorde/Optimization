using Optimization.DB_EF;
using Optimization.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Optimization.CRUD
{
    internal class AssignmentCRUD: ICRUD<Assignment>
    {
        public List<Assignment> _Assignment { get; set; }
        ApplicationContext context;
        public AssignmentCRUD() 
        {
            context = new ApplicationContext();
            _Assignment = context.Assignments.ToList();
        }

        public void Create(Assignment item)
        {
            context.Assignments.Add(item);
            context.SaveChanges();
        }

        public bool Read(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Assignment item)
        {
            var assignment = _Assignment.FirstOrDefault(a => a.Id == item.Id);
            assignment.Name = item.Name;
            assignment.Description = item.Description;
            
            context.Update(assignment);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var assignment = _Assignment.FirstOrDefault(a => a.Id == id);
            context.Assignments.Remove(assignment);
            context.SaveChanges();
        }

        public void Delete(Assignment item)
        {
            throw new NotImplementedException();
        }
    }
}

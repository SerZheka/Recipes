using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MyRecipes.DataAccess.Interfaces;
using MyRecipes.DataAccess.Models;

namespace MyRecipes.DataAccess.Sql
{
    public class SqlStepData : IStepData
    {
        private readonly MyRecipesDbContext db;

        public SqlStepData(MyRecipesDbContext db)
        {
            this.db = db;
        }

        public Step Add(Step newStep)
        {
            db.Add(newStep);
            return newStep;
        }

        public int Commit()
        {
            return db.SaveChanges();
        }

        public Step Delete(int id)
        {
            var step = GetById(id);
            if (step != null)
            {
                db.Steps.Remove(step);
            }
            return step;
        }

        public Step GetById(int id)
        {
            return db.Steps.Find(id);
        }

        public IEnumerable<Step> GetStepsByTitle(string title)
        {
            var query = db.Steps
                .Where(r => r.Title.StartsWith(title) || string.IsNullOrEmpty(title))
                .OrderBy(r => r.Title);
            return query;
        }

        public Step Update(Step updatedStep)
        {
            var entity = db.Steps.Attach(updatedStep);
            entity.State = EntityState.Modified;
            return updatedStep;
        }
    }
}

using System.Collections.Generic;
using MyRecipes.DataAccess.Models;

namespace MyRecipes.DataAccess.Interfaces
{
    public interface IStepData
    {
        IEnumerable<Step>GetStepsByTitle(string title);
        Step GetById(int id);
        Step Update(Step updatedIngredient);
        Step Add(Step newIngredient);
        Step Delete(int id);
        int Commit();
    }
}

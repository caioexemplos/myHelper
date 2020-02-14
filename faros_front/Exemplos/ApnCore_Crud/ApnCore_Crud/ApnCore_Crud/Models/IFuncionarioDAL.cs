using System.Collections.Generic;

namespace ApnCore_Crud.Models
{
    public interface IFuncionarioDAL
    {
        IEnumerable<Funcionario> GetAllFuncionarios();
        void AddFuncionario(Funcionario funcionario);
        void UpdateFuncionario(Funcionario funcionario);
        Funcionario GetFuncionario(int? id);
        void DeleteFuncionario(int? id);
    }
}

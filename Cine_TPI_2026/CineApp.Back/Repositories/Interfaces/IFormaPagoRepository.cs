using CineApp.Back.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineApp.Back.Repositories.Interfaces
{
    public interface IFormaPagoRepository
    {
        Task<List<FormasPago>> GetAllFormasPagoAsync();
    }
}

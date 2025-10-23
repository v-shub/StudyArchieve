using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace Domain.Interfaces
{
    public interface ITagRepository : IRepositoryBase<Tag>
    {
        Task<List<Tag>> GetByIdsAsync(IEnumerable<int> ids);
        Task AttachAsync(Tag tag);
    }
}

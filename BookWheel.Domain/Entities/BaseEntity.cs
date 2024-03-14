using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Domain.Entities
{
    public interface IBaseEntity
    {
        public List<BaseDomainEvent> Events { get; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime DeletedAt { get; set; }
    }
    public class BaseEntity<TId> : IBaseEntity
    {
        public TId Id { get; set; }

        [NotMapped]
        public List<BaseDomainEvent> Events { get; protected set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime DeletedAt { get; set; }
    }
}

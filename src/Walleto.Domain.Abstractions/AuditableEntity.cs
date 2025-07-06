using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walleto.Domain.Abstractions
{
    public abstract class AuditableEntity : Entity
    {
        public bool IsActive { get; protected set; } = true;
        
        public bool IsDeleted { get; protected set; } = false;
        
        public int CreatorId { get; protected set; }
        
        public DateTime CreationDateTime { get; protected set; }
        
        public int? ModifierId { get; protected set; }
        
        public DateTime? ModificationDateTime { get; protected set; }

        protected AuditableEntity(int id) : base(id) { }
        
        protected AuditableEntity() { }
    }
}

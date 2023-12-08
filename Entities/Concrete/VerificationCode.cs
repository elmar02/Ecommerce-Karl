using Core.Entities;
using Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class VerificationCode : BaseEntity, IEntity
    {
        public int Code { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
        public int AttempCount { get; set; }
        public Guid LinkId { get; set; } =  Guid.NewGuid();
        public DateTime ExpiredDate { get; set; } = DateTime.Now.AddMinutes(30);
    }
}

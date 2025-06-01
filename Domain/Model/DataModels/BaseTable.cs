using System;

namespace Domain.Model.DataModels
{
    public class BaseTable
    {
        public Guid Id { get; set; }
        public Guid ReferenceCode { get; set; }
        public string Email { get; set; }
    }
}

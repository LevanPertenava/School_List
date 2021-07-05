using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Models
{
    public class Student
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public int PersonalId { get; set; }

        public byte Age { get; set; }

        public Gender Gender { get; set; }

        public bool IsActive { get; set; }
    }
}

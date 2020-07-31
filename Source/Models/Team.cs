using System;
using System.Collections.Generic;
using System.Text;

namespace Source.Models
{
    class Team
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string MainShirtColor { get; set; }
        public string SecundaryShirtColor { get; set; }
        public long? IdCaptain { get; set; }

        public Team()
        {
        }

        public Team(long id, string name, DateTime birthDate, string mainShirtColor, 
            string secundaryShirtColor)
        {
            this.Id = id;
            this.Name = name;
            this.BirthDate = birthDate;
            this.MainShirtColor = mainShirtColor;
            this.SecundaryShirtColor = secundaryShirtColor;
        }

    }
}

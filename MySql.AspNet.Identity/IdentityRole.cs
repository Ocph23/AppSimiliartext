using System;
using Microsoft.AspNet.Identity;

namespace MySql.AspNet.Identity
{
    public class IdentityRole : IRole
    {
        public IdentityRole()
        {
            Id = Guid.NewGuid().ToString();
        }

        public IdentityRole(string name)
            : this()
        {
            Name = name;
        }

        public IdentityRole(string name, string id)
        {
            Name = name;
            Id = id;
        }

        public IdentityRole(string name, string id, int airportId)
        {
            Name = name;
            Id = id;
            this.AirportId = airportId;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public int AirportId { get; set; }
    }
}

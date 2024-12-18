namespace FleetManagement.Domain.Entities
{
    public class Color: Entity
    {
        public string Name { get; set; }
        
        public IList<Vehicle> Vehicles { get; set; }

        public Color(int id, string name)
        {
            Name = name;
            Id = id;
        }

        public Color(string name)
        {
            Name=name;
        }
    }
}

namespace FleetManagement.Domain.Entities
{
    public class VehicleType: Entity
    {
        public string Name { get; set; }
        public int PassengersCapacity { get; set; }
        
        public List<Vehicle> Vehicles { get; set; }

        public VehicleType(int id , string name, int passengersCapacity)
        {
            Id = id;
            Name = name;
            PassengersCapacity = passengersCapacity;    
        }

        public VehicleType(string name, int passengersCapacity)
        {
            Name = name;
            PassengersCapacity = passengersCapacity;
        }
    }
}

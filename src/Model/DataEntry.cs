namespace GridViewDynamicColumns.Model
{
    public class DataEntry
    {
        public int Id { get; set; }

        public string Name { get; set; }

        // animal
        public int PopulationSize { get; set; }

        public int MaxAge { get; set; }


        // vehicle
        public int MaxSpeed { get; set; }

        public int EngineHorsePower { get; set; }

        public int ManufacturedYear { get; set; }
    }
}
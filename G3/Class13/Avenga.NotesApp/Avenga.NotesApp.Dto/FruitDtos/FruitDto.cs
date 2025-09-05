namespace Avenga.NotesApp.Dto.FruitDtos
{
    public class FruitDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Genus { get; set; }
        public string family { get; set; }
        public string Order { get; set; }
        public NutritionDto? Nutritions { get; set; }
    }
}

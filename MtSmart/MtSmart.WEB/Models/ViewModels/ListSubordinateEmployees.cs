namespace MtSmart.WEB.Models.ViewModels
{
    public class ListSubordinateEmployees
    {
        public List<RootEmployee> rootEmployees = new();

        public class NestedEmployee
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }

        public class RootEmployee
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;

            public List<NestedEmployee> nestedEmployees = new();
        }
    }
}

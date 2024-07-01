namespace CollegeApp.Models
{
    public static class CollegeRepository
    {
        public static List<Student> Students { get; set; } = new List<Student>()
        {
            new Student
                {
                    Id = 1,
                    StudentName = "Alex",
                    Email = "alex@gmail.com",
                    Address = "Nottingham, UK"

                },
                new Student
                {
                    Id = 2,
                    StudentName = "Sian",
                    Email = "sian@gmail.com",
                    Address = "Nottingham, UK"

                }
        };
    }
}

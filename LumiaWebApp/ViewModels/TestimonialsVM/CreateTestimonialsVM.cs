using LumiaWebApp.Models;

namespace LumiaWebApp.ViewModels.TestimonialsVM
{
    public class CreateTestimonialsVM
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Bio { get; set; } = null!;
        public int JobId { get; set; }
        public List<Job>? Jobs { get; set; }
        public IFormFile ProfileImage { get; set; } = null!;
    }
}

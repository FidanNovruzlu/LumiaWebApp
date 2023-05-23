using LumiaWebApp.Models;

namespace LumiaWebApp.ViewModels.TestimonialsVM
{
    public class UpdatetestimonialsVM
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Bio { get; set; }
        public int JobId { get; set; }
        public List<Job>? Jobs { get; set; }
        public IFormFile ProfileImage { get; set; } = null!;
        public string? ProfileImageNmae { get; set; }
    }
}

namespace LumiaWebApp.Models
{
    public class Job
    {
        public Job()
        {
            Testimonials = new List<Testimonials>();
        }
        public int  Id { get; set; }
        public string Name { get; set; } = null!;
        public List<Testimonials> Testimonials { get; set; }
    }
}

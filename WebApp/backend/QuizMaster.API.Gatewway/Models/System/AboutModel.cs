using System.ComponentModel.DataAnnotations;

namespace QuizMaster.API.Gateway.Models.System
{
    public class AboutModel
    {
        public string Version { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string Web_link { get; set; } = string.Empty;
        public string Mobile_link { get; set; } = string.Empty;
        public string Ios_link { get; set; } = string.Empty;
    }

    public class SystemAbout : AboutModel
    {
        [Key]
        public int Id { get; set; } = 0;

        public static SystemAbout DEFAULT => new() { Id = 1, Description = "Lorem ipsum dolor sit amet consectetur. Pulvinar porta egestas molestie purus faucibus neque malesuada lectus. Lacus auctor sit felis sed ultrices nullam sapien ornare justo. Proin adipiscing viverra vestibulum arcu sit. Suscipit bibendum ullamcorper ut et dolor quisque nulla et.", Version = "1.0.0" };
    }
}

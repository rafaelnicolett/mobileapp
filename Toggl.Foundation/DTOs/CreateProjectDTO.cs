namespace Toggl.Foundation.DTOs
{
    public sealed class CreateProjectDTO
    {
        public string Name { get; set; }

        public string Color { get; set; }

        public bool IsPrivate { get; set; }

        public long? ClientId { get; set; }

        public long? TemplateId { get; set; }

        public long WorkspaceId { get; set; }
    }
}

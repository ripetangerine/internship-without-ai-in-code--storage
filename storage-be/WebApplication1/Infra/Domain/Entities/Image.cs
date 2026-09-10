namespace Infra.Domain.Entities
{
    public class Image
    {
        public int id { get; set; }
        public string FileName { get; set; }
        public string FileURL { get; set; }
        public string Ext { set; get; }
        public DateTime CreatedAt { get; set;  }
    }
}

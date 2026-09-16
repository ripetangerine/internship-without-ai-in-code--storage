namespace Main.DTO.Upload
{
    public class UploadRequest
    {
        public IFormFile? ProfileImage { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

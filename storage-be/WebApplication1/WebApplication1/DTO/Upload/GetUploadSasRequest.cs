namespace Main.DTO.Upload
{
    public class GetUploadSasRequest
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }

        public static implicit operator GetUploadSasRequest(GetUploadSasResponse v)
        {
            throw new NotImplementedException();
        }
    }
}

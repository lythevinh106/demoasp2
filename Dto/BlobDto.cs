namespace demoAsp2.Dto
{


    public class BlobRequest
    {

        public IFormFile? file { get; set; }


    }

    public class BlobObject
    {

        public string? ContentType { get; set; }
        public Stream? Content { get; set; }


    }

    public class BlobContent
    {

        public string? FileName { get; set; }
        public string? FilePath { get; set; }


    }


    public class BlobDto
    {
        public Stream? Content { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }


    }


}

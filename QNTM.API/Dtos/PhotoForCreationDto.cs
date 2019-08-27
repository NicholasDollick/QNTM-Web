using Microsoft.AspNetCore.Http;

namespace QNTM.API.Dtos
{
    public class PhotoForCreationDto
    {
        public string Url { get; set; }
        public IFormFile File { get; set; }
        public string Desc { get; set; }
        public string PublicId { get; set; }
    }
}
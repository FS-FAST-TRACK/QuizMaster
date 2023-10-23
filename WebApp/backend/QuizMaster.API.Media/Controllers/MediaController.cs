using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using QuizMaster.API.Media.Services;
using QuizMaster.API.Media.Utility;

namespace QuizMaster.API.Media.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly IFileRepository fileRepository;
        private readonly IWebHostEnvironment webHostEnvironment;

        // dependency inject the properties
        public MediaController(IFileRepository fileRepository, IWebHostEnvironment webHostEnvironment)
        {
            this.fileRepository = fileRepository;
            this.webHostEnvironment = webHostEnvironment;
        }

        // Post new File
        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] IFormFile File)
        {
            // if the form is null, return bad request
            if (File == null) 
                return BadRequest(new { Message = "Form Data should not be null." });

            if (File.FileName == null)
                return BadRequest(new { Message = "Data should be included in the form." });

            // Save the file from the IFormFile
            var FileInformation = await FileHandler.SaveFile(webHostEnvironment, File);

            if (FileInformation == null)
                return BadRequest(new { Message = "Failed to upload file, server error." });

            // Save the FileInformation in the database
            fileRepository.Save(FileInformation);
            
            return Ok(new { Message = "File Uploaded.", FileInformation});
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(new { Files = fileRepository.GetAll() });
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetSpecific(string Id)
        {
            // Parse the string Id to GUID, return bad request if could not parse
            if(!Guid.TryParse(Id, out var id)) 
                return BadRequest(new { Message = "Id specified is invalid format." });

            // Retrieve the file info based on the parsed Guid
            var fileInfo = fileRepository.GetFile(id);

            // Return NotFound if fileInfo is null
            if (fileInfo == null) 
                return NotFound(new { Message = "Could not find file based on the 'id' specified." });

            // Return the FileInfo
            return Ok(new { File = fileInfo });
        }

        [HttpGet]
        [Route("download/{id}")]
        public async Task<IActionResult> DownloadImage(string Id)
        {
            // Parse the string Id to GUID, return bad request if could not parse
            if (!Guid.TryParse(Id, out var id))
                return BadRequest(new { Message = "Id specified is invalid format." });

            // Retrieve the file info based on the parsed Guid
            var fileInfo = fileRepository.GetFile(id);

            // Return NotFound if fileInfo is null
            if (fileInfo == null)
                return NotFound(new { Message = "Could not find file based on the 'id' specified." });

            // get the image path
            var imagePath = Path.Combine(webHostEnvironment.WebRootPath, "Images", $"{fileInfo.Id}{fileInfo.Type}");

            // prepare the provider for the file send response
            var provider = new FileExtensionContentTypeProvider();
            if(!provider.TryGetContentType(imagePath, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            // get the bytes
            var bytes = await System.IO.File.ReadAllBytesAsync(imagePath);

            // send the file to the client
            return File(bytes, contentType, $"{fileInfo.Name}{fileInfo.Type}");
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteFile(string Id)
        {
            // Parse the string Id to GUID, return bad request if could not parse
            if (!Guid.TryParse(Id, out var id))
                return BadRequest(new { Message = "Id specified is invalid format." });

            // Retrieve the file info based on the parsed Guid
            var fileInfo = fileRepository.GetFile(id);

            // Return NotFound if fileInfo is null
            if (fileInfo == null)
                return NotFound(new { Message = "Could not find file based on the 'id' specified." });

            // delete the file first
            var fileDeleted = FileHandler.DeleteFile(webHostEnvironment, fileInfo);

            if (!fileDeleted)
                return BadRequest(new { Message = "Failed to delete file, might not exist in the server.", fileInfo });

            // delete the file information in the database
            fileRepository.Remove(id);

            return Ok(new { Message = "File Deleted." });
        }
    }
}

using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QuizMaster.API.Gateway.Configuration;
using QuizMaster.API.Gateway.Helper;
using QuizMaster.API.Media.Models;
using QuizMaster.API.Media.Proto;
using QuizMaster.API.Media.Utility;

namespace QuizMaster.API.Gateway.Controllers
{
    [ApiController]
    [Route("gateway/api/media")]
    public class MediaGatewayController : Controller
    {
        private readonly GrpcChannel _channel;
        private readonly MediaService.MediaServiceClient _channelClient;
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MediaGatewayController(ILogger<MediaGatewayController> logger, IWebHostEnvironment webHostEnvironment, IOptions<GrpcServerConfiguration> options)
        {
            _channel = GrpcChannel.ForAddress(options.Value.Media_Service);
            _channelClient = new MediaService.MediaServiceClient(_channel);
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Upload media
        /// </summary>
        /// <param name="File"></param>
        /// <returns>Task<IActionResult></returns>
        [QuizMasterAuthorization]
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile File)
        {
            if (File == null)
                return BadRequest(new { Message = "Form Data should not be null." });

            if (File.FileName == null)
                return BadRequest(new { Message = "Data should be included in the form." });

            // Save media   
            var FileInformation = await FileHandler.SaveFile(_webHostEnvironment, File);

            if (FileInformation == null)
                return BadRequest(new { Message = "Failed to upload file, server error." });

            // Create request to save media info
            var request = new UploadMediaRequest
            {
                Media = JsonConvert.SerializeObject(FileInformation)
            };

            try
            {
                // Call service to save media info
                var response = await _channelClient.UploadMediaAsync(request);

                // If failed save
                if (response.StatusCode == 500)
                {
                    return BadRequest(new { Message = "Failed to upload file, server error." });
                }

                return Ok(new { Message = "File Uploaded", FileInformation });
            }
            catch(Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        /// <summary>
        /// Get all media
        /// </summary>
        /// <returns>Task<IActionResult></returns>
        [QuizMasterAuthorization]
        [HttpGet("get_all_media")]
        public async Task<IActionResult> GetAllMedia()
        {
            // call service to get all media
            var response = _channelClient.GetAllMedia(new Media.Proto.Empty());

            var medias  = new List<FileInformation>();

            // loop through the response stream
            while(await response.ResponseStream.MoveNext())
            {
                // deserialize the median and add to list
                medias.Add(JsonConvert.DeserializeObject<FileInformation>(response.ResponseStream.Current.Media));
            };

            return Ok(medias);
        }

        /// <summary>
        /// Get media by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IActionResult</returns>
        [QuizMasterAuthorization]
        [HttpGet("get_media/{id}")]
        public IActionResult GetMedia(string id)
        {
            // check if id is valid guid
            if (!Guid.TryParse(id, out var Id))
                return BadRequest(new { Message = "Id specified is invalid format." });

            // create request to get media
            var request = new GetMediaRequest() { Id = id };

            // call service if media exist
            var response = _channelClient.GetMedia(request);

            // Return NotFound if fileInfo is null
            if (response.StatusCode == 404)
                return NotFound(new { Message = "Could not find file based on the 'id' specified." });

            var file = JsonConvert.DeserializeObject<FileInformation>(response.Media);
            // Return the FileInfo
            return Ok(new { File =  file});
        }

        /// <summary>
        /// Download media
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Task<IActionResult></returns>
        [QuizMasterAuthorization]
        [HttpGet("download_media/{id}")]
        public async Task<IActionResult> DownloadMedia(string id)
        {
            // check of id is valid guid
            if (!Guid.TryParse(id, out var Id))
                return BadRequest(new { Message = "Id specified is invalid format." });

            // create request to get media
            var request = new GetMediaRequest() { Id = id };

            // call service if media exist
            var response = _channelClient.GetMedia(request);

            // Return NotFound if fileInfo is null
            if (response.StatusCode == 404)
                return NotFound(new { Message = "Could not find file based on the 'id' specified." });

            // deserialize the media
            var file = JsonConvert.DeserializeObject<FileInformation>(response.Media);

            // get the image path
            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", $"{file.Id}{file.Type}");

            // prepare the provider for the file send response
            var provider = new FileExtensionContentTypeProvider();
            if(!provider.TryGetContentType(imagePath, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            try
            {
                // download file
                var bytes = await System.IO.File.ReadAllBytesAsync(imagePath);
                return File(bytes, contentType, $"{file.Name}{file.Type}");
            }
            catch(FileNotFoundException ex)
            {
                // delete file info if media does not exist
                _logger.LogError(ex.Message);
                DeleteMedia(id);

                return NotFound(new { Message = "Could not find file based on the 'id' specified." });
            }
            catch(Exception ex)
            {
                // if failed download
                return BadRequest(new { Message = "Failed to download media" });
            }
            
        }

        /// <summary>
        /// Delete media
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IActionResult</returns>
        [QuizMasterAuthorization]
        [HttpDelete("delete_media/{id}")]
        public IActionResult DeleteMedia(string id)
        {
            // check of id is valid guid
            if (!Guid.TryParse(id, out var Id))
                return BadRequest(new { Message = "Id specified is invalid format." });

            // create request to get media
            var request = new GetMediaRequest() { Id = id };

            // call service to delete media
            var response = _channelClient.DeleteMedia(request);

            // if media not found
            if(response.StatusCode == 404)
            {
                return NotFound(new { Message = "Could not find file based on the 'id' specified." });
            }

            // deserialize media
            var file = JsonConvert.DeserializeObject<FileInformation>(response.Media);

            // delete media file
            var fileDeleted = FileHandler.DeleteFile(_webHostEnvironment, file);
            
            // if unsuccessful delete
            if (!fileDeleted)
            {
                return BadRequest(new { Message = "Failed to delete file, server error." });
            }

            return Ok(new { Message = "File Deleted." });
                
        }

    }
}

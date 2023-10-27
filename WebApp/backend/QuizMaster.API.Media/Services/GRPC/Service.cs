using Grpc.Core;
using Newtonsoft.Json;
using QuizMaster.API.Media.Models;
using QuizMaster.API.Media.Proto;

namespace QuizMaster.API.Media.Services.GRPC
{
    public class Service : MediaService.MediaServiceBase
    {
        private readonly IFileRepository _fileRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger _logger;

        public Service(IFileRepository fileRepository, IWebHostEnvironment webHostEnvironment, ILogger<Service> logger)
        {
            _fileRepository = fileRepository;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        /// <summary>
        /// Upload media
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns>Task<UploadMediaResponse></returns>
        public override async Task<UploadMediaResponse> UploadMedia(UploadMediaRequest request, ServerCallContext context)
        {
            var reply = new UploadMediaResponse();
            // deserialize the request
            var fileInformation = JsonConvert.DeserializeObject<FileInformation>(request.Media);

            try
            {
                // save media
                _fileRepository.Save(fileInformation);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                reply.StatusCode = 500;
                return await Task.FromResult(reply);
            }
            

            reply.StatusCode = 200;
            return await Task.FromResult(reply);
        }

        /// <summary>
        /// Get all media
        /// </summary>
        /// <param name="request"></param>
        /// <param name="responseStream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task GetAllMedia(Empty request, IServerStreamWriter<GetAllMediaResponse> responseStream, ServerCallContext context)
        {
            var reply = new GetAllMediaResponse();
            // get all the media
            foreach (var media in _fileRepository.GetAll())
            {
                // serialize the media
                reply.Media = JsonConvert.SerializeObject(media);
                // write the media to the stream
                await responseStream.WriteAsync(reply);
            }
        }

        /// <summary>
        /// Get media by id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns>Task<GetMediaReply></returns>
        public override Task<GetMediaReply> GetMedia(GetMediaRequest request, ServerCallContext context)
        {
            var reply = new GetMediaReply();
            // get the media
            var fileInformation = _fileRepository.GetFile(Guid.Parse(request.Id));

            // if does not exist
            if (fileInformation == null)
            {
                reply.StatusCode = 404;
                return Task.FromResult(reply);
            }

            reply.StatusCode = 200;
            reply.Media = JsonConvert.SerializeObject(fileInformation);
            return Task.FromResult(reply);
        }

        /// <summary>
        /// Delete media
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns>Task<GetMediaReply></returns>
        public override Task<GetMediaReply> DeleteMedia(GetMediaRequest request, ServerCallContext context)
        {
            var reply = new GetMediaReply();
            // get the media
            var fileInformation = _fileRepository.GetFile(Guid.Parse(request.Id));

            // if does not exist
            if (fileInformation == null)
            {
                reply.StatusCode = 404;
                return Task.FromResult(reply);
            }

            try
            {
                // delete the media information
                _fileRepository.Remove(Guid.Parse(request.Id));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                reply.StatusCode = 500;
                return Task.FromResult(reply);
            }
            reply.StatusCode = 200;
            // serialize the media
            reply.Media = JsonConvert.SerializeObject(fileInformation);
            return Task.FromResult(reply);
        }
    }
}

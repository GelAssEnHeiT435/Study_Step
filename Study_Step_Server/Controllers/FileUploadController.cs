using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Study_Step_Server.Interfaces;
using Study_Step_Server.Models;
using Study_Step_Server.Models.DTO;
using Study_Step_Server.Services;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Study_Step_Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileUploadController : Controller
    {
        private readonly IUoW _unitOfWork;
        private readonly DtoConverterService _dtoConverter;
        private readonly IFileService _fileService;
        private readonly IConfiguration _config;

        public FileUploadController( IUoW unitOfWork,
                                     DtoConverterService converter,
                                     IFileService fileService,
                                     IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _dtoConverter = converter;
            _fileService = fileService;
            _config = config;
        }

        [HttpPost("upload")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadFile()
        {
            // read info from headers
            var fileName = WebUtility.UrlDecode(Request.Headers["X-FileName"]);
            var fileSize = long.Parse(Request.Headers["X-FileSize"]);
            string? savePath = _config["Paths:MediaPath"];
            var tempFilePath = Path.Combine(savePath, Guid.NewGuid() + ".tmp");

            try
            {
                // reading file in parts
                await using (var tempFileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
                {
                    await Request.Body.CopyToAsync(tempFileStream);
                }

                var finalPath = Path.Combine(savePath, $"{Guid.NewGuid()}_{fileName}");
                System.IO.File.Move(tempFilePath, finalPath); // replace .tmp to our file

                Console.WriteLine(finalPath);

                return Ok(new { Path = finalPath, Size = fileSize });
            }
            finally
            {
                // delete .tmp in case of error
                if (System.IO.File.Exists(tempFilePath)) System.IO.File.Delete(tempFilePath);
            }
        }

        [HttpGet("download")]
        public async Task<IActionResult> GetFile(int Id) 
        {
            var file = await _unitOfWork.Files.GetByIdAsync(Id); // get file's info from db
            if (!System.IO.File.Exists(file.Path))
            {
                return NotFound("File not found.");
            }
            var fileStream = System.IO.File.OpenRead(file.Path);

            return new FileStreamResult(fileStream, "application/octet-stream")
            {
                FileDownloadName = Uri.EscapeDataString(file.Name) // Кодируем имя файла
            };
        }
    }

}

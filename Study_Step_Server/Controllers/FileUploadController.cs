using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Study_Step_Server.Interfaces;
using Study_Step_Server.Models;
using Study_Step_Server.Models.DTO;
using Study_Step_Server.Services;
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

        public FileUploadController( IUoW unitOfWork,
                                     DtoConverterService converter,
                                     IFileService fileService )
        {
            _unitOfWork = unitOfWork;
            _dtoConverter = converter;
            _fileService = fileService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] List<IFormFile> files, [FromForm] List<string> fileModels)
        {
            if (files == null || files.Count == 0 || fileModels == null || fileModels.Count == 0)
            {
                return BadRequest("No files or metadata provided.");
            }

            if (files.Count != fileModels.Count)
            {
                return BadRequest("The number of files and metadata entries must match.");
            }

            List<FileModel> uploadedFiles = new List<FileModel>();

            for (int i = 0; i < files.Count; i++)
            {
                var file = files[i];
                var fileModelJson = fileModels[i];

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine("D:/my_works/dotnetProjects/Study_Step/Study_Step_Server/Media/", fileName);

                // Сохраняем файл на сервере
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var fileModel = System.Text.Json.JsonSerializer.Deserialize<FileModel>(fileModelJson);
                uploadedFiles.Add(fileModel);
            }

            return Ok(uploadedFiles);
        }

        [HttpGet("download")]
        public async Task<IActionResult> GetFile(int Id) 
        {
            var file = await _unitOfWork.Files.GetByIdAsync(Id);
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

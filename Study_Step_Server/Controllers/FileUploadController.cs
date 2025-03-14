using Microsoft.AspNetCore.Mvc;
using Study_Step_Server.Interfaces;
using Study_Step_Server.Models.DTO;
using Study_Step_Server.Services;

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
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)  { return BadRequest("Файл не был загружен."); }

            // Сохраняем файл на сервере
            var filePath = Path.Combine("Uploads", file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Возвращаем информацию о файле
            var fileInfo = new
            {
                Name = file.FileName,
                Size = file.Length,
                MimeType = file.ContentType,
                Path = filePath
            };

            // Уведомляем всех клиентов через SignalR
            //await _hubContext.Clients.All.SendAsync("ReceiveFile", fileInfo);

            return Ok(fileInfo);
        }

        [HttpGet("download")]
        public async Task<FileModelDTO?> GetFile(int Id) 
        {
            var file = await _unitOfWork.Files.GetByIdAsync(Id);
            return _dtoConverter.GetFileDTO(file);
        }
    }

}

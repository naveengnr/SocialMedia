using System;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using EFExample.Photos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace EFExample.Photos
{
    [Route("api/images")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly Cloudinary _cloudinary;

        public ImageController(IOptions<CloudinarySettings> cloudinarySettings)
        {
            Account Account = new Account(
                cloudinarySettings.Value.CloudName,
                cloudinarySettings.Value.ApiKey,
                cloudinarySettings.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(Account);
        }

        [HttpPost("UploadProfilePic/{userId}")]
        public IActionResult UploadImage(int userId, [FromForm] ImageUpload model)
        {
            if (model.File == null || model.File.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var publicId = $"user_{userId}_profile_pic";

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(model.File.FileName, model.File.OpenReadStream()),
                Folder = "uploads",
                PublicId = publicId
            };

            try
            {
                var uploadResult = _cloudinary.Upload(uploadParams);

                return Ok(uploadResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        //[HttpGet("{publicId}")]
        //public IActionResult GetImage(string publicId)
        //{
        //    var imageUrl = _cloudinary.Api.UrlImgUp
        //        .Transform(new Transformation().Width(300).Height(200).Crop("fill"))
        //        .BuildUrl($"{publicId}.jpg");

        //    return Ok(new { ImageUrl = imageUrl });
        //}

        [HttpGet("GetProfilePic/{userId}")]
        public IActionResult GetProfilePicture(int userId)
        {
            var Folder = "uploads";
            var publicId = $"{Folder}/user_{userId}_profile_pic";

            var imageUrl = _cloudinary.Api.UrlImgUp

                .Transform(new Transformation().Width(300).Height(200).Crop("fill"))

                .BuildUrl(publicId);

            return Ok(new { ImageUrl = imageUrl });
        }

        [HttpPut("UpdateProfilePic/{userId}")]
        public IActionResult UpdateProfileImage(int userId, [FromForm] ImageUpload model)
        {
            if (model.File == null || model.File.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            var currentPublicId = $"user_{userId}_profile_pic";

            DeleteImage(currentPublicId);

            var publicId = $"user_{userId}_profile_pic";

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(model.File.FileName, model.File.OpenReadStream()),
                Folder = "uploads",
                PublicId = publicId
            };

            try
            {
                var uploadResult = _cloudinary.Upload(uploadParams);

                return Ok(uploadResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpDelete("DeleteProfilePic/{userId}")]
        public IActionResult DeleteProfileImage(int userId)
        {
            var Folder = "uploads";
            var publicId = $"{Folder}/user_{userId}_profile_pic";

            DeleteImage(publicId);

            return Ok("Profile picture deleted successfully.");
        }

        private void DeleteImage(string publicId)
        {
            if (!string.IsNullOrEmpty(publicId))
            {

                var deletionParams = new DeletionParams(publicId);
                var deletionResult = _cloudinary.Destroy(deletionParams);
            }
        }

    }
}
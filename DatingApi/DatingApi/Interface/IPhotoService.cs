﻿using CloudinaryDotNet.Actions;

namespace DatingApi.Interface
{
    public interface IPhotoService
    {
        Task<ImageUploadResult>AddPhotoAsync(IFormFile file);
        Task<DeletionResult>DeletePhotoAsync(string publicId);
    }
}

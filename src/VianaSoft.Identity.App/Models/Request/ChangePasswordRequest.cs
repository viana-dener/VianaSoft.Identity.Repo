﻿namespace VianaSoft.Identity.App.Models.Request
{
    public class ChangePasswordRequest
    {
        public string? Email { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? ConfirmPassword { get; set; }
    }
}

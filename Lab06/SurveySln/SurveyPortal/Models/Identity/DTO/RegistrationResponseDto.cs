namespace SurveyPortal.Models.Identity.DTO
{
    public class RegistrationResponseDto
    {
        public bool isSuccesRegistration { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}

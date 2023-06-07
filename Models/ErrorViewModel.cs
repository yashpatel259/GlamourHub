namespace GlamourHub.Models
{
    public class ErrorViewModel
    {
        /// <summary>
        /// This is for first Sync
        /// </summary>
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EventStoreLearning.Appointment.CommandApi.Contract
{
    /// <summary>
    /// Represents a request to change an existing appointment
    /// </summary>
    public class ChangeAppointmentRequest
    {
        /// <summary>
        /// The ID of the appointment to change
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// The current version of the appointment
        /// </summary>
        [Required]
        public long Version { get; set; }

        /// <summary>
        /// The title of the appointment
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// When the appointment begins
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// How long the appointment will be
        /// </summary>
        public int? DurationMinutes { get; set; }
    }
}

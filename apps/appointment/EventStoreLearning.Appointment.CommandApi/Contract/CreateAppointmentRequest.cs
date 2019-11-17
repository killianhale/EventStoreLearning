using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EventStoreLearning.Appointment.CommandApi.Contract
{
    /// <summary>
    /// Represents a request to create a new appointment
    /// </summary>
    public class CreateAppointmentRequest
    {
        /// <summary>
        /// The title of the appointment
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// When the appointment begins
        /// </summary>
        [Required]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// How long the appointment will be
        /// </summary>
        [Required]
        [DefaultValue(30)]
        public int DurationMinutes { get; set; }
    }
}

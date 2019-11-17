using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EventStoreLearning.Appointment.QueryApi.Contract
{
    /// <summary>
    /// Represents an appointment
    /// </summary>
    public class Appointment
    {
        /// <summary>
        /// The unique ID of the appointment
        /// </summary>
        [Required]
        public Guid Id { get; set; }

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

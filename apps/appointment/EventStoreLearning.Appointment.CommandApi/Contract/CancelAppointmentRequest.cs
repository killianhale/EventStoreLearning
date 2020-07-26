using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EventStoreLearning.Appointment.CommandApi.Contract
{
    /// <summary>
    /// Represents a request to cancel an existing appointment
    /// </summary>
    public class CancelAppointmentRequest
    {
        /// <summary>
        /// The ID of the appointment to cancel
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// The current version of the appointment
        /// </summary>
        [Required]
        public long Version { get; set; }
    }
}

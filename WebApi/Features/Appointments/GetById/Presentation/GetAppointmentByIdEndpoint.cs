using Microsoft.AspNetCore.Mvc;
using WebApi.Features.Appointments.GetById.Contracts;
using WebApi.Features.Appointments.GetById.Data;

namespace WebApi.Features.Appointments.GetById.Presentation;

[ApiController]
[Route("api")]
public class GetAppointmentByIdEndpoint : ControllerBase
{
    private readonly IGetIngestedAppointmentByIdDataService _getIngestedAppointmentByIdDataService;

    public GetAppointmentByIdEndpoint(IGetIngestedAppointmentByIdDataService getIngestedAppointmentByIdDataService)
    {
        _getIngestedAppointmentByIdDataService = getIngestedAppointmentByIdDataService;
    }

    [HttpGet("appointments/{id:int}")]
    public async Task<ActionResult<IngestedAppointment>> GetAppointmentById(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var appointment = await _getIngestedAppointmentByIdDataService.Get(id, cancellationToken);
        
        if(appointment == null)
        {
            return NotFound("Appointment");
        }
        
        return Ok(appointment);
    }
}

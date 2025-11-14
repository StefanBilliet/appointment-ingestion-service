using Vogen;
using WebApi.Endpoints.IngestAppointment;

namespace Tests.Unit.WebApi.Endpoints.IngestAppointment;

public class ServiceDurationTests
{
    [Fact]
    public void Negative_service_duration_throws()
    {
        Assert.Throws<ValueObjectValidationException>(() => ServiceDuration.From(-1));
    }
    
    [Fact]
    public void Zero_service_duration_throws()
    {
        Assert.Throws<ValueObjectValidationException>(() => ServiceDuration.From(0));
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    public void Positive_service_duration_does_not_throw(int serviceDurationInMinutes)
    {
        _ = ServiceDuration.From(serviceDurationInMinutes);
    }
}
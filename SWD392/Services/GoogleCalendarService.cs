using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

public class GoogleCalendarService
{
    private readonly CalendarService _calendarService;

    public GoogleCalendarService(IConfiguration configuration)
    {
        string credentialPath = configuration["GoogleCalendar:CredentialPath"];
        string fullPath = Path.Combine(Directory.GetCurrentDirectory(), credentialPath);

        GoogleCredential credential;
        using (var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
        {
            credential = GoogleCredential.FromStream(stream)
                .CreateScoped(CalendarService.Scope.Calendar);
        }

        _calendarService = new CalendarService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "Doctor Booking App"
        });
    }

    public async Task<string> CreateGoogleMeetEventAsync(string doctorEmail, string customerEmail, string timeSlot)
    {
        var startTime = DateTime.Parse(timeSlot);
        var endTime = startTime.AddHours(1);

        Event newEvent = new Event()
        {
            Summary = "Lịch khám với bác sĩ",
            Description = "Cuộc hẹn khám bệnh trực tuyến",
            Start = new EventDateTime() { DateTime = startTime, TimeZone = "Asia/Ho_Chi_Minh" },
            End = new EventDateTime() { DateTime = endTime, TimeZone = "Asia/Ho_Chi_Minh" },
            Attendees = new List<EventAttendee>()
            {
                new EventAttendee() { Email = doctorEmail },
                new EventAttendee() { Email = customerEmail }
            },
            ConferenceData = new ConferenceData()
            {
                CreateRequest = new CreateConferenceRequest()
                {
                    RequestId = Guid.NewGuid().ToString(),
                    ConferenceSolutionKey = new ConferenceSolutionKey()
                    {
                        Type = "hangoutsMeet"
                    }
                }
            }
        };

        var request = _calendarService.Events.Insert(newEvent, "primary");
        request.ConferenceDataVersion = 1;
        var createdEvent = await request.ExecuteAsync();

        return createdEvent.HangoutLink;
    }
}

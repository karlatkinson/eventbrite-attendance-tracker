namespace eventbrite.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Runtime.Serialization;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    [Route("api/[controller]")]
    public class EventbriteController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<EventbriteController> _logger;

        public EventbriteController(IHttpClientFactory httpClientFactory, ILogger<EventbriteController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [HttpGet("[action]")]
        public async Task<List<EventStatus>> EventStatus()
        {
            var httpClient = _httpClientFactory.CreateClient("eventbrite");

            var events = await httpClient.GetAsync("/v3/organizations/104810307145/events/");
            events.EnsureSuccessStatusCode();
            var eventData = await events.Content.ReadAsAsync<EventData>();

           
            var eventStatuses = new List<EventStatus>();
            foreach (var item in eventData.Events)
            {
                var attendees = await httpClient.GetAsync($"/v3/events/{item.Id}/attendees/");
                attendees.EnsureSuccessStatusCode();
                var eventAttendeeData = await attendees.Content.ReadAsAsync<EventAttendeeData>();
                var eventStatus = new EventStatus
                {
                    EventId = "74714941401",
                    EventName = item.Name.Text,
                    EventDate = item.Start.Local,
                    PossibleAttendance = eventAttendeeData.Attendees.Count,
                    CurrentAttendance = eventAttendeeData.Attendees.Count(x => x.CheckedIn)
                };
                eventStatuses.Add(eventStatus);
            }

            return eventStatuses.OrderBy(x=>x.EventDate).ToList();
        }
    }

    public class EventStatus
    {
        public string EventId { get; set; }
        public string EventName { get; set; }
        public int PossibleAttendance { get; set; }
        public int CurrentAttendance { get; set; }
        public DateTime EventDate { get; set; }
    }

    public partial class EventAttendeeData
    {
        public Pagination Pagination { get; set; }
        public List<Attendee> Attendees { get; set; }
    }

    public partial class Attendee
    {
        public object Team { get; set; }
        public long Id { get; set; }
        public DateTimeOffset Changed { get; set; }
        public DateTimeOffset Created { get; set; }
        public long Quantity { get; set; }
        public object VariantId { get; set; }
        public Profile Profile { get; set; }
        public List<Barcode> Barcodes { get; set; }

        [JsonProperty("checked_in")]
        public bool CheckedIn { get; set; }
        public bool Cancelled { get; set; }
        public bool Refunded { get; set; }
        public string Affiliate { get; set; }
        public object GuestlistId { get; set; }
        public object InvitedBy { get; set; }
        public string Status { get; set; }
        public string TicketClassName { get; set; }
        public string DeliveryMethod { get; set; }
        public string EventId { get; set; }
        public long OrderId { get; set; }
        public long TicketClassId { get; set; }
    }

    public partial class Barcode
    {
        public string Status { get; set; }
        public string BarcodeBarcode { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Changed { get; set; }
        public long CheckinType { get; set; }
        public string CheckinMethod { get; set; }
        public bool IsPrinted { get; set; }
    }

    public partial class Profile
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public Addresses Addresses { get; set; }
    }

    public partial class Addresses
    {
    }

    public partial class Pagination
    {
        public long ObjectCount { get; set; }
        public long PageNumber { get; set; }
        public long PageSize { get; set; }
        public long PageCount { get; set; }
        public bool HasMoreItems { get; set; }
    }

    public class EventData
    {
        public Pagination Pagination { get; set; }
        public List<Event> Events { get; set; }
    }

    public class Event
    {
        public string Id { get; set; }
        public EventName Name { get; set; }
        public EventDateTime Start { get; set; }
    }

    public class EventName {
        public string Text { get; set; }
    }

    public class EventDateTime {
        public DateTime Local { get; set; }
    }
}
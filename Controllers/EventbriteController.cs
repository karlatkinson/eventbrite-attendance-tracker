namespace eventbrite.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Runtime.Serialization;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    [Route("api/[controller]")]
    public class EventbriteController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EventbriteController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("[action]")]
        public async Task<EventStatus> EventStatus()
        {
            var httpClient = _httpClientFactory.CreateClient("eventbrite");

            var attendees = await httpClient.GetAsync("/v3/events/74714941401/attendees/");
            attendees.EnsureSuccessStatusCode();
            var eventAttendeeData = await attendees.Content.ReadAsAsync<EventAttendeeData>();

            return new EventStatus
            {
                EventId = "74714941401",
                PossibleAttendance = eventAttendeeData.Attendees.Count,
                CurrentAttendance = eventAttendeeData.Attendees.Count(x => x.CheckedIn)
            };
        }
    }

    public class EventStatus
    {
        public string EventId { get; set; }
        public int PossibleAttendance { get; set; }
        public int CurrentAttendance { get; set; }
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

}
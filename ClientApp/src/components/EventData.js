import React, { Component } from 'react';

export class EventData extends Component {
  static displayName = EventData.name;

  constructor(props) {
    super(props);
    this.state = { forecasts: [], loading: true };

    fetch('api/Eventbrite/EventStatus')
      .then(response => response.json())
      .then(data => {
        this.setState({ events: data, loading: false });
      });
  }

  static renderEventTable(event) {
    return (
      <table className='table table-striped'>
        <thead>
          <tr>
            <th>EventId</th>
            <th>Event Date</th>
            <th>Event Name</th>
            <th>Possible Attendance</th>
            <th>Current Attendance</th>
          </tr>
        </thead>
        <tbody>
          <tr key={event.eventId}>
            <td>{event.eventId}</td>
            <td>{event.eventDate}</td>
            <td>{event.eventName}</td>
            <td>{event.possibleAttendance}</td>
            <td>{event.currentAttendance}</td>
          </tr>
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : this.state.events.map(event => EventData.renderEventTable(event));

    return (
      <div>
        <h1>Event Data</h1>
        <p>The current event data for the event.</p>
        {contents}
      </div>
    );
  }
}

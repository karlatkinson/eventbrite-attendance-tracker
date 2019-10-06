import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { EventData } from './components/EventData';

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={EventData} />
      </Layout>
    );
  }
}

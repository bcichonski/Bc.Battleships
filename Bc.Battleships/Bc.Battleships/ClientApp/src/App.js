import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import Game from './components/Game';

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Game} />
      </Layout>
    );
  }
}

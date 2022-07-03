import React, { Component } from 'react';
import { Container } from 'reactstrap';
import { NavMenu } from './NavMenu';
import StartNewGame from './StartNewGame';

export class Layout extends Component {
  render () {
    return (
      <div>
        <NavMenu />
        <Container>
          <StartNewGame />
        </Container>
        <Container>
          {this.props.children}
        </Container>
      </div>
    );
  }
}

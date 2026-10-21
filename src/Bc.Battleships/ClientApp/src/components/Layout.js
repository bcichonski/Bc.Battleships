import React, { Component } from 'react';
import { Container } from 'reactstrap';
import { NavMenu } from './NavMenu';

import '../custom.css'

export class Layout extends Component {
  render () {
    return (
      <div>
        <NavMenu />
        <Container className='center'>  
          {this.props.children}
        </Container>
      </div>
    );
  }
}

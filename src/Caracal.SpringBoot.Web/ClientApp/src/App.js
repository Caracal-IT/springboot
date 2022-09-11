import React, {Component} from 'react';
import {Switch} from 'react-router-dom';
import AppRoutes from './AppRoutes';
import { Layout } from './components/Layout';
import './custom.css';
import {ApmRoute} from "@elastic/apm-rum-react";


export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <Layout>
        <Switch>
          {AppRoutes.map((route, index) => {
            const { ...rest } = route;
            return <ApmRoute exact key={index} {...rest} />;
          })}
        </Switch>
      </Layout>
    );
  }
}

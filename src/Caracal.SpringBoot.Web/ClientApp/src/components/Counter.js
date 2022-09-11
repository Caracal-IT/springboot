import React, { Component } from 'react';

export class Counter extends Component {
  static displayName = Counter.name;

  constructor(props) {
    super(props);
    
    const transaction = Window.apm.startTransaction('Counter_load', 'custom', { managed: false });
    const httpSpan = transaction.startSpan('GET ', 'custom', { blocking: true });
    
    this.state = { currentCount: 0 };
    this.incrementCounter = this.incrementCounter.bind(this);

    setTimeout(() => {
      httpSpan.end();
      transaction.end();
    }, 0);
  }

  incrementCounter() {
    this.setState({
      currentCount: this.state.currentCount + 1
    });
  }

  render() {
    return (
      <div>
        <h1>Counter</h1>

        <p>This is a simple example of a React component.</p>

        <p aria-live="polite">Current count: <strong>{this.state.currentCount}</strong></p>

        <button className="btn btn-primary" onClick={this.incrementCounter}>Increment</button>
      </div>
    );
  }
}
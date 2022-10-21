import React, { Component } from 'react';

export class FileValidator extends Component {
  static displayName = FileValidator.name;

  render() {
    return (
      <div>
        <h1>Smart File Validator</h1>
        <p>Welcome to your File Validator</p>
        <p>To help you get started, we have also set up:</p>
        <ul>
          <li><strong>Upload</strong>. Upload your zip file to the app</li>
          <li><strong>Validate</strong>. Validate the file</li>
          <li><strong>Save</strong>. Save the file</li>
        </ul>
       </div>
    );
  }
}

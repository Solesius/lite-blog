import { Component } from '@angular/core';

@Component({
  selector: 'app-not-found',
  template: `<div
    class="container-fluid d-flex align-items-center justify-content-center  h-100"
    style="padding-top: 80px;"
  >
    <div class="row">
      <div class="col-12 text-center">
        <h1 class="display-1 text-danger">404</h1>
        <p class="lead text-muted">Uh-Oh</p>
        <p class="lead text-muted">Looks like that's not here</p>
      </div>
    </div>
  </div>`,
})
export class NotFoundComponent {}

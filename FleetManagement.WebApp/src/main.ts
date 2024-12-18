import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { provideRouter } from '@angular/router';
import { routes } from './app/app.routes';
import { provideHttpClient } from '@angular/common/http';
import { provideToastr, ToastrModule } from 'ngx-toastr';
import { importProvidersFrom } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

bootstrapApplication(AppComponent, {
  providers: [
    provideRouter(routes),
    provideHttpClient(),
    importProvidersFrom(
        BrowserAnimationsModule,
        ToastrModule.forRoot({
          timeOut: 20000,
          positionClass: 'toast-top-center',
          preventDuplicates: true,
        })
      ),
      provideToastr(),

  ],
}).catch(err => console.error(err));

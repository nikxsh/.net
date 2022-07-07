import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MigrationService } from './app.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { PrettyPrintPipe } from './jsonprint.pipe';
import { NgModelformModule } from '@nikxsh/ngmodelform';

@NgModule({
  declarations: [
    AppComponent,
    PrettyPrintPipe
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    NgbModule,
    NgModelformModule 
  ],
  providers: [
    MigrationService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

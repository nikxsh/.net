import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { DbSchemaRequest, DbSchemaResponse } from './app.model';

@Injectable()
export class MigrationService {

    constructor(private http: HttpClient) {
    }

    public fetchSchema(request: DbSchemaRequest): Observable<DbSchemaResponse[]> {
        return this.http.post<any>(`https://localhost:44386/api/sqltomongo/schema`, request)
            .pipe(
                map(response => response.map((res: { Table: any; Columns: string[]; }):DbSchemaResponse => ({
                    table: res.Table,
                    columns: res.Columns,
                    selectedColumns: new Array(res.Columns.length),
                    mainCheck: false,
                    nestedCheck: false,
                    objectName: "",
                    sourceId:"",
                    targetTable: "",
                    targetId: ""
                })))
            );
    }

    
    public fetchSampleDocument(request: any): Observable<any> {
        return this.http.post<any>(`https://localhost:44386/api/sqltomongo/sample`, request)
            .pipe(
                map((response: any) => response)
            );
    }

    public migrate(request: any): Observable<any> {
        return this.http.post(`https://localhost:44386/api/sqltomongo/migrate`, request)
            .pipe(
                map((response: any) => response)
            );
    }
}
import { Component } from '@angular/core';
import { MigrationService } from './app.service';
import { DbSchemaRequest, DbSchemaResponse, NestedTable, Settings, Table, Template, TemplateSettings } from './app.model';
import { FormField } from '@nikxsh/ngmodelform';
import { FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  sqlFormFields: FormField[] = [];
  mongoFormFields: FormField[] = [];
  targetColumns: string[] = []
  dbSchema: DbSchemaResponse[] = []
  sampleDocument: any
  template: Template = new Template();
  migrationResponse: any
  displayTemplate = ""

  constructor(private MigrationServiceRef: MigrationService) {
    this.template.mainTable = new Table({});
    this.template.nestedTables = [];
    this.updateTempate();
  }

  ngOnInit(): void {

    this.sqlFormFields = [
      new FormField({ label: "MSSQL Connection" }).TextField({
        name: "sqlDbUrl",
        control: new FormControl('', [Validators.required])
      }),
      new FormField({ label: "MSSQL Databse Name" }).TextField({
        name: "sqlDbName",
        control: new FormControl('', [Validators.required])
      }),
      new FormField({ label: "Filter" }).TextField({
        name: "filterTable",
        control: new FormControl('')
      })
    ];

    this.mongoFormFields = [
      new FormField({ label: "MongoDB Connection" }).TextField({
        name: "mongoDbUrl",
        control: new FormControl('', [Validators.required])
      }),
      new FormField({ label: "MongoDB Name" }).TextField({
        name: "mongoDbName",
        control: new FormControl('', [Validators.required])
      }),
      new FormField({ label: "MongoDB Collection" }).TextField({
        name: "mongoDbCollection",
        control: new FormControl('', [Validators.required])
      }),
      new FormField({ label: "Transfer Size" }).SelectField({
        name: "transferSize",
        control: new FormControl('', [Validators.required])
      },
      [10, 20, 50, 100, 1000, 10000])
    ];
  }

  getAllTables(excludeTable: string) {
    let tables = this.dbSchema.map((x: { table: any; }) => x.table);
    const index = tables.findIndex((x: string) => x == excludeTable);
    if (index > -1) {
      tables.splice(index, 1);
    }
    return tables;
  }

  showNestedCheckBox(index: number) {
    return !this.dbSchema[index].mainCheck && this.dbSchema[index].table != this.template.mainTable?.tableName
  }

  getColumns(index: number) {
    return this.dbSchema[index].columns;
  }

  onMainTableSelect(table: string, index: number) {
    this.dbSchema[index].nestedCheck = false;
    this.dbSchema[index].selectedColumns = [];

    let mainTable = this.dbSchema.find((x: { table: string; }) => x.table == table);
    if (mainTable)
      this.targetColumns = mainTable!.columns;

    const lastMainTableIndex = this.dbSchema.findIndex(x => x.table == this.template.mainTable!.tableName);
    if (lastMainTableIndex > -1) {
      this.dbSchema[lastMainTableIndex].selectedColumns = [];
    }

    this.template.mainTable!.tableName = table;
    this.template.mainTable!.select = [];
    this.template.mainTable!.conditions = "";

    const searchIndex = this.template.nestedTables!.findIndex(x => x.tableName == table);
    if (searchIndex > -1) {
      this.template.nestedTables?.splice(searchIndex, 1);
    }

    this.updateTempate();
  }

  onNestedTableSelect(table: string, checked: boolean, index: number) {
    this.dbSchema[index].mainCheck = false;
    this.dbSchema[index].selectedColumns = [];
    if (table == this.template.mainTable!.tableName) {
      this.template.mainTable!.tableName = "";
      this.template.mainTable!.select = [];
    }
    this.dbSchema[index].nestedCheck = checked;

    if (checked) {
      let nestedObject = new NestedTable({ tableName: table, select: [] });
      this.template.nestedTables?.push(nestedObject)
    }
    else {
      const index = this.template.nestedTables!.findIndex(x => x.tableName == table);
      if (index > -1) {
        this.template.nestedTables?.splice(index, 1);
      }
    }
    this.updateTempate();
  }

  onColumnSelect(table: string, value: string, event: any) {
    if (this.template.mainTable?.tableName == table) {
      if (this.template.mainTable!.select == undefined)
        this.template.mainTable!.select = [];

      if (event.target.checked && !(this.template.mainTable.select!.indexOf(value) > -1)) {
        this.template.mainTable.select.push(value);
      }
      else {
        const index = this.template.mainTable.select.indexOf(value, 0);
        if (index > -1) {
          this.template.mainTable.select.splice(index, 1);
        }
      }
    }
    else {
      let existingNestedTable = this.template.nestedTables?.find(x => x.tableName == table);
      if (event.target.checked && !(existingNestedTable!.select!.indexOf(value) > -1)) {
        existingNestedTable?.select?.push(value);
      }
      else {
        const index = existingNestedTable!.select!.indexOf(value, 0);
        if (index > -1) {
          existingNestedTable!.select!.splice(index, 1);
        }
      }
    }

    this.updateTempate();
  }

  onObjectNameSelect(index: number, selectName: string, sourceTable: string) {
    let nestedObject = this.template.nestedTables?.find(x => x.tableName == sourceTable);
    nestedObject!.objectIdentifier = selectName;
    this.updateTempate();
  }

  onSourceIdSelect(index: number, selectId: string, sourceTable: string) {
    let nestedObject = this.template.nestedTables?.find(x => x.tableName == sourceTable);
    nestedObject!.skey = selectId;
    this.updateTempate();
  }

  onTargetIdSelect(index: number, selectId: string, sourceTable: string) {
    let nestedObject = this.template.nestedTables?.find(x => x.tableName == sourceTable);
    nestedObject!.tkey = selectId;
    this.updateTempate();
  }

  updateTempate() {
    this.displayTemplate = JSON.stringify(this.template, undefined, 4);
  }

  public getDbSchema(event: any) {
    let sqlSetting = new Settings({ connection: event.sqlDbUrl, database: event.sqlDbName });
    this.template.settings = new TemplateSettings({ sql: sqlSetting })
    this.updateTempate();

    let schemaRequest = new DbSchemaRequest({ url: sqlSetting.connection, dbName: sqlSetting.database, filter: event.filterTable });

    this.MigrationServiceRef.fetchSchema(schemaRequest)
      .subscribe(schemaInfo => {
        this.dbSchema = schemaInfo
      });
  }

  public saveMongoSettings(event: any) {
    let monogSetting = new Settings({ connection: event.mongoDbUrl, database: event.mongoDbName, collection: event.mongoDbCollection });
    this.template.settings = new TemplateSettings({ mongo: monogSetting, sql: this.template.settings?.sql })
    this.template.transferSize = +event.transferSize
    this.updateTempate();
  }

  public canGetSampleDocument() {
    return this.template.settings?.sql &&
      this.template.settings?.mongo &&
      this.template.nestedTables.length > 0 &&
      this.template.nestedTables[0].select.length > 0 &&
      this.template.nestedTables[0].objectIdentifier &&
      this.template.nestedTables[0].skey &&
      this.template.nestedTables[0].tkey &&
      this.template.nestedTables[0].tableName &&
      this.template.mainTable.select.length > 0 &&
      this.template.mainTable.tableName;
  }

  public getSampleDocument() {
    this.MigrationServiceRef.fetchSampleDocument(this.template)
      .subscribe(response => {
        this.sampleDocument = response
      });
  }

  public migrateData() {
    this.migrationResponse = "Migration Started..."
    this.MigrationServiceRef.migrate(this.template)
      .subscribe(response => {
        this.migrationResponse = response.Message;
      });
  }
}


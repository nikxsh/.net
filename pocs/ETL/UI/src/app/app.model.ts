class DbSchemaRequest {
	url?: string
	dbName?: string
	filter?: string

    public constructor(init?:Partial<DbSchemaRequest>) {
        Object.assign(this, init);
    }
}

interface DbSchemaResponse {
	table: string
	columns: string[]
	selectedColumns: string[]
	mainCheck: boolean
	nestedCheck: boolean
	objectName: string
	sourceId: string
	targetTable: string
	targetId: string	
}

class Template {
	settings?: TemplateSettings
	mainTable?: Table
	nestedTables?: NestedTable[]
	transferSize?: number = 0

    public constructor(init?:Partial<Template>) {
        Object.assign(this, init);
    }
}

class TemplateSettings {
	sql?: Settings
	mongo?: Settings

    public constructor(init?:Partial<TemplateSettings>) {
        Object.assign(this, init);
    }
}

class Settings {
	connection?: string
	database?: string
	collection?: string

    public constructor(init?:Partial<Settings>) {
        Object.assign(this, init);
    }
}

class Table {
	tableName?: string
	select?: string[]
	conditions?: string

    public constructor(init?:Partial<Table>) {
        Object.assign(this, init);
    }
}

class NestedTable {
	tableName?: string
	select?: string[]
	conditions?: string
	objectIdentifier?: string
	parent?: string
	skey?: string
	tkey?: string

    public constructor(init?:Partial<NestedTable>) {
        Object.assign(this, init);
    }
}

export { DbSchemaRequest, DbSchemaResponse, Template, TemplateSettings, Settings, Table, NestedTable };
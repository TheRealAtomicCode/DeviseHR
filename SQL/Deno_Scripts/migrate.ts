import { Client } from 'https://deno.land/x/postgres/mod.ts';
import { config } from 'https://deno.land/x/dotenv/mod.ts';

// Load environment variables from .env file
const { DB_USER, DB_PASSWORD, DB_NAME, DB_HOST, DB_PORT } = config();

const connectionConfig = {
	user: DB_USER, // Use environment variable
	password: DB_PASSWORD, // Use environment variable
	database: DB_NAME, // Use environment variable
	hostname: DB_HOST, // Use environment variable
	port: Number(DB_PORT), // Convert port to number
};

export default async function runSqlFiles(): Promise<void> {
	const sqlFilePaths: string[] = [
		// * scripts and types
		'../Scripts/Drop_Script.sql',
		// * tables
		'../DB_Types/Contract_Record.sql',
		'../Tables/Operator.sql',
		'../Tables/Note.sql',
		'../Tables/Company.sql',
		'../Tables/Permission.sql',
		'../Tables/Employee.sql',
		'../Tables/Hierarchy.sql',
		'../Tables/Absence_Type.sql',
		'../Tables/Working_Pattern.sql',
		'../Tables/Contract.sql',
		// "../Tables/Discarded_Contracts.sql",
		// "../Tables/Leave_Years.sql",
		'../Tables/Absence.sql',
		// "../Tables/Terms.sql",

		// * functions
		// './Database/Functions/edit_user_roles.sql',
		// './Database/Functions/edit_subordinates.sql',
		// './Database/Functions/update_last_contract_end_date.sql',
		// './Database/Functions/add_absence.sql',
		// * triggers

		// * final scripts
		'../Scripts/Insert_Absence_Type.sql',
	];

	const client = new Client(connectionConfig);
	try {
		await client.connect();

		for (const filePath of sqlFilePaths) {
			const sql = await Deno.readTextFile(filePath);
			await client.queryArray(sql);
			console.log(`SQL file ${filePath} executed successfully.`);
		}

		console.log('All SQL files executed successfully.');
	} catch (error) {
		console.error('Error executing SQL files:', error);
	} finally {
		await client.end(); // Close the database connection
	}
}
